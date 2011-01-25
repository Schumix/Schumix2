
namespace WolframAPI
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// The pod element in the response.
    /// </summary>
    [Serializable, CLSCompliant(true)]
    public sealed class WAPod
    {
        /// <summary>
        /// Gets or sets the pod's title.
        /// </summary>
        [XmlAttribute("title")]
        public string Title { get; set; }

        /// <summary>
        /// No information.
        /// Seen: Identity
        /// </summary>
        [XmlAttribute("scanner")]
        public string Scanner { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the pod.
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the pod position.
        /// </summary>
        [XmlAttribute("position")]
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there was an error in this pod or not.
        /// </summary>
        [XmlAttribute("error")]
        public bool Error { get; set; }

        /// <summary>
        /// Gets or sets the number of sub pods.
        /// </summary>
        [XmlAttribute("numsubpods")]
        public int NumSubPods { get; set; }

        /// <summary>
        /// Gets or sets the list of sub pods.
        /// </summary>
        [XmlElement("subpod")]
        public List<WASubPod> SubPods { get; set; }   
    }
}
