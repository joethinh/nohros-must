using System;
using System.Collections.Generic;
using Nohros.Resources;

namespace Nohros.Data.Json
{
  /// <summary>
  /// An implementation of the <see cref="IJsonToken{T}"/> that represents a
  /// json object token.
  /// </summary>
  public partial class JsonObject : IJsonToken<JsonObject.JsonMember[]>,
                                    IJsonCollection
  {
    readonly List<JsonMember> members_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonObject"/> class that
    /// contains no members.
    /// </summary>
    public JsonObject() {
      members_ = new List<JsonMember>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonObject"/> class that
    /// is empty and has the specified initial capacity or the default initial
    /// capacity, whichever is greater.
    /// </summary>
    /// <param name="capacity">
    /// The initial number of elements taht the <see cref="JsonMember"/> can
    /// initially store.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="capacity"/> is less than 0.
    /// </exception>
    /// <remarks>
    /// The capacity of a <see cref="JsonObject"/> is the number of elements
    /// taht the <see cref="JsonObject"/> can hold. As elements are added to
    /// a <see cref="JsonObject"/>, the capacity is automatically increased
    /// as required by reallocating the internal array.
    /// <para>
    /// If the size of the collection can be estimated, specifying the initial
    /// capacity eliminates the need to perform a number of resizing operations
    /// while adding elements to the <see cref="JsonObject"/>.
    /// </para>
    /// <para>
    /// This constructor is O(n) operation, where n is
    /// <paramref name="capacity"/>.
    /// </para>
    /// </remarks>
    public JsonObject(int capacity) {
      members_ = new List<JsonMember>(capacity);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonObject"/> class that
    /// contains elements copied from the specified collection and has
    /// sufficient capacity to accomodate the number of elements copied.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="members"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <see cref="members"/> contains one or more <c>null</c> elements.
    /// </exception>
    /// <remarks>
    /// The elements are copied onto the <see cref="JsonObject"/> in the same
    /// order they are ready by the <see cref="IEnumerator{T}"/> of the
    /// collection.
    /// <para>
    /// THis construtor is a O(n) operation, where n is the number of elements
    /// in <paramref name="members"/>
    /// </para>
    /// </remarks>
    public JsonObject(IEnumerable<JsonMember> members) : this() {
      if (members == null) {
        throw new ArgumentNullException("members");
      }

      foreach (JsonMember member in members) {
        if (member == null) {
          throw new ArgumentException(string.Format(
            StringResources.Argument_CollectionNoNulls, "members"));
        }
        members_.Add(member);
      }
    }
    #endregion

    /// <inheritdoc/>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="token"/> is not instance of the
    /// <see cref="JsonMember"/> class.
    /// </exception>
    void IJsonCollection.Add(IJsonToken token) {
      JsonMember member = token as JsonMember;
      if (token == null) {
        throw new ArgumentOutOfRangeException(string.Format(
          StringResources.Arg_WrongType, "token", "JsonObject.JsonMember"));
      }
      Add(member);
    }

    /// <summary>
    /// Gets the number of members that this object contains.
    /// </summary>
    public int Count {
      get { return members_.Count; }
    }

    /// <inheritdoc/>
    public string AsJson() {
      JsonStringBuilder builder = new JsonStringBuilder();
      builder.WriteBeginObject();
        for (int i = 0, j = members_.Count; i < j; i++) {
          IJsonToken token = members_[i];
          builder.WriteUnquotedString(token.AsJson());
        }
      builder.WriteEndObject();
      return builder.ToString();
    }

    /// <summary>
    /// Gets an array of <see cref="JsonMember"/> containing all the memebrs
    /// that a <see cref="JsonObject"/> contain.
    /// </summary>
    public JsonMember[] Value {
      get { return members_.ToArray(); }
    }

    /// <summary>
    /// Adds an <see cref="JsonMember"/> object to the
    /// <seealso cref="IJsonCollection"/>.
    /// </summary>
    /// <param name="member">
    /// The <seealso cref="JsonMember"/> object to be added to the
    /// <see cref="IJsonCollection"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="member"/> is <c>null</c>.
    /// </exception>
    public void Add(JsonMember member) {
      if (member == null) {
        throw new ArgumentNullException("member");
      }
      members_.Add(member);
    }
  }
}
