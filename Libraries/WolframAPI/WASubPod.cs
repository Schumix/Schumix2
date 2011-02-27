namespace WolframAPI
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// The subpod element in the response.
    /// </summary>
    [Serializable, CLSCompliant(true)]
    public sealed class WASubPod : IEquatable<WASubPod>, IEquatable<string>
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

        #region Interface Implementations

        #region Implementation of IEquatable<WASubPod>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(WASubPod other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Title, Title) && Equals(other.PlainText, PlainText) && Equals(other.Image, Image);
        }

        #endregion

        #region Implementation of IEquatable<string>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(string other)
        {
            return other.Equals(Title);
        }

        #endregion

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (WASubPod)) return false;
            return Equals((WASubPod) obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = (Title != null ? Title.GetHashCode() : 0);
                result = (result*397) ^ (PlainText != null ? PlainText.GetHashCode() : 0);
                result = (result*397) ^ (Image != null ? Image.GetHashCode() : 0);
                return result;
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(WASubPod left, WASubPod right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(WASubPod left, WASubPod right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
