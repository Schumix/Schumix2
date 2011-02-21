namespace WolframAPI
{
    internal interface ISerializableType
    {
        /// <summary>
        /// Serializes the current instance and returns the result as a <see cref="string"/>
        /// <para>Should be used with XML serialization only.</para>
        /// </summary>
        /// <returns>The serialized instance.</returns>
        string Serialize();
    }
}
