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
using System.IO;
using System.Reflection;
using ICSharpCode.SharpZipLib.Zip;

namespace Schumix.Installer.UnZip
{
	sealed class GZip
	{
		public string DirectoryName { get; private set; }

		public GZip()
		{
			Zip("schumix.zip");
		}

		private void Zip(string FileName)
		{
			using(var s = new ZipInputStream(File.OpenRead(FileName)))
			{
				int i = 0;
				ZipEntry theEntry;

				while((theEntry = s.GetNextEntry()) != null)
				{
					string fileName      = Path.GetFileName(theEntry.Name);
					string directoryName = Path.GetDirectoryName(theEntry.Name);

					if(i == 0)
						DirectoryName = directoryName;

					// create directory
					if(directoryName.Length > 0)
						Directory.CreateDirectory(directoryName);

					if(fileName != string.Empty)
					{
						using(var streamWriter = File.Create(theEntry.Name))
						{
							int size = 2048;
							byte[] data = new byte[2048];

							while(true)
							{
								size = s.Read(data, 0, data.Length);

								if(size > 0)
									streamWriter.Write(data, 0, size);
								else
									break;
							}
						}
					}

					i++;
				}
			}
		}
	}
}