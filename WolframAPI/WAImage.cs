
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
