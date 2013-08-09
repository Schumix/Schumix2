/*
 * This file is part of Schumix.
 * 
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
using Schumix.Framework;
using NGit;

namespace Schumix.Components.Updater.Download
{
	sealed class NGitProgressMonitor : ProgressMonitor
	{
		private int _totalwork;
		private int _completed;

		public NGitProgressMonitor() {}

		~NGitProgressMonitor()
		{
			Console.CursorVisible = true;
			Console.WriteLine();
		}

		// Do not report.
		public override void Start(int totalTasks)
		{
		}

		public override void BeginTask(string title, int totalWork)
		{
			if(totalWork <= 0)
				return;

			Console.CursorVisible = false;

			if(_totalwork > 0)
				drawTextProgressBar(_totalwork, _totalwork);

			_completed = 0;
			_totalwork = totalWork;
			Console.WriteLine();
			Console.WriteLine(title);
			drawTextProgressBar(0, totalWork);
		}

		public override void Update(int completed)
		{
			if(completed >= _totalwork)
				return;

			_completed += completed;
			drawTextProgressBar(_completed, _totalwork);
		}

		// Do not report.
		public override bool IsCancelled()
		{
			return false;
		}

		public override void EndTask()
		{
			drawTextProgressBar(_totalwork, _totalwork);
			Console.CursorVisible = true;
		}

		private void drawTextProgressBar(int progress, int total)
		{
			// Draw empty progress bar
			Console.CursorLeft = 0;
			Console.Write("["); // Start
			Console.CursorLeft = 32;
			Console.Write("]"); // End
			Console.CursorLeft = 1;
			float onechunk = 30.0f / total;

			// Draw filled part
			int position = 1;

			if(progress == 0)
			{
				Console.BackgroundColor = ConsoleColor.Gray;
				Console.CursorLeft = position;
				Console.Write(SchumixBase.Space);
			}
			else
			{
				for(int i = 0; i < onechunk * progress; i++)
				{
					Console.BackgroundColor = ConsoleColor.Gray;
					Console.CursorLeft = position++;
					Console.Write(SchumixBase.Space);
				}
			}

			if(progress == total)
			{
				for(int i = position; i <= 31; i++)
				{
					Console.BackgroundColor = ConsoleColor.Gray;
					Console.CursorLeft = position++;
					Console.Write(SchumixBase.Space);
				}
			}
			else
			{
				// Draw unfilled part
				for(int i = position; i <= 31; i++)
				{
					Console.BackgroundColor = ConsoleColor.Black;
					Console.CursorLeft = position++;
					Console.Write(SchumixBase.Space);
				}
			}

			// Draw totals
			Console.CursorLeft = 35;
			Console.BackgroundColor = ConsoleColor.Black;
			Console.Write(progress.ToString() + " of " + total.ToString() + "    "); // Blanks at the end remove any excess
		}
	}
}