using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data
{
    /// <summary>
    /// Represents one table of in-memory data. The purpose of this class is to provide a way
    /// to transfer tabular data between layers. It is optimized for sequential accesses.
    /// </summary>
    public class TableTransferObject : IDataTransferObject, IEnumerable
    {
        /// <summary>
        /// Represents a node of data in a <see cref="TableTransferObject"/>.
        /// </summary>
        #region TableNode
        internal class TableNode
        {
            /// <summary>
            /// An string array containing the values of the columns.
            /// </summary>
            public string[] values;

            /// <summary>
            /// An integer representing the index of the node within the <see cref="TableTransferObject"/>.
            /// </summary>
            public int index;

            /// <summary>
            /// The next node.
            /// </summary>
            public TableNode next;

            /// <summary>
            /// Initializes a new instance of the TableNode class with is empty and positioned
            /// at the index zero.
            /// </summary>
            public TableNode() { }

            /// <summary>
            /// Initialzes a new instance of the TableNode class by using the specified node index.
            /// </summary>
            /// <param name="r">The index of the node within the <see cref="TableTransferObject"/>.</param>
            public TableNode(int r) {
                index = r;
            }

            /// <summary>
            /// Initializes a new instance od the TableNode class by using the specified row number and values.
            /// </summary>
            /// <param name="r">The number of the row that this node represents</param>
            /// <param name="str">An string array containing the values of the columns</param>
            public TableNode(int r, string[] str) {
                index = r;
                values = str;
            }
        }
        #endregion

        static object lock_;

        string[] columns_;
        internal TableNode root_;
        TableNode current_node_;

        #region .ctor
        /// <summary>
        /// Static constructor.
        /// </summary>
        static TableTransferObject()
        {
            lock_ = new object();
        }

        /// <summary>
        /// Initializes a new instance of the TableTransferObject class by using the specified column names.
        /// </summary>
        /// <param name="columns">An string array containing the names of the columns</param>
        public TableTransferObject(params string[] columns)
        {
            if (columns == null)
                throw new ArgumentNullException("columns");

            if (columns.Length == 0)
                throw new ArgumentException("columns");

            root_ = new TableNode(0, null);

            // ensure that the column names are unique.
            columns_ = DataHelper.Unique(columns);
        }
        #endregion

        /// <summary>
        /// Adds the specified values to the a new row within the TableTransferObject object.
        /// </summary>
        /// <param name="values">The array of string object to add to the table</param>
        public void Add(params string[] values)
        {
            if (values.Length > columns_.Length)
                throw new ArgumentOutOfRangeException("values");

            string[] new_node_values = new string[columns_.Length];
            for (int i = 0; i < columns_.Length; i++)
                new_node_values[i] = values[i];

            lock (lock_) {
                current_node_.next = new TableNode();
                current_node_.next.index = current_node_.index + 1;
                current_node_ = current_node_.next;
                current_node_.values = new_node_values;
            }
        }

        /// <summary>
        /// Removes the node at the specified index of the TableTransferObject.
        /// </summary>
        /// <param name="index">The number of the row to remove</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than one.-or- index
        /// is greater than <see cref="Count"/></exception>
        /// <remarks>The TableTransferObject is optimized for sequential access.
        /// <para>Remove is a non-sequential method, which should be avoided in performance-sensitive code paths.</para>
        /// </remarks>
        public void Remove(int index)
        {
            if (index < 1 || index > current_node_.index)
                throw new ArgumentOutOfRangeException("index");

            TableNode node = root_;

            // traverse down the linked list searching for the specified index.
            for (int i = 1, j = node.index+1; i < j; ++i) {
                // to remove a node we need a reference to the previous node, so look-ahead
                if (node.next != null && node.next.index == index) {
                    lock(lock_) {
                        node = node.next = node.next.next;
                        while (node != null) --node.index;
                    }
                }
            }
        }

        #region IEnumerable
        /// <summary>
        /// Returns an enumerator that iterates through a colletion.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a colletion.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
        public TableTransferObjectEnumerator GetEnumerator()
        {
            return new TableTransferObjectEnumerator(this);
        }
        #endregion

        /// <summary>
        /// Gets the number of rows actually contained in the TableTransferObject.
        /// </summary>
        /// <remarks>Retrieving the value of this property is an O(1) operation.</remarks>
        public int Count {
            get { return current_node_.index; }
        }

        /// <summary>
        /// Gets the number of columns contained in the TableTransferObject.
        /// </summary>
        /// <remarks>Retrieving the value of this property is an O(1) operation.</remarks>
        public int Size {
            get { return columns_.Length; }
        }

        /// <summary>
        /// Gets a JSON-compliant string that represents the underlying class and is formatted
        /// like a JSON array element.
        /// </summary>
        /// <returns>A string formatted like an JSON array which elements are JSON arrays.</returns>
        /// <remarks>
        /// Calling the <see cref="DataTransferObjectSet<T>.ToJsElement()"/> method when
        /// "T" represents a TableTransferObject will produce a JSON-compliant string
        /// representing an array of array of arrays that looks like: [ [ ["A1", "A2"], ["B1", "B2"] ] ]. If you
        /// intend to produce only an array of array that looks like: [ ["A1", "A2" ], ["B1", "B2"] ] call this
        /// method directly.
        /// </remarks>
        public string ToJsElement()
        {
            int capacity = 0;
            string[] values = null;

            TableNode node = root_.next;
            StringBuilder json = null;

            // the capacity of the |json| will be compute by using the
            // following formule:
            //
            // json.Capacity = (DATA_LENGTH_AVERAGE * this.Count + this.Size * 10)
            //
            // where DATA_LENGTH_AVERAGE represents the average length of the data
            // contained in the first row.
            if (node != null && node.values != null) {
                values = node.values;
                for (int i = 0, j = values.Length; i < j; i++) {
                    capacity += values[i].Length;
                }
            }

            json = new StringBuilder(
                capacity * // DATA_LENGTH_AVERAGE
                Count + Size *
                10 // column names length average
                );

            // JSON array of array:
            //    [ ["A1", "A2"], ["B1", "B2"] ]
            json.Append("[");

            while(node != null) {
                values = node.values;
                if (values != null) {

                    // JSON array:
                    //    [ "value", "value" ]
                    json.Append("[");
                    for (int i = 0, j = values.Length; i < j; i++) {
                        json.Append(string.Concat("\"", ((values[i] == null) ? "nil" : values[i]), "\","));
                    }

                    // removing the trailing comma
                    json.Remove(json.Length - 1, 1);
                    json.Append("],");
                }
                node = node.next;
            }

            // removing the trailing comma
            return (json.Length == 1) ? "[]" : json.ToString(0, json.Length - 1) + "]";
        }

        public string ToJsObject()
        {
            throw new NotImplementedException();
        }
    }
}
