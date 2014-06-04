using System;
using System.Collections.Generic;

namespace Nohros.Collections
{
  /// <summary>
  /// A container class, used for the AndersonTree
  /// </summary>
  internal class AndersonTreeNode<TKey, TValue>
  {
    /// <summary>
    /// 0:Left; 1:Right
    /// </summary>
    internal AndersonTreeNode<TKey, TValue>[] childs;

    TKey key_;
    int level_;
    TValue value_;

    #region .ctor
    /// <summary>
    /// Constructor used to initialize the sentinel nodes
    /// </summary>
    internal AndersonTreeNode() {
      level_ = 0;
      childs = new AndersonTreeNode<TKey, TValue>[2];
    }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AndersonTreeNode{TKey,TValue}"/> by using the specified key,
    /// value and sentinel node.
    /// </summary>
    /// <param name="key">The key of the node</param>
    /// <param name="value">The value of the node</param>
    /// <param name="sentinel">The sentinel node</param>
    public AndersonTreeNode(TKey key, TValue value,
      AndersonTreeNode<TKey, TValue> sentinel) {
      childs = new AndersonTreeNode<TKey, TValue>[2];
      childs[0] = sentinel;
      childs[1] = sentinel;
      key_ = key;
      value_ = value;
      level_ = 1;
    }
    #endregion

    public KeyValuePair<TKey, TValue> ToKeyValuePair() {
      return new KeyValuePair<TKey, TValue>(key_, value_);
    }

    /// <summary>
    /// Gets or sets the level of the current node.
    /// </summary>
    public int Level {
      get { return level_; }
      set { level_ = value; }
    }

    /// <summary>
    /// Gets or set the right node of the subtree.
    /// </summary>
    public AndersonTreeNode<TKey, TValue> Right {
      get { return childs[1]; }
      set { childs[1] = value; }
    }

    /// <summary>
    /// Gets or sets the left subtree
    /// </summary>
    public AndersonTreeNode<TKey, TValue> Left {
      get { return childs[0]; }
      set { childs[0] = value; }
    }

    /// <summary>
    /// Gets or sets the value of the current node.
    /// </summary>
    public TValue Value {
      get { return value_; }
      set { value_ = value; }
    }

    /// <summary>
    /// Gets or sets the key of the current node.
    /// </summary>
    public TKey Key {
      get { return key_; }
      internal set { key_ = value; }
    }
  }
}
