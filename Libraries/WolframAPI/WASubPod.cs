

namespace WolframAPI
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// The subpod element in the response.
    /// </summary>
    [Serializable, CLSCompliant(true)]
    public sealed class WASubPod
    {
        /// <summary>
        /// Gets or sets the subpod's title.
        /// </summary>
        [XmlAttribute("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the expression submitted as plain text.
        /// </summary>
        [XmlElement("plaintext")]
        public string PlainText { get; set; }

        /// <summary>
        /// Gets or sets the image representation of the submitted expression.
        /// </summary>
        [XmlElement("img")]
        public WAImage Image { get; set; }
    }
}
