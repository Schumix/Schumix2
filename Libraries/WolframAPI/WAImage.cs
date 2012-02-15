/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
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

namespace WolframAPI
{
	using System;
	using System.Xml.Serialization;

	/// <summary>
	/// Provides the Image (img) element's datas.
	/// </summary>
	[Serializable, CLSCompliant(true)]
	public sealed class WAImage
	{
		/// <summary>
		/// Gets or sets the source (url) where the image resides.
		/// </summary>
		[XmlAttribute("src")]
		public string Source { get; set; }

		/// <summary>
		/// Gets or sets the alternative form of the image.
		/// Usually the text representation of the image.
		/// </summary>
		[XmlAttribute("alt")]
		public string Alt { get; set; }

		/// <summary>
		/// Gets or sets the image's title.
		/// </summary>
		[XmlAttribute("title")]
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the image's width
		/// </summary>
		[XmlAttribute("width")]
		public int Width { get; set; }

		/// <summary>
		/// Gets or sets the image's height
		/// </summary>
		[XmlAttribute("height")]
		public int Height { get; set; }
	}
}