namespace WolframAPI
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Xml.Serialization;
    using Exceptions;

    /// <summary>
    /// Used to handle the response received event which occurs when a response is successfully
    /// retrieved using the API.
    /// </summary>
    /// <param name="response">The (raw) response returned.</param>
    /// <param name="expression">The expression submitted.</param>
    public delegate void ResponseReceivedEventHandler(string response, string expression);

    /// <summary>
    /// Used to access Wolfram Alpha. 
    /// Submits expressions, retrieves and parses responses.
    /// </summary>
    public sealed class WAClient
    {
        /// <summary>
        /// The base WA API url.
        /// </summary>
        public const string BaseUrl =
            "http://api.wolframalpha.com/v2/query?appid={0}&input={1}&format=image,plaintext";

        /// <summary>
        /// The application ID.
        /// </summary>
        private readonly string _appId;

        /// <summary>
        /// Occurs when a response is successfully retrieved using the API.
        /// </summary>
        public event ResponseReceivedEventHandler OnResponseReceived;


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
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(expression));
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            var response = Submit(expression);
            var result = Parse(response);
            
            if(result.Pods == null || result.Pods.Count <= 0)
            {
                return "No solution found. The response might have been malformed.";
            }

            var solution = (from pod in result.Pods
                           where pod.Title.ToLower().Contains("solution") 
                           || pod.Title.ToLower().Contains("result")
                           || pod.Title.ToLower().Contains("derivative")
                           || pod.Title.ToLower().Contains("decimal form")
                           select pod).FirstOrDefault();

            if(solution == null)
            {
                return "No solution found.";
            }


            if (solution.SubPods == null || solution.SubPods.Count <= 0)
            {
                return "No solution found. The response might have been malformed.";
            }

            Contract.Assume(solution.SubPods[0] != null);

            if (string.IsNullOrEmpty(solution.SubPods[0].PlainText))
            {
                return "No solution found. The pod order might have changed. Report to devs!";
            }
            
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
            if (string.IsNullOrEmpty(expression))
            {
                throw new ArgumentNullException("expression", "The parameter passed to this method was null or empty.");
            }

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
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(expression));
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            try
            {
                expression = expression.Replace("=", " = ");
                var wacode = HttpUtility.UrlEncode(expression);

                var url = new Uri(string.Format(BaseUrl, _appId, wacode));

                string returned;

                using (var client = new WebClient())
                {
                    returned = client.DownloadString(url);
                }

                if(!string.IsNullOrEmpty(returned))
                {
                    if(OnResponseReceived != null)
                    {
                        OnResponseReceived(returned, expression);
                    }

                    return returned;
                }

                return "Couldn't retrieve information!";
            }
            catch(WebException x)
            {
                throw new WolframException("WebException thrown while getting the response.", x);
            }
            catch(Exception x)
            {
                throw new WolframException("Unhandled exception thrown while submitting the expression.", x);
            }
        }

        /// <summary>
        /// Parses the raw response.
        /// </summary>
        /// <param name="response">The response to parse</param>
        /// <returns>The parsed response</returns>
        /// <exception cref="WolframException">Throws in case of any error.</exception>
        /// <exception cref="ArgumentNullException">Throws if the specified argument is null.</exception>
        public WAResult Parse(string response)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(response));
            Contract.Ensures(Contract.Result<WAResult>() != null);

            try
            {
                var serializer = new XmlSerializer(typeof (WAResult));

                WAResult result;

                using (var reader = new StringReader(response))
                {
                    result = serializer.Deserialize(reader) as WAResult;
                }

                if(result == null)
                {
                    throw new WolframException("Could not deserialize the response. It might have been a malformed one.");
                }

                return result;
            }
            catch(InvalidOperationException x)
            {
                throw new WolframException("Exception thrown while deserializing the response.", x);
            }
        }
    }
}
