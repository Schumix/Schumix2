/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013 Schumix Team <http://schumix.eu/>
 * 
 * Schumix is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * Schumix is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with Schumix.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.IO;
using System.Net;
using System.Web;
using System.Linq;
using System.Reflection;
using System.Diagnostics.Contracts;
using System.Runtime.Remoting.Messaging;
using System.Xml.Serialization;
using WolframAPI.Exceptions;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace WolframAPI
{
	/// <summary>
	/// Used to handle the response received event which occurs when a response is successfully
	/// retrieved using the API.
	/// </summary>
	/// <param name="response">The (raw) response returned.</param>
	/// <param name="expression">The expression submitted.</param>
	public delegate void SolutionReceivedEventHandler(string response, string expression = "");

	/// <summary>
	/// Used to handle the result response which is used in the <see cref="WAClient.GetResult"/> method.
	/// </summary>
	/// <param name="result">The result received</param>
	/// <param name="expression">The expression submitted</param>
	public delegate void ResultReceivedEventHandler(WAResult result, string expression = "");

	/// <summary>
	/// Used to asynchronously call the expression processor methods.
	/// <param name="expression">The expression submitted.</param>
	/// </summary>
	public delegate string ExpressionProcessorMethod(string expression);

	/// <summary>
	/// Used to asynchronously fetch the result from Wolfram.
	/// </summary>
	/// <param name="expression">Expression to submit.</param>
	/// <returns>The fetched result.</returns>
	public delegate WAResult RetrieveResultMethod(string expression);

	/// <summary>
	/// Used to access Wolfram Alpha. 
	/// Submits expressions, retrieves and parses responses.
	/// </summary>
	public sealed class WAClient
	{
		private static readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		//private readonly Utilities sUtilities = Singleton<Utilities>.Instance;

		/// <summary>
		/// The base WA API url.
		/// </summary>
		public const string BaseUrl = "http://api.wolframalpha.com/v2/query?appid={0}&input={1}&format=image,plaintext";

		/// <summary>
		/// The application ID.
		/// </summary>
		private readonly string _appId;

		/// <summary>
		/// Occurs when a response is successfully retrieved using the API.
		/// </summary>
		public event SolutionReceivedEventHandler OnSolutionReceived;

		/// <summary>
		/// Occurs when a result is successfully retrieved using the API.
		/// </summary>
		public event ResultReceivedEventHandler OnResultReceived;

		private static readonly object SyncLock = new object();

		/// <summary>
		/// Gets the version of the currently used library.
		/// </summary>
		/// <value>The version.</value>
		public static string Version
		{
			get
			{
				return (Assembly.GetExecutingAssembly().GetName().Version.ToString());
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WAClient"/> class.
		/// </summary>
		/// <param name="appId">The application ID provided by Wolfram. You have to request one for each of your apps.</param>
		public WAClient(string appId)
		{
			_appId = appId;
		}

		/// <summary>
		/// Solves the specified expression.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <exception cref="WolframException">Throws in case of any error.</exception>
		/// <exception cref="ArgumentNullException">Throws if the specified argument is null.</exception>
		/// <returns>The solution of the given expression</returns>
		public string Solve(string expression)
		{
			//if(sPlatform.GetPlatformType() != PlatformType.Linux)
			//{
			//	Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(expression));
			//	Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
			//}

			var response = Submit(expression);
			var result = Parse(response);
			
			if(result.Pods.IsNull() || result.Pods.Count <= 0)
				return sLConsole.GetString("No solution found. The response might have been malformed.");

			var solution = (from pod in result.Pods
						   where pod.Title.ToLower().Contains("solution") 
						   || pod.Title.ToLower().Contains("result")
						   || pod.Title.ToLower().Contains("derivative")
						   || pod.Title.ToLower().Contains("decimal form")
						   select pod).FirstOrDefault();

			if(solution.IsNull())
				return sLConsole.GetString("No solution found.");


			if(solution.SubPods.IsNull() || solution.SubPods.Count <= 0)
				return sLConsole.GetString("No solution found. The response might have been malformed.");

			//if(sPlatform.GetPlatformType() != PlatformType.Linux)
			//	Contract.Assume(!solution.SubPods[0].IsNull());

			if(string.IsNullOrEmpty(solution.SubPods[0].PlainText))
				return sLConsole.GetString("No solution found. The pod order might have changed. Report to devs!");

			return solution.SubPods[0].PlainText;
		}

		/// <summary>
		/// Gets the result of the specified expression.
		/// <para>The expression is returned as <see cref="WAResult"/></para> 
		/// so you can manually go through the pods of the response (to get ANY information you'd like)
		/// <para>It is encouraged to use this method instead of <see cref="Solve"/></para>
		/// </summary>
		/// <param name="expression">The expression to solve.</param>
		/// <exception cref="WolframException">Throws in case of any error.</exception>
		/// <exception cref="ArgumentNullException">Throws if the specified argument is null.</exception>
		/// <returns>The result</returns>
		public WAResult GetResult(string expression)
		{
			if(string.IsNullOrEmpty(expression))
				throw new ArgumentNullException("expression", sLConsole.GetString("The parameter passed to this method was null or empty."));

			var response = Submit(expression);
			var result = Parse(response);
			return result;
		}

		/// <summary>
		/// Submits the specified expression and returns the raw result.
		/// </summary>
		/// <param name="expression">The expression to post.</param>
		/// <exception cref="WolframException">Throws in case of any error.</exception>
		/// <exception cref="ArgumentNullException">Throws if the specified argument is null.</exception>
		/// <returns>Raw response</returns>
		public string Submit(string expression)
		{
			//if(sPlatform.GetPlatformType() != PlatformType.Linux)
			//{
			//	Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(expression));
			//	Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
			//}

			try
			{
				expression = expression.Replace("=", " = ");
				var wacode = HttpUtility.UrlEncode(expression);
				var url = new Uri(string.Format(BaseUrl, _appId, wacode));
				string returned;

				using(var client = new WebClient())
				{
					returned = client.DownloadString(url);
				}

				if(!string.IsNullOrEmpty(returned))
					return returned;

				return sLConsole.GetString("Couldn't retrieve information!");
			}
			catch(WebException x)
			{
				throw new WolframException(sLConsole.GetString("WebException thrown while getting the response."), x);
			}
			catch(Exception x)
			{
				throw new WolframException(sLConsole.GetString("Unhandled exception thrown while submitting the expression."), x);
			}
		}

		/// <summary>
		/// Parses the raw response.
		/// </summary>
		/// <param name="response">The response to parse</param>
		/// <returns>The parsed response</returns>
		/// <exception cref="WolframException">Throws in case of any error.</exception>
		/// <exception cref="ArgumentNullException">Throws if the specified argument is null.</exception>
		public static WAResult Parse(string response)
		{
			//if(sPlatform.GetPlatformType() != PlatformType.Linux)
			//{
			//	Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(response));
			//	Contract.Ensures(!Contract.Result<WAResult>().IsNull());
			//}

			try
			{
				var serializer = new XmlSerializer(typeof (WAResult));
				WAResult result;

				using(var reader = new StringReader(response))
				{
					result = serializer.Deserialize(reader) as WAResult;
				}

				if(result.IsNull())
					throw new WolframException(sLConsole.GetString("Could not deserialize the response. It might have been a malformed one."));

				return result;
			}
			catch(InvalidOperationException x)
			{
				throw new WolframException(sLConsole.GetString("Exception thrown while deserializing the response."), x);
			}
		}

		#region Async stuff

#if WITH_ASYNC
		private void HandleSolutionReceived(IAsyncResult ar)
		{
			//if(sPlatform.GetPlatformType() != PlatformType.Linux)
			//{
			//	Contract.Requires<ArgumentNullException>(!ar.IsNull());
			//	Contract.Requires<ArgumentNullException>(!((AsyncResult)ar).AsyncDelegate.IsNull());
			//}

			if(!ar.IsNull())
			{
				var expr = ar.AsyncState as string;
				var proc = (ExpressionProcessorMethod)(((AsyncResult) ar).AsyncDelegate);

				if(!proc.IsNull())
				{
					var solution = proc.EndInvoke(ar);

					if(!OnSolutionReceived.IsNull())
						OnSolutionReceived(solution, expr);
				}
			}
		}

		/// <summary>
		/// Solves the specified expression asynchronously.
		/// <remarks>An event is raised on completion.</remarks>
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <exception cref="WolframException">Throws in case of any error.</exception>
		/// <exception cref="ArgumentNullException">Throws if the specified argument is null.</exception>
		/// <returns>The solution of the given expression</returns>
		public void SolveAsync(string expression)
		{
			//if(sPlatform.GetPlatformType() != PlatformType.Linux)
			//	Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(expression));

			var procedure = new ExpressionProcessorMethod(Solve);

			lock(SyncLock)
			{
				procedure.BeginInvoke(expression, HandleSolutionReceived, expression);
			}
		}

		private void HandleResultReceived(IAsyncResult ar)
		{
			//if(sPlatform.GetPlatformType() != PlatformType.Linux)
			//{
			//	Contract.Requires<ArgumentNullException>(!ar.IsNull());
			//	Contract.Requires<ArgumentNullException>(!((AsyncResult)ar).AsyncDelegate.IsNull());
			//}

			if(!ar.IsNull())
			{
				var expr = ar.AsyncState as string;
				var proc = (RetrieveResultMethod)(((AsyncResult)ar).AsyncDelegate);

				if(!proc.IsNull())
				{
					var result = proc.EndInvoke(ar);

					if(!OnResultReceived.IsNull())
						OnResultReceived(result, expr);
				}
			}
		}

		/// <summary>
		/// Gets the result of the specified expression asynchronously.
		/// <para>The expression is returned as <see cref="WAResult"/></para> 
		/// so you can manually go through the pods of the response (to get ANY information you'd like)
		/// <para>It is encouraged to use this method instead of <see cref="Solve"/></para>
		/// <remarks>An event is raised upon completion.</remarks>
		/// </summary>
		/// <param name="expression">The expression to solve.</param>
		/// <exception cref="WolframException">Throws in case of any error.</exception>
		/// <exception cref="ArgumentNullException">Throws if the specified argument is null.</exception>
		/// <returns>The result</returns>
		public void GetResultAsync(string expression)
		{
			//if(sPlatform.GetPlatformType() != PlatformType.Linux)
			//	Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(expression));

			var procedure = new RetrieveResultMethod(GetResult);

			lock(SyncLock)
			{
				procedure.BeginInvoke(expression, HandleResultReceived, expression);
			}
		}

#endif
		#endregion
	}
}