namespace WolframAPI.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Provides a list type which supports only unique list of elements.
    /// </summary>
    [Serializable]
    public sealed class UniqueList<T> : List<T>
        where T : IEquatable<T>, IEquatable<string>
    {
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public new void Add(T item)
        {
            Contract.Requires(!Equals(item, null));
            
            if (Contains(item) || FindIndex(it => it.Equals(item)) != -1)
            {
                throw new InvalidOperationException("The unique list already contains that element.");
            }

            base.Add(item);
        }

        /// <summary>
        /// Gets or sets the element with the specified index.
        /// </summary>
        /// <value></value>
        public T this[T ind]
        {
            get
            {
                Contract.Requires(!Equals(ind, null));

                return (from elem in this
                        where elem.Equals(ind)
                        select elem).FirstOrDefault();
            }

            set
            {
                Contract.Requires(!Equals(ind, null));
                Contract.Requires(!Equals(value, null));

                int index;

                if ((index = FindIndex(it => it.Equals(ind))) != -1)
                {
                    this[index] = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the element with the specified index.
        /// </summary>
        /// <value></value>
        public T this[string ind]
        {
            get
            {
                return (from elem in this
                        where elem.Equals(ind)
                        select elem).FirstOrDefault();
            }

            set
            {
                int index;

                if ((index = FindIndex(it => it.Equals(ind))) != -1)
                {
                    this[index] = value;
                }
            }
        }
    }
}
