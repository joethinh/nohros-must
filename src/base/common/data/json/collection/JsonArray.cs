using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data.Json
{
  /// <summary>
  /// An implementation of the <see cref="IJsonToken{T}"/> that represents a
  /// json array token.
  /// </summary>
  public class JsonArray : IJsonToken<IJsonToken[]>, IJsonCollection
  {
    readonly List<IJsonToken> tokens_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonArray"/> class.
    /// </summary>
    public JsonArray() {
      tokens_ = new List<IJsonToken>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonArray"/> class using
    /// the specified collection of <paramref name="IJsonToken"/> objects.
    /// </summary>
    /// <param name="elements">
    /// An array of <see cref="IJsonToken"/> containing the elements of the
    /// json array.
    /// </param>
    public JsonArray(IEnumerable<IJsonToken> elements) {
      if (elements == null) {
        throw new ArgumentNullException("elements");
      }
      tokens_ = new List<IJsonToken>(elements);
    }
    #endregion

    /// <inheritdoc/>
    public int Count {
      get { return tokens_.Count; }
    }

    /// <inheritdoc/>
    public void Add(IJsonToken json_token) {
      tokens_.Add(json_token);
    }

    /// <inheritdoc/>
    public IJsonToken[] Value {
      get { return tokens_.ToArray(); }
    }

    /// <summary>
    /// Gets the json string representation of the <see cref="IJsonToken{T}"/>
    /// class.
    /// </summary>
    /// <returns>
    /// The json string representation of the <see cref="IJsonToken{T}"/>
    /// class.
    /// </returns>
    public string AsJson() {
      JsonStringBuilder builder = new JsonStringBuilder();
      builder.WriteBeginArray();
      for (int i = 0, j = tokens_.Count; i < j; i++) {
        IJsonToken token = tokens_[i];
        builder.WriteUnquotedString(token.AsJson());
      }
      builder.WriteEndArray();
      return builder.ToString();
    }
  }
}
