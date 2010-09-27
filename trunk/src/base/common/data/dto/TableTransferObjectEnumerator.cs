using System;
using System.Collections;
using System.Text;

namespace Nohros.Data
{
    /// <summary>
    /// Exposes the <see cref="System.Collections.IEnumerable.GetEnumerator"/> method, which supports
    /// a simple iteration over a <see cref="Nohros.Data.TableTransferObject"/> object.
    /// </summary>
    public class TableTransferObjectEnumerator : IEnumerator
    {
        TableTransferObject table_;
        TableTransferObject.TableNode current_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance_ of the TableTransferObjectEnumerator object by using the specified
        /// <see cref="TableTransferObject"/>
        /// </summary>
        /// <param name="table">The <see cref="TableTransferObject"/> through which to iterate</param>
        /// <exception cref="ArgumentNullException"><paramref name="table"/> is null.</exception>
        public TableTransferObjectEnumerator(TableTransferObject table)
        {
            table_ = table;
            current_ = table.root_;
        }
        #endregion

        /// <summary>
        /// Advances the enumerator to the next row of the <see cref="TableTransferObject"/> object.
        /// </summary>
        /// <returns>true if the enumerator was sucessfully advanced to the next element; false if the
        /// enumerator has passed the end of the collection.</returns>
        public bool MoveNext()
        {
            if (current_ == null)
                return false;

            current_ = current_.next;

            return (current_ == null);
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first ROW
        /// in the <see cref="Nohros.Data.TableTransferObject"/>.
        /// </summary>
        public void Reset()
        {
            current_ = table_.root_;
        }

        /// <summary>
        /// Gets the values of the current row in the <see cref="Nohros.Data.TableTransferObject"/> object.
        /// </summary>
        /// <returns>An string array containing the values of the current row.</returns>
        /// <exception cref="InvalidOperationException">The enumerator is positioned before the first row of
        /// the <see cref="Nohros.Data.TableTransferObject"/> object of after the last row of the
        /// <see cref="Nohros.Data.TableTransferObject"/> object.</exception>
        public object Current
        {
            get {
                if (current_ == null || ReferenceEquals(current_, table_.root_))
                    throw new InvalidOperationException("The enumerator is positioned before the first element of the collection or after the last element.");

                return current_.values;
            }
        }
    }
}
