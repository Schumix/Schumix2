namespace WolframAPI
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Xml.Serialization;

    /// <summary>
    /// The QueryResult (main) part of the response.
    /// </summary>
    [Serializable, CLSCompliant(true), XmlRoot("queryresult")]
    public sealed class WAResult : XmlSerialized, IEquatable<WAResult>, IEquatable<string>, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WAResult"/> class.
        /// </summary>
        public WAResult()
        {
            
        }

        internal WAResult(bool success, bool error, int numPods, string dataTypes, string timedOut, double timing, double parseTiming, bool parseTimedOut, string recalculate, string version, List<WAPod> pods)
        {
            Success = success;
            Error = error;
            NumPods = numPods;
            DataTypes = dataTypes;
            TimedOut = timedOut;
            Timing = timing;
            ParseTiming = parseTiming;
            ParseTimedOut = parseTimedOut;
            Recalculate = recalculate;
            Version = version;
            Pods = pods;
        }

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

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(WAResult other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Success.Equals(Success) && other.Error.Equals(Error) && other.NumPods == NumPods && Equals(other.DataTypes, DataTypes) && Equals(other.TimedOut, TimedOut) && other.Timing.Equals(Timing) && other.ParseTiming.Equals(ParseTiming) && other.ParseTimedOut.Equals(ParseTimedOut) && Equals(other.Recalculate, Recalculate) && Equals(other.Version, Version) && Equals(other.Pods, Pods);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(string other)
        {
            return (ToString() == other);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(WAResult a, WAResult b)
        {
            return ReferenceEquals(a, b) || (a.Equals(b));
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(WAResult a, WAResult b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Returns the <see cref="string"/> representation of this <see cref="WAResult"/> instance.
        /// </summary>
        /// <returns>The string representation</returns>
        public override string ToString()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            var sd = Serialize();

            if (string.IsNullOrEmpty(sd))
                sd = GetType().FullName;
            
            return sd;
            
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == typeof (WAResult) && Equals((WAResult) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var result = Success.GetHashCode();
                result = (result*397) ^ Error.GetHashCode();
                result = (result*397) ^ NumPods;
                result = (result*397) ^ (DataTypes != null ? DataTypes.GetHashCode() : 0);
                result = (result*397) ^ (TimedOut != null ? TimedOut.GetHashCode() : 0);
                result = (result*397) ^ Timing.GetHashCode();
                result = (result*397) ^ ParseTiming.GetHashCode();
                result = (result*397) ^ ParseTimedOut.GetHashCode();
                result = (result*397) ^ (Recalculate != null ? Recalculate.GetHashCode() : 0);
                result = (result*397) ^ (Version != null ? Version.GetHashCode() : 0);
                result = (result*397) ^ (Pods != null ? Pods.GetHashCode() : 0);
                return result;
            }
        }

        /// <summary>
        /// Creates a new object that is a copy of the current <see cref="WAResult"/> instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public object Clone()
        {
            Contract.Ensures(Contract.Result<object>() != null);

            return new WAResult(Success, Error, NumPods, DataTypes, TimedOut, Timing, ParseTiming, ParseTimedOut,
                                Recalculate, Version, Pods);
        }
    }
}