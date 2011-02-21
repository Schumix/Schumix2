using WolframAPI.Exceptions;

namespace WolframAPI
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Xml.Serialization;

    ///<summary>
    /// Base class for XML-serialized types.
    ///</summary>
    public abstract class XmlSerialized : ISerializableType
    {
        #region Implementation of ISerializableType

        /// <summary>
        /// Serializes the current instance and returns the result as a <see cref="string"/>
        /// <para>Should be used with XML serialization only.</para>
        /// </summary>
        /// <returns>The serialized instance.</returns>
        public string Serialize()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            string data;

            try
            {
                using (var ms = new MemoryStream())
                {
                    var serializer = new XmlSerializer(GetType());
                    var nss = new XmlSerializerNamespaces();
                    nss.Add("", "");

                    serializer.Serialize(ms, this, nss);

                    using (var reader = new StreamReader(ms))
                    {
                        data = reader.ReadToEnd();
                    }
                }
            }
            catch(InvalidOperationException x)
            {
                throw new WolframException("Error during serialization", x);
            }

            if(string.IsNullOrEmpty(data))
            {
                throw new WolframException(string.Format("Error while serializing instance! Type: {0}", GetType().FullName));
            }

            return data;
        }

        #endregion
    }
}
