using System;
using System.Collections.Generic;

namespace Nohros.Data.Json
{
  public partial class JsonObject
  {
    #region JsonMember
    /// <summary>
    /// An implementation of the <see cref="IJsonToken{T}"/> that represents a
    /// json object member token.
    /// </summary>
    public class JsonMember: IJsonToken<KeyValuePair<string, IJsonToken>>
    {
      readonly string name_;
      readonly IJsonToken value_;

      #region .ctor
      /// <summary>
      /// Initializes a new instance of the <see cref="JsonMember"/> class
      /// by using the member name and value.
      /// </summary>
      /// <param name="name">
      /// The member's name.
      /// </param>
      /// <param name="value">
      /// The member's value.
      /// </param>
      public JsonMember(string name, IJsonToken value) {
        name_ = name;
        value_ = value;
      }
      #endregion

      /// <inheritdoc/>
      public string AsJson() {
        return "\"" + name_ + "\":" + value_.AsJson();
      }

      /// <inheritdoc/>
      public KeyValuePair<string, IJsonToken> Value {
        get { return new KeyValuePair<string, IJsonToken>(name_, value_); }
      }
    }
    #endregion
  }
}
