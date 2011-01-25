

namespace WolframAPI
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// The QueryResult part of the response.
    /// </summary>
    [Serializable, CLSCompliant(true), XmlRoot("queryresult")]
    public sealed class WAResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the processing was successful or not.
        /// </summary>
        [XmlAttribute("success")]
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there was an error or not.
        /// </summary>
        [XmlAttribute("error")]
        public bool Error { get; set; }

        /// <summary>
        /// Gets or sets the number of pods in the response.
        /// </summary>
        [XmlAttribute("numpods")]
        public int NumPods { get; set; }

        /// <summary>
        /// Gets or sets the data types passed in the response.
        /// Usually blank.
        /// </summary>
        [XmlAttribute("datatypes")]
        public string DataTypes { get; set; }

        /// <summary>
        /// Gets or sets the timedout value.
        /// It's a string because it's empty in the response.
        /// Probaby set to true or something in case of timeout.
        /// </summary>
        [XmlAttribute("timedout")]
        public string TimedOut { get; set; }

        /// <summary>
        /// Gets or sets the time it took for the response.
        /// </summary>
        [XmlAttribute("timing")]
        public double Timing { get; set; }

        /// <summary>
        /// Gets or sets the time it took to parse.
        /// </summary>
        [XmlAttribute("parsetiming")]
        public double ParseTiming { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the parse timed out or not.
        /// </summary>
        [XmlAttribute("parsetimedout")]
        public bool ParseTimedOut { get; set; }

        /// <summary>
        /// Gets or sets a value which is empty when there is no recalculation (?)
        /// </summary>
        [XmlAttribute("recalculate")]
        public string Recalculate { get; set; }

        /// <summary>
        /// Gets or sets the API's version.
        /// </summary>
        [XmlAttribute("version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the list of pods.
        /// </summary>
        [XmlElement("pod")]
        public List<WAPod> Pods { get; set; }
    }
}