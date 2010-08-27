using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Nohros.Data
{
    internal delegate bool TreeWalkAction<TKey, TValue>(AndersonTreeNode<TKey, TValue> node);

    /// <summary>
    /// An implementation of a Anderson tree.
    /// </summary>
    /// <see cref="http://user.it.uu.se/~arnea/ps/gb.pdf"/>
    public class AndersonTree<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        /// <summary>
        /// The end of the tree
        /// </summary>
        AndersonTreeNode<TKey, TValue> sentinel;

        /// <summary>
        /// The top of the tree
        /// </summary>
        AndersonTreeNode<TKey, TValue> _root;
        IComparer<TKey> _comparer;
        int _count = 0;

        #region .ctor

        /// <summary>
        /// Initializes a new instance of the AndersonTree.
        /// </summary>
        /// <remarks>The default comparer will be used when comparing items</remarks>
        public AndersonTree():this(Comparer<TKey>.Default)
        {
        }

        /// <summary>
        /// Initializes a new empty <see cref="AndersonTree&alt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="comparer">The comparer to use when comparing items</param>
        /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is null</exception>
        public AndersonTree(IComparer<TKey> comparer)
        {
            if (comparer == null)
                Thrower.ThrowArgumentNullException(ExceptionArgument.comparer);

            sentinel = new AndersonTreeNode<TKey, TValue>();
            sentinel.Level = 0;
            sentinel.Right = sentinel;
            sentinel.Left = sentinel;

            _root = sentinel;
            _count = 0;
            _comparer = comparer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndersonTree&alt;TKey, TValue&gt;"/> class by using
        /// the specified <see cref="System.Comparison&alt;TKeyt&gt;"/> delegate.
        /// </summary>
        /// <param name="comparison">The <see cref="Comparison&alt;T&gt;"/> to use when comparing keys</param>
        public AndersonTree(Comparison<TKey> comparison):this( new ComparisonComparer<TKey>(comparison) )
        {
        }
        #endregion

        /// <summary>
        /// Performs a skew operation over a node.
        /// </summary>
        /// <param name="node">the node to skew</param>
        /// <remarks>Skew is a right rotation when an insertion or deletion creates
        /// a left horizontal link. No changes are needed to the levels after a skew
        /// because the operation simply turns a left horizontal link into a right
        /// horizontal link.</remarks>
        internal void Skew(ref AndersonTreeNode<TKey, TValue> node)
        {
            // non-sentinel node with a horizontal link
            if (node.Level != 0 && node.Left.Level == node.Level)
            {
                // remove the horizontal link by rotating right
                AndersonTreeNode<TKey, TValue> save = node.Left;
                node.Left = save.Right;
                save.Right = node;
                node = save;
            }
        }

        /// <summary>
        /// Performs a split operation over a node.
        /// </summary>
        /// <param name="node">The node to split</param>
        /// Split performs a left rotation operation on a node when an inserion
        /// or deletion creates a consecutive horizontal links.
        internal void Split(ref AndersonTreeNode<TKey, TValue> node)
        {
            // non-sentinel node with a consecutive horizontal link
            if (node.Level != 0 && node.Right.Right.Level == node.Level)
            {
                // remove the horizontal link by rotating left and
                // increasing the node level
                AndersonTreeNode<TKey, TValue> save = node.Right;
                node.Right = save.Left;
                save.Left = node;
                node = save;
                ++node.Level;
            }
        }

        /// <summary>
        /// Retieves the AndersonTreeNode object in the AndersonTree associated with
        /// the specified key.
        /// </summary>
        /// <param name="key">The key of the node to search for</param>
        /// <returns>The AndersonTreeNode associated with the specified key, if the
        /// key is found; otherwise an AndersonTreeNode with level zero - sentinel node</returns>
        /// <exception cref="ArgumentNullException">key is null</exception>
        internal AndersonTreeNode<TKey, TValue> FindNode(TKey key)
        {
            if (key == null)
                Thrower.ThrowArgumentNullException(ExceptionArgument.key);

            int cmp;
            for (AndersonTreeNode<TKey, TValue> node = _root; node.Level != 0; node = (cmp < 0) ? node.Left : node.Right)
            {
                cmp = _comparer.Compare(key, node.Key);
                if (cmp == 0)
                    return node;
            }
            return sentinel;
        }

        /// <summary>
        /// Performs  in-order traversal on the <see cref="AndersonTree&lt;TKey,TValue&gt;"/>
        /// </summary>
        /// <param name="action">A method used to perform some action with each
        /// each node in the tree</param>
        /// <remarks>The <paramref name="action"/> must return true in order to allow the
        /// traversal to continue</remarks>
        internal bool InOrderTreeWalk(TreeWalkAction<TKey, TValue> action)
        {
            if (_root.Level != 0)
            {
                Stack<AndersonTreeNode<TKey, TValue>> stack = new Stack<AndersonTreeNode<TKey, TValue>>(2 * ((int)Math.Log((double)(_count +1))));
                AndersonTreeNode<TKey, TValue> node = _root;
                while (node.Level != 0)
                {
                    stack.Push(node);
                    node = node.Left;
                }
                while (stack.Count != 0)
                {
                    node = stack.Pop();
                    if (!action(node))
                    {
                        return false;
                    }
                    for (AndersonTreeNode<TKey, TValue> node2 = node.Right; node2.Level != 0; node2 = node2.Left)
                    {
                        stack.Push(node2);
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Inserts an item to the AndersonTree
        /// </summary>
        /// <param name="key">The key of the value to insert into the tree</param>
        /// <param name="value">The value to insert into the tree</param>
        /// <param name="add">a value indicating when the item will be added or modified</param>
        private void Insert(TKey key, TValue value, bool add)
        {
            if (key == null)
                Thrower.ThrowArgumentNullException(ExceptionArgument.key);

            // empty tree
            if (_root.Level == 0)
            {
                _root = new AndersonTreeNode<TKey, TValue>(key, value, sentinel);
                _count++;
                return;
            }

            int cmp, dir;
            List<AndersonTreeNode<TKey, TValue>> path = new List<AndersonTreeNode<TKey, TValue>>(_count);
            AndersonTreeNode<TKey, TValue> node = _root;

            // Find the place to insert the item
            //  - If the item is found and we trying to add a new one,
            //    throw an exception - no duplicate items allowed.
            //  - If a leaf is reached, insert the item in the correct place.
            //  - Else, traverse the tree further.
            while (true)
            {
                path.Add(node);

                cmp = _comparer.Compare(key, node.Key);
                if (cmp == 0)
                {
                    if (add)
                        Thrower.ThrowArgumentException(ExceptionResource.Argument_AddingDuplicate);

                    if (node.Level != 0)
                        node.Value = value;

                    return;
                }

                dir = (cmp < 0) ? 0 : 1;

                if (node._childs[dir].Level == 0)
                    break;

                node = node._childs[dir];
            }

            // create the new node
            node._childs[dir] = new AndersonTreeNode<TKey, TValue>(key, value, sentinel);

            // Walk back and rebalance
            int top = path.Count;
            while (--top >= 0)
            {
                AndersonTreeNode<TKey, TValue> n = path[top];

                //which child ?
                if (top != 0)
                    dir = (path[top - 1].Right == n) ? 1 : 0;

                Skew(ref n);
                Split(ref n);

                // Fix the parent
                if (top != 0)
                    path[top - 1].Right = n;
                else
                    _root = n;
            }
            _count++;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="IEnumerator&lt;,&gt;"/>that can be used to iterate through the collection</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            if (_root.Level != 0)
            {
                Stack<AndersonTreeNode<TKey, TValue>> stack = new Stack<AndersonTreeNode<TKey, TValue>>();

                stack.Push(_root);

                while (stack.Count > 0)
                {
                    AndersonTreeNode<TKey, TValue> node = stack.Pop();

                    KeyValuePair<TKey, TValue> keyValuePair = new KeyValuePair<TKey, TValue>(node.Key, node.Value);

                    yield return keyValuePair;

                    if (node.Left.Level != 0)
                    {
                        stack.Push(node.Left);
                    }

                    if (node.Right.Level != 0)
                    {
                        stack.Push(node.Right);
                    }
                }
            }
        }

        #region ICollection<KeyValuePair<TKey, TValue>> members

        /// <summary>
        /// Adds an item to the <see cref="ICollection&lt;,&gt;"/>
        /// </summary>
        /// <param name="item">The <see cref="KeyValuePair&lt;,&gt;"/> to add to
        /// the <see cref="AndersonTree&lt;TKey,TValue&gt;"/></param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Removes all items from the <see cref="AndersonTree&lt;TKey,TValue&gt;"/>
        /// </summary>
        public void Clear()
        {
            _root = sentinel;
            _count = 0;
        }

        /// <summary>
        /// Determines whether a value is in the <see cref="AndersonTree&lt;TKey,TValue&gt;"/>
        /// </summary>
        /// <param name="keyValuePair">The value to locate in the <see cref="AndersonTree&lt;TKey,TValue&gt;"/></param>
        /// <returns>true if the item is found in the</returns>
        public bool Contains(KeyValuePair<TKey, TValue> keyValuePair)
        {
            AndersonTreeNode<TKey, TValue> node = FindNode(keyValuePair.Key);
            return node.Level != 0 && keyValuePair.Value.Equals(node.Value);
        }

        /// <summary>
        /// Copies the elements of the <see cref="AndersonTree&lt;TKey,TValue&gt;"/> to an
        /// <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is
        /// the destination of the elements copied form the <see cref="AndersonTree&lt;TKey,TValue&gt;"/>
        /// The array must have zero-based indexing</param>
        /// <param name="index">The zero-based index in array at which copying begins</param>
        /// <exception cref="ArgumentOutOfRangeException">index is less than 0</exception>
        /// <exception cref="ArgumentNullException">array is null</exception>
        /// <exception cref="ArgumentException">array is multidimensional.-or-<paramref name="index"/>
        /// is equal to or greater than the length of the array.-or-The number of elements in the
        /// source <see cref="AndersonTree&lt;TKey,TValue&gt;"/>is greater than the available space from
        /// <paramref name="index"/>to the end of the destination array.-or-Type <see cref=" KeyValuePair&lt;TKey, TValue&gt;"/>
        /// cannot be cast automatically to the type of the destination array</exception>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            if (array == null)
                Thrower.ThrowArgumentNullException(ExceptionArgument.array);

            if ((index < 0) || (index > array.Length))
                Thrower.ThrowArgumentOutOfRangeException(ExceptionArgument.index);

            if ((array.Length - index) < _count)
                Thrower.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);

            InOrderTreeWalk(delegate(AndersonTreeNode<TKey, TValue> node)
            {
                array[index++] = node.ToKeyValuePair();
                return true;
            });
        }

        /// <summary>
        /// Removes the first occurrence of a specified object from the <see cref="AndersonTree&lt;TKey,TValue&gt;"/>
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="AndersonTree&lt;,&gt"/></param>
        /// <returns>true if the item was suscesfully removed from the <see cref="AndersonTree&lt;TKey,TValue&gt;"/>; otherwise, false.
        /// This method also returns false if item is not found in the original <see cref="AndersonTree&lt;TKey,TValue&gt;"/></returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        /// <summary>
        /// Gets the number of nodes contained in the <see cref="AndersonTree&lt;TKey,TValue&gt;"/>
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="AndersonTree&lt;TKey,TValue&gt;"/>
        /// object is read-only
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region IEnumerable members
        /// <summary>
        /// Returns an enumerator that iterates through a collection
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate
        /// through the collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region IEnumerable<KeyValuePair<TKey, TValue>> members

        /// <summary>
        /// Returns an enumerator that iterates through the tree.
        /// </summary>
        /// <returns>A <see cref="IEnumerator&alt;,&gt;"/> that can be used to iterate through the tree</returns>
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region IDictionary<Tkey, TValue>

        /// <summary>
        /// Adds the specified key and value to the tree.
        /// </summary>
        /// <param name="key">The value of the element to add. The value can be
        /// null for reference types</param>
        /// <param name="value">The key of the element to add</param>
        /// <exception cref="ArgumentException">An element with the same key already
        /// exists in the AndersonTree<,></exception>
        /// <exception cref="ArgumentNullException">key is null</exception>
        public void Add(TKey key, TValue value)
        {
            Insert(key, value, true);
        }

        /// <summary>
        /// Determines whether the <see cref="AndersonTree&lt;TKey,TValue&gt;"/>constains and element
        /// with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the  <see cref="AndersonTree&lt;TKey,TValue&gt;"/></param>
        /// <returns>true if the <see cref="AndersonTree&lt;TKey,TValue&gt;"/>constains ans element
        /// with the key; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">key is null</exception>
        public bool ContainsKey(TKey key)
        {
            AndersonTreeNode<TKey, TValue> node = FindNode(key);
            if (node.Level == 0)
                return false;

            return true;
        }

        /// <summary>
        /// Removes the element with the specified <paramref name="key"/> from the
        /// <see cref="AndersonTree&lt;TKey, TValue&gt;"/>
        /// </summary>
        /// <param name="key">The key of the element to remove</param>
        /// <returns>true if the element is successfully removed; otherwise, false.
        /// This method also returns false if key was not found in the tree</returns>
        /// <exception cref="ArgumentNullException">key is null</exception>
        public bool Remove(TKey key)
        {
            if (key == null)
                Thrower.ThrowArgumentNullException(ExceptionArgument.key);

            int top = 0, dir = 0, cmp;
            AndersonTreeNode<TKey, TValue> node = _root;
            List<AndersonTreeNode<TKey, TValue>> path = new List<AndersonTreeNode<TKey, TValue>>(_count);
            
            // Find node to remove and save path
            while (true)
            {
                path.Add(node);

                // the key was not found
                if (node.Level == 0)
                    return false;

                cmp = _comparer.Compare(key, node.Key);
                if (cmp == 0)
                    break;

                dir = (cmp < 0) ? 0 : 1;

                node = node._childs[dir];
            }

            AndersonTreeNode<TKey, TValue>[] childs = node._childs;

            top = path.Count;

            // If the node was found, remove it.
            if (childs[0] == sentinel || childs[1] == sentinel)
            {
                // Single child case
                int dir2 = (childs[0] == sentinel) ? 1 : 0;

                // Unlink the item
                if (--top != 0)
                {
                    path[top - 1]._childs[dir] = childs[dir2];
                }
                else
                {
                    // The (--top) expression will be zero when the node
                    // that will be removed is the root node. Since, a horizontal
                    // left link is not allowed, the non-sentinel node will never be
                    // the left node - in single child case only.
                    //
                    // If a node with a key less than the root key is inserted into
                    // the tree a left horizontal link will be created and the split
                    // operation will rotate the tree left on the rebalance.
                    _root = node.Right;
                }
            }
            else
            {
                // Two child case
                AndersonTreeNode<TKey, TValue> heir = node.Right;
                AndersonTreeNode<TKey, TValue> prev = node;

                while (heir.Left != sentinel)
                {
                    prev = heir;
                    path.Add(prev);
                    heir = heir.Left;
                }

                // clone the node
                node.Value = heir.Value;
                node.Key = heir.Key;
                prev._childs[ (prev == node) ? 1 : 0 ] = heir.Right;
            }

            // Walk back and rebalance
            while (--top >= 0)
            {
                node = path[top];

                // Which child?
                if (top != 0)
                {
                    dir = path[top - 1].Right == node ? 1 : 0;
                }

                if (node.Left.Level < node.Level - 1 || node.Right.Level < node.Level - 1)
                {
                    if (node.Right.Level > --node.Level)
                    {
                        node.Right.Level = node.Level;
                    }

                    // Order is important!
                    Skew(ref node);
                    Skew(ref node._childs[1]);
                    Skew(ref node._childs[1]._childs[1]);
                    Split(ref node);
                    Split(ref node._childs[1]);
                }

                // Fix the parent
                if (top != 0)
                {
                    path[top - 1]._childs[dir] = node;
                }
                else
                {
                    _root = node;
                }
            }

            _count--;

            return true;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get</param>
        /// <param name="value">When this method returns, contain the value associated
        /// with the specified key, if the is found; otherwise, the default value for
        /// the type of the <paramref name="value"/> parameter. This parameter is passed
        /// uninitialized</param>
        /// <returns>true if the AndersonTree contains an element with the specified
        /// key; otherwise, false</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            AndersonTreeNode<TKey, TValue> node = FindNode(key);
            if (node.Level == 0)
            {
                value = default(TValue);
                return false;
            }
            value = node.Value;
            return true;
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key
        /// </summary>
        /// <param name="key">The key whose value to get or set</param>
        /// <returns>The value associated with the specified key. If the specified
        /// key is not found, attempting to get it throws a <see cref="KeyNotFoundException"/>
        /// ,and attempting to set it creates a new element with the specified key</returns>
        /// <exception cref="ArgumentNullException">key is null</exception>
        /// <exception cref="KeyNotFoundException">The key does not exists in the tree</exception>
        public TValue this[TKey key]
        {
            get
            {
                AndersonTreeNode<TKey, TValue> node = FindNode(key);
                if (node.Level == 0)
                    throw new KeyNotFoundException("key");

                return node.Value;
            }
            set
            {
                Insert(key, value, false);
            }
        }
        #endregion
    }
}