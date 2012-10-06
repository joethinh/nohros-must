using System;
using System.Collections;
using System.Collections.Generic;

namespace Nohros.Collections
{
  internal delegate bool TreeWalkAction<TKey, TValue>(
    AndersonTreeNode<TKey, TValue> node);

  /// <summary>
  /// An implementation of a Anderson tree.
  /// </summary>
  /// <remarks>
  /// This class is not thread-safe.
  /// <see cref="http://user.it.uu.se/~arnea/ps/gb.pdf"/>
  /// </remarks>
  public class AndersonTree<TKey, TValue> :
    ICollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IInOrderVisitable<TKey, TValue>,
    IInOrderVisitable<TValue>,
    IEnumerable
  {
    readonly IComparer<TKey> comparer_;
    readonly AndersonTreeNode<TKey, TValue> sentinel_;
    int count_;
    bool key_is_value_type_;

    /// <summary>
    /// The top of the tree
    /// </summary>
    AndersonTreeNode<TKey, TValue> root_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance_ of the AndersonTree.
    /// </summary>
    /// <remarks>The default comparer will be used when comparing
    /// items.</remarks>
    public AndersonTree()
      : this(Comparer<TKey>.Default) {
    }

    /// <summary>
    /// Initializes a new empty <see cref="AndersonTree{TKey,TValue}"/>
    /// class.
    /// </summary>
    /// <param name="comparer">
    /// The comparer to use when comparing items.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="comparer"/> is <c>null</c>.
    /// </exception>
    public AndersonTree(IComparer<TKey> comparer) {
      if (comparer == null) {
        Thrower.ThrowArgumentNullException(ExceptionArgument.comparer);
      }

      sentinel_ = new AndersonTreeNode<TKey, TValue>();
      sentinel_.Level = 0;
      sentinel_.Right = sentinel_;
      sentinel_.Left = sentinel_;

      root_ = sentinel_;
      count_ = 0;
      comparer_ = comparer;
      key_is_value_type_ = typeof (TKey).IsValueType;
    }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AndersonTree{TKey,TValue}"/> class by using the specified
    /// <see cref="Comparison{T}"/> delegate.
    /// </summary>
    /// <param name="comparison">
    /// The <see cref="Comparison{T}"/> to use when comparing keys.
    /// </param>
    public AndersonTree(Comparison<TKey> comparison)
      : this(new ComparisonComparer<TKey>(comparison)) {
    }
    #endregion

    /// <summary>
    /// Adds an item to the <see cref="ICollection{T}"/>
    /// </summary>
    /// <param name="item">
    /// The <see cref="KeyValuePair{TKey,TValue}"/> to add to the
    /// <see cref="AndersonTree{TKey,TValue}"/>
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="item.Key"/> is <c>null</c>.
    /// </exception>
    public void Add(KeyValuePair<TKey, TValue> item) {
      Add(item.Key, item.Value);
    }

    /// <summary>
    /// Removes all items from the
    /// <see cref="AndersonTree{TKey,TValue}"/>
    /// </summary>
    public void Clear() {
      root_ = sentinel_;
      count_ = 0;
    }

    /// <summary>
    /// Determines whether a value is in the
    /// <see cref="AndersonTree{TKey,TValue}"/>
    /// </summary>
    /// <param name="key_value_pair">
    /// The value to locate in the <see cref="AndersonTree{TKey,TValue}"/>
    /// </param>
    /// <returns>
    /// <c>true</c> if the item is found in the
    /// <see cref="AndersonTree{TKey,TValue}"/>
    /// </returns>
    public bool Contains(KeyValuePair<TKey, TValue> key_value_pair) {
      AndersonTreeNode<TKey, TValue> node = FindNode(key_value_pair.Key);
      if (node.Level != 0) {
        EqualityComparer<TValue> comparer = EqualityComparer<TValue>.Default;
        return comparer.Equals(key_value_pair.Value, node.Value);
      }
      return false;
    }

    /// <summary>
    /// Copies the elements of the <see cref="AndersonTree{TKey,TValue}"/> to
    /// an <see cref="Array"/>, starting at a particular index.
    /// </summary>
    /// <param name="array">
    /// The one-dimensional <see cref="Array"/> that is the destination of the
    /// elements copied from the <see cref="AndersonTree{TKey,TValue}"/>.
    /// The array must have zero-based indexing.
    /// </param>
    /// <param name="index">
    /// The zero-based index in array at which copying begins.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="index"/> is less than 0.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="array"/>is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="array"/> is multidimensional.-or-
    /// <paramref name="index"/> is equal to or greater than the length of the
    /// array.-or-The number of elements in the source
    /// <see cref="AndersonTree{TKey,TValue}"/>is greater than the
    /// available space from <paramref name="index"/> to the end of the
    /// destination array.-or- <see cref=" KeyValuePair{TKey,TValue}"/> cannot
    /// be cast automatically to the type of the destination array.
    /// </exception>
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index) {
      if ((index < 0) || (index > array.Length))
        Thrower.ThrowArgumentOutOfRangeException(ExceptionArgument.index);

      if ((array.Length - index) < count_)
        Thrower.ThrowArgumentException(
          ExceptionResource.Arg_ArrayPlusOffTooSmall);

      if (count_ == 0) {
        return;
      }

      InOrderTreeWalk(delegate(AndersonTreeNode<TKey, TValue> node)
      {
        array[index++] = node.ToKeyValuePair();
        return true;
      });
    }

    /// <summary>
    /// Removes the first occurrence of a specified object from the
    /// <see cref="AndersonTree&lt;TKey,TValue&gt;"/>
    /// </summary>
    /// <param name="item">The object to remove from the
    /// <see cref="AndersonTree&lt;,&gt"/></param>
    /// <returns>true if the item was suscesfully removed from the
    /// <see cref="AndersonTree&lt;TKey,TValue&gt;"/>; otherwise, false. This
    /// method also returns false if item is not found in the original
    /// <see cref="AndersonTree&lt;TKey,TValue&gt;"/></returns>
    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(
      KeyValuePair<TKey, TValue> item) {
      return Remove(item.Key);
    }

    /// <summary>
    /// Gets the number of nodes contained in the
    /// <see cref="AndersonTree&lt;TKey,TValue&gt;"/>
    /// </summary>
    public int Count {
      get { return count_; }
    }

    /// <summary>
    /// Gets a value indicating whether the
    /// <see cref="AndersonTree&lt;TKey,TValue&gt;"/> object is read-only
    /// </summary>
    public bool IsReadOnly {
      get { return false; }
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/> object that can be used to
    /// iterate through the collection</returns>
    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the tree.
    /// </summary>
    /// <returns>A <see cref="IEnumerator&alt;,&gt;"/> that can be used to
    /// iterate through the tree</returns>
    IEnumerator<KeyValuePair<TKey, TValue>>
      IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() {
      return GetEnumerator();
    }

    /// <summary>
    /// Accepts the specified visitor and allow it to visit every node of the
    /// tree.
    /// </summary>
    /// <param name="visitor">
    /// The visitor to accepts.
    /// </param>
    /// <param name="reverse">
    /// A value indicating if the elements will be visit in the reverse order
    /// or not.
    /// </param>
    /// <param name="state">
    /// An object that contains the <paramref name="visitor"/> state.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="visitor"/> is a <c>null</c> reference.
    /// </exception>
    public void Accept(InOrderVisitor<TKey, TValue> visitor, object state,
      bool reverse) {
      if (visitor == null) {
        throw new ArgumentNullException("visitor");
      }

      TreeWalkAction<TKey, TValue> on_tree_walk =
        new TreeWalkAction<TKey, TValue>(
          delegate(AndersonTreeNode<TKey, TValue> node)
          {
            visitor.Visit(node.Key, node.Value, state);
            return !visitor.IsCompleted;
          });

      if (reverse)
        ReverseInOrderTreeWalk(on_tree_walk);
      else
        InOrderTreeWalk(on_tree_walk);
    }

    /// <summary>
    /// Accepts the specified visitor and allow it to visit every node of the
    /// tree.
    /// </summary>
    /// <param name="visitor">The visitor to accepts.</param>
    /// <param name="reverse">A value indicating if the elements will be visit
    /// in the reverse order or not.</param>
    /// <param name="state">A user-defined object that qualifies or contains
    /// information about the visitor's current state.</param>
    /// <exception cref="ArgumentNullException"><paramref name="visitor"/> is
    /// a null reference</exception>
    public void Accept(InOrderVisitor<TValue> visitor, object state,
      bool reverse) {
      if (visitor == null)
        throw new ArgumentNullException("visitor");

      TreeWalkAction<TKey, TValue> on_tree_walk =
        new TreeWalkAction<TKey, TValue>(
          delegate(AndersonTreeNode<TKey, TValue> node)
          {
            visitor.Visit(node.Value, state);
            return !visitor.IsCompleted;
          });

      if (reverse)
        ReverseInOrderTreeWalk(on_tree_walk);
      else
        InOrderTreeWalk(on_tree_walk);
    }

    /// <summary>
    /// Accepts the specified visitor and allow it to visit every node of the
    /// tree.
    /// </summary>
    /// <param name="visitor">
    /// The visitor to accepts.
    /// </param>
    /// <param name="reverse">
    /// A value indicating if the elements will be visit in the reverse order
    /// or not.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="visitor"/> is a <c>null</c> reference.
    /// </exception>
    public void Accept(InOrderVisitor<TKey, TValue> visitor, bool reverse) {
      Accept(visitor, null, reverse);
    }


    /// <summary>
    /// Accepts the specified visitor and allow it to visit every node of the
    /// tree.
    /// </summary>
    /// <param name="visitor">
    /// The visitor to accepts.
    /// </param>
    /// <param name="reverse">
    /// A value indicating if the elements will be visit in the reverse order
    /// or not.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="visitor"/> is a<c>null</c> reference.
    /// </exception>
    public void Accept(InOrderVisitor<TValue> visitor, bool reverse) {
      Accept(visitor, null, reverse);
    }

    /// <summary>
    /// Performs a skew operation over a node.
    /// </summary>
    /// <param name="node">the node to skew</param>
    /// <remarks>Skew is a right rotation when an insertion or deletion creates
    /// a left horizontal link. No changes are needed to the levels after a
    /// skew because the operation simply turns a left horizontal link into a
    /// right horizontal link.</remarks>
    internal void Skew(ref AndersonTreeNode<TKey, TValue> node) {
      // non-sentinel_ node with a horizontal link
      if (node.Level != 0 && node.Left.Level == node.Level) {
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
    internal void Split(ref AndersonTreeNode<TKey, TValue> node) {
      // non-sentinel_ node with a consecutive horizontal link
      if (node.Level != 0 && node.Right.Right.Level == node.Level) {
        // remove the horizontal link by rotating left and increasing the node
        // level
        AndersonTreeNode<TKey, TValue> save = node.Right;
        node.Right = save.Left;
        save.Left = node;
        node = save;
        ++node.Level;
      }
    }

    /// <summary>
    /// Retrieves a <see cref="AndersonTreeNode{TKey,TValue}"/> object in the
    /// <see cref="AndersonTree{TKey,TValue}"/> associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the node to search for</param>
    /// <returns>The AndersonTreeNode associated with the specified key, if the
    /// key is found; otherwise an AndersonTreeNode with level zero - sentinel_
    /// node.</returns>
    /// <exception cref="ArgumentNullException">key is null</exception>
    internal AndersonTreeNode<TKey, TValue> FindNode(TKey key) {
      int cmp;
      for (AndersonTreeNode<TKey, TValue> node = root_;
           node.Level != 0;
           node = (cmp < 0) ? node.Left : node.Right) {
        cmp = comparer_.Compare(key, node.Key);
        if (cmp == 0)
          return node;
      }
      return sentinel_;
    }

    /// <summary>
    /// Performs in-order traversal on the
    /// <see cref="AndersonTree{TKey,TValue}"/>
    /// </summary>
    /// <param name="action">
    /// A method used to perform some action with each node in the tree.
    /// </param>
    /// <remarks>
    /// The <paramref name="action"/> must returns <c>true</c> in order to
    /// allows the traversal to continue.
    /// </remarks>
    internal bool InOrderTreeWalk(TreeWalkAction<TKey, TValue> action) {
      if (root_.Level != 0) {
        Stack<AndersonTreeNode<TKey, TValue>> stack =
          new Stack<AndersonTreeNode<TKey, TValue>>
            (2*((int) Math.Log((double) (count_ + 1))));

        AndersonTreeNode<TKey, TValue> node = root_;

        // traverse the left branch of the tree until reach a sentinel node.
        while (node.Level != 0) {
          stack.Push(node);
          node = node.Left;
        }

        while (stack.Count != 0) {
          node = stack.Pop();

          if (!action(node))
            return false;

          // traverse the right branch of the current node and store then into
          // the execution stack.
          for (AndersonTreeNode<TKey, TValue> node2 = node.Right;
               node2.Level != 0;
               node2 = node2.Left)
            stack.Push(node2);
        }
      }
      return true;
    }

    /// <summary>
    /// Performs reverse in-order traversal on the
    /// <see cref="AndersonTree{TKey,TValue}"/>
    /// </summary>
    /// <param name="action">
    /// A method used to perform some action with each node in the tree.
    /// </param>
    /// <remarks>
    /// The <paramref name="action"/> must return <c>true</c> in order to
    /// allows the traversal to continue.
    /// </remarks>
    internal bool ReverseInOrderTreeWalk(TreeWalkAction<TKey, TValue> action) {
      if (root_.Level != 0) {
        Stack<AndersonTreeNode<TKey, TValue>> stack =
          new Stack<AndersonTreeNode<TKey, TValue>>
            (2*((int) Math.Log((double) (count_ + 1))));

        AndersonTreeNode<TKey, TValue> node = root_;

        // traverse the left branch of the tree until reach a sentinel_ node.
        while (node.Level != 0) {
          stack.Push(node);
          node = node.Right;
        }

        while (stack.Count != 0) {
          node = stack.Pop();

          if (!action(node)) {
            return false;
          }

          // traverse the right branch of the current node and store then into
          // the execution stack.
          for (AndersonTreeNode<TKey, TValue> node2 = node.Left;
               node2.Level != 0;
               node2 = node2.Right) {
            stack.Push(node2);
          }
        }
      }
      return true;
    }

    /// <summary>
    /// Inserts an item to the AndersonTree
    /// </summary>
    /// <param name="key">
    /// The key of the value to insert into the tree.
    /// </param>
    /// <param name="value">
    /// The value to insert in the tree.
    /// </param>
    /// <param name="add">
    /// A value indicating when the item will be added or modified.
    /// </param>
    void Insert(TKey key, TValue value, bool add) {
      if (!key_is_value_type_ && key == null) {
        Thrower.ThrowArgumentNullException(ExceptionArgument.key);
      }

      // empty tree
      if (root_.Level == 0) {
        root_ = new AndersonTreeNode<TKey, TValue>(key, value, sentinel_);
        count_++;
        return;
      }

      int cmp, dir;
      List<AndersonTreeNode<TKey, TValue>> path =
        new List<AndersonTreeNode<TKey, TValue>>(count_);

      AndersonTreeNode<TKey, TValue> node = root_;

      // Find the place to insert the item
      //  - If the item is found and we trying to add a new one,
      //    throw an exception - no duplicate items allowed.
      //  - If a leaf is reached, insert the item in the correct place.
      //  - Else, traverse the tree further.
      while (true) {
        path.Add(node);

        cmp = comparer_.Compare(key, node.Key);
        if (cmp == 0) {
          if (add)
            Thrower.ThrowArgumentException(
              ExceptionResource.Argument_AddingDuplicate);

          if (node.Level != 0)
            node.Value = value;

          return;
        }

        dir = (cmp < 0) ? 0 : 1;

        if (node.childs[dir].Level == 0)
          break;

        node = node.childs[dir];
      }

      // create the new node
      node.childs[dir] =
        new AndersonTreeNode<TKey, TValue>(key, value, sentinel_);

      // Walk back and rebalance
      int top = path.Count;
      while (--top >= 0) {
        AndersonTreeNode<TKey, TValue> n = path[top];

        // which child ?
        if (top != 0) {
          dir = (path[top - 1].Right == n) ? 1 : 0;
        }

        Skew(ref n);
        Split(ref n);

        // Fix the parent
        if (top != 0) {
          path[top - 1].childs[dir] = n;
        } else {
          root_ = n;
        }
      }
      count_++;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="IEnumerator{T}"/>that can be used to iterate through the
    /// collection.
    /// </returns>
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
      if (root_.Level != 0) {
        Stack<AndersonTreeNode<TKey, TValue>> stack =
          new Stack<AndersonTreeNode<TKey, TValue>>();

        stack.Push(root_);

        while (stack.Count > 0) {
          AndersonTreeNode<TKey, TValue> node = stack.Pop();

          KeyValuePair<TKey, TValue> key_value_pair =
            new KeyValuePair<TKey, TValue>(node.Key, node.Value);

          yield return key_value_pair;

          if (node.Left.Level != 0) {
            stack.Push(node.Left);
          }

          if (node.Right.Level != 0) {
            stack.Push(node.Right);
          }
        }
      }
    }

    /// <summary>
    /// Adds the specified key and value to the tree.
    /// </summary>
    /// <param name="key">
    /// The value of the element to add. The value can be <c>null</c> for
    /// reference types.
    /// </param>
    /// <param name="value">
    /// The key of the element to add.
    /// </param>
    /// <exception cref="ArgumentException">
    /// An element with the same key already exists in the
    /// <see cref="AndersonTree{TKey,TValue}"/>
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="key"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// <paramref name="key"/> cannot be <c>null</c>, but
    /// <paramref name="value"/> can be, if <typeparamref name="TValue"/> is
    /// a reference type.
    /// </remarks>
    public void Add(TKey key, TValue value) {
      Insert(key, value, true);
    }

    /// <summary>
    /// Determines whether the <see cref="AndersonTree&lt;TKey,TValue&gt;"/>
    /// constains and element with the specified key.
    /// </summary>
    /// <param name="key">The key to locate in the
    /// <see cref="AndersonTree&lt;TKey,TValue&gt;"/></param>
    /// <returns>true if the <see cref="AndersonTree&lt;TKey,TValue&gt;"/>
    /// constains an element with the key; otherwise, false</returns>
    /// <exception cref="ArgumentNullException">key is null</exception>
    public bool ContainsKey(TKey key) {
      AndersonTreeNode<TKey, TValue> node = FindNode(key);
      if (node.Level == 0)
        return false;

      return true;
    }

    /// <summary>
    /// Removes the element with the specified <paramref name="key"/> from the
    /// <see cref="AndersonTree&lt;TKey, TValue&gt;"/>
    /// </summary>
    /// <param name="key">
    /// The key of the element to remove.
    /// </param>
    /// <returns>
    /// <c>true</c> if the element is successfully removed; otherwise,
    /// <c>false</c>. This method also returns <c>false</c> if key was not
    /// found in the tree.
    /// </returns>
    public bool Remove(TKey key) {
      int top = 0, direction = 0;
      AndersonTreeNode<TKey, TValue> node = root_;
      List<AndersonTreeNode<TKey, TValue>> path =
        new List<AndersonTreeNode<TKey, TValue>>(count_);

      // Find node to remove and save path
      while (true) {
        path.Add(node);

        // the key was not found
        if (node.Level == 0) {
          return false;
        }

        int cmp = comparer_.Compare(key, node.Key);
        if (cmp == 0) {
          break;
        }

        direction = (cmp < 0) ? 0 : 1;

        node = node.childs[direction];
      }

      AndersonTreeNode<TKey, TValue>[] childs = node.childs;

      top = path.Count;

      // If the node was found, remove it.
      if (childs[0] == sentinel_ || childs[1] == sentinel_) {
        // Single child case
        int dir2 = (childs[0] == sentinel_) ? 1 : 0;

        // Unlink the item
        if (--top != 0) {
          path[top - 1].childs[direction] = childs[dir2];
        } else {
          // The (--top) expression will be zero when the node
          // that will be removed is the root node. Since, a horizontal
          // left link is not allowed, the non-sentinel node will never be
          // the left node - in single child case only.
          //
          // If a node with a key less than the root key is inserted into
          // the tree a left horizontal link will be created and the split
          // operation will rotate the tree left on the rebalance.
          root_ = node.Right;
        }
      } else {
        // Two child case
        AndersonTreeNode<TKey, TValue> heir = node.Right;
        AndersonTreeNode<TKey, TValue> prev = node;

        while (heir.Left != sentinel_) {
          prev = heir;
          path.Add(prev);
          heir = heir.Left;
        }

        // clone the node
        node.Value = heir.Value;
        node.Key = heir.Key;
        prev.childs[(prev == node) ? 1 : 0] = heir.Right;
      }

      // Walk back and rebalance
      while (--top >= 0) {
        node = path[top];

        // Which child?
        if (top != 0) {
          direction = path[top - 1].Right == node ? 1 : 0;
        }

        if (node.Left.Level < node.Level - 1 ||
          node.Right.Level < node.Level - 1) {
          if (node.Right.Level > --node.Level) {
            node.Right.Level = node.Level;
          }

          // Order is important!
          Skew(ref node);
          Skew(ref node.childs[1]);
          Skew(ref node.childs[1].childs[1]);
          Split(ref node);
          Split(ref node.childs[1]);
        }

        // Fix the parent
        if (top != 0) {
          path[top - 1].childs[direction] = node;
        } else {
          root_ = node;
        }
      }

      count_--;

      return true;
    }

    /// <summary>
    /// Gets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get</param>
    /// <param name="value">When this method returns, contain the value
    /// associated with the specified key, if the is found; otherwise, the
    /// default value for the type of the <paramref name="value"/> parameter.
    /// This parameter is passed uninitialized</param>
    /// <returns>true if the AndersonTree contains an element with the
    /// specified key; otherwise, false</returns>
    public bool TryGetValue(TKey key, out TValue value) {
      AndersonTreeNode<TKey, TValue> node = FindNode(key);
      if (node.Level == 0) {
        value = default(TValue);
        return false;
      }
      value = node.Value;
      return true;
    }

    /// <summary>
    /// Gets or sets the value associated with the specified key
    /// </summary>
    /// <param name="key">
    /// The key whose value to get or set.
    /// </param>
    /// <returns>
    /// The value associated with the specified key. If the specified key is
    /// not found, attempting to get it throws a
    /// <see cref="KeyNotFoundException"/>, and attempting to set it creates a
    /// new element with the specified key.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// key is <c>null</c>.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// The key does not exists in the tree.
    /// </exception>
    public TValue this[TKey key] {
      get {
        AndersonTreeNode<TKey, TValue> node = FindNode(key);
        if (node.Level == 0)
          throw new KeyNotFoundException("key");

        return node.Value;
      }
      set { Insert(key, value, false); }
    }

    /// <summary>
    /// Gets a <see cref="KeyValuePair{TKey,TValue}"/> associated with the
    /// least key in the tree, or a <see cref="KeyValuePair{TKey,TValue}"/>
    /// where <see cref="KeyValuePair{TKey,TValue}.Key"/> and
    /// <see cref="KeyValuePair{TKey,TValue}.Value"/> contains the default
    /// values of it's types if the tree is empty.
    /// </summary>
    /// <remarks>
    /// Retrieving the value of this property is a O(log(n)) operation.
    /// </remarks>
    public KeyValuePair<TKey, TValue> First {
      get {
        if (count_ == 0) {
          return new KeyValuePair<TKey, TValue>(default(TKey), default(TValue));
        }

        // traverse the left branch of the tree until reach a sentinel node.
        AndersonTreeNode<TKey, TValue> node = root_;
        while (node.Left.Level != 0) {
          node = node.Left;
        }

        return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
      }
    }

    /// <summary>
    /// Gets a <see cref="KeyValuePair{TKey,TValue}"/> associated with the
    /// greatest key in the tree, or a <see cref="KeyValuePair{TKey,TValue}"/>
    /// where <see cref="KeyValuePair{TKey,TValue}.Key"/> and
    /// <see cref="KeyValuePair{TKey,TValue}.Value"/> contains the default
    /// values of it's types if the tree is empty.
    /// </summary>
    /// <remarks>
    /// Retrieving the value of this property is a O(log(n)) operation.
    /// </remarks>
    public KeyValuePair<TKey, TValue> Last {
      get {
        if (count_ == 0) {
          return new KeyValuePair<TKey, TValue>(default(TKey), default(TValue));
        }

        // traverse the left branch of the tree until reach a sentinel node.
        AndersonTreeNode<TKey, TValue> node = root_;
        while (node.Right.Level != 0) {
          node = node.Right;
        }

        return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
      }
    }

    /// <summary>
    /// Copies the elements of the <see cref="AndersonTree{TKey,TValue}"/> to
    /// a new array.
    /// </summary>
    /// <returns>
    /// An array containing copies of the elements of the
    /// <see cref="AndersonTree{TKey,TValue}"/>.
    /// </returns>
    /// <remarks>
    /// This method is an O(n) operation, where n is <see cref="Count"/>.
    /// </remarks>
    public KeyValuePair<TKey, TValue>[] ToArray() {
      KeyValuePair<TKey, TValue>[] array =
        new KeyValuePair<TKey, TValue>[count_];
      CopyTo(array, 0);
      return array;
    }
  }
}
