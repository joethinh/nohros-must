using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data
{
    /// <summary>
    /// A comparer using a comparison delegate for comparison between items.
    /// </summary>
    public sealed class ComparisonComparer<T> : IComparer<T>
    {
        Comparison<T> _comparison;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComparisonComparer"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">comparison is null</exception>
        public ComparisonComparer(Comparison<T> comparison)
        {
            if (comparison == null)
                throw new ArgumentNullException("comparison");
            _comparison = comparison;
        }

        /// <summary>
        ///  Gets or sets the comparison used in this comparer
        /// </summary>
        /// <exception cref="ArgumentNullException">The value assigned is a null reference</exception>
        public Comparison<T> Comparison
        {
            get { return _comparison; }
            set
            {
                if (_comparison == null)
                    throw new ArgumentNullException("value");
                _comparison = value;
            }
        }

        #region IComparer

        /// <inheritdoc />
        public int Compare(T x, T y)
        {
            return _comparison(x, y);
        }
        #endregion
    }
}
