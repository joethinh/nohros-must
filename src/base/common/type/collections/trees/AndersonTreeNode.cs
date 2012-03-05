using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Collections
{
    /// <summary>
    /// A container class, used for the AndersonTree
    /// </summary>
    internal class AndersonTreeNode<TKey, TValue>
    {
        int _level;
        TKey _key;
        TValue _value;

        /// <summary>
        /// 0:Left; 1:Right
        /// </summary>
        internal AndersonTreeNode<TKey, TValue>[] _childs;

        #region .ctor
        /// <summary>
        /// Constructor used to initialize the sentinel nodes
        /// </summary>
        internal AndersonTreeNode()
        {
            _level = 0;
            _childs = new AndersonTreeNode<TKey, TValue>[2];
        }

        /// <summary>
        /// Initializes a new instance_ of the AndersonTreeNode by using the
        /// specified key, value and sentinel node.
        /// </summary>
        /// <param name="key">The key of the node</param>
        /// <param name="value">The value of the node</param>
        /// <param name="sentinel">The sentinel node</param>
        public AndersonTreeNode(TKey key, TValue value, AndersonTreeNode<TKey, TValue> sentinel)
        {
            _childs = new AndersonTreeNode<TKey, TValue>[2];
            _childs[0] = sentinel;
            _childs[1] = sentinel;
            _key = key;
            _value = value;
            _level = 1;
        }
        #endregion

        public KeyValuePair<TKey, TValue> ToKeyValuePair()
        {
            return new KeyValuePair<TKey, TValue>(_key, _value);
        }

        /// <summary>
        /// Gets or sets the level of the current node.
        /// </summary>
        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }

        /// <summary>
        /// Gets or set the right node of the subtree.
        /// </summary>
        public AndersonTreeNode<TKey, TValue> Right
        {
            get { return _childs[1]; }
            set { _childs[1] = value; }
        }

        /// <summary>
        /// Gets or sets the left subtree
        /// </summary>
        public AndersonTreeNode<TKey, TValue> Left
        {
            get { return _childs[0]; }
            set { _childs[0] = value; }
        }

        /// <summary>
        /// Gets or sets the value of the current node.
        /// </summary>
        public TValue Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Gets or sets the key of the current node.
        /// </summary>
        public TKey Key
        {
            get { return _key; }
            internal set { _key = value; }
        }
    }
}
