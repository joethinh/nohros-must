using System;
using System.Collections.Generic;

namespace Nohros.Data.Json
{
  public partial class JsonObject
  {
    /// <summary>
    /// An implementation of the <see cref="IJsonToken{T}"/> that represents a
    /// json object member token.
    /// </summary>
    public class JsonMember : IJsonToken<KeyValuePair<string, IJsonToken>>
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

      /// <summary>
      /// Initializes a new instance of the <see cref="JsonMember"/> class by
      /// using the member name and a string as member value.
      /// </summary>
      /// <param name="name">
      /// The name of member
      /// </param>
      /// <param name="value">
      /// A string that represents the member's value.
      /// </param>
      public JsonMember(string name, string value) {
        name_ = name;
        value_ = new JsonString(value);
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="JsonMember"/> class by
      /// using the member name and a <see cref="int"/> as member value.
      /// </summary>
      /// <param name="name">
      /// The name of member
      /// </param>
      /// <param name="value">
      /// A <see cref="int"/> that represents the member's value.
      /// </param>
      public JsonMember(string name, int value) {
        name_ = name;
        value_ = new JsonInteger(value);
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="JsonMember"/> class by
      /// using the member name and a <see cref="double"/> as member value.
      /// </summary>
      /// <param name="name">
      /// The name of member
      /// </param>
      /// <param name="value">
      /// A <see cref="double"/> that represents the member's value.
      /// </param>
      public JsonMember(string name, double value) {
        name_ = name;
        value_ = new JsonDouble(value);
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="JsonMember"/> class by
      /// using the member name and a <see cref="short"/> as member value.
      /// </summary>
      /// <param name="name">
      /// The name of member
      /// </param>
      /// <param name="value">
      /// A <see cref="short"/> that represents the member's value.
      /// </param>
      public JsonMember(string name, short value) {
        name_ = name;
        value_ = new JsonShort(value);
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="JsonMember"/> class by
      /// using the member name and a <see cref="bool"/> as member value.
      /// </summary>
      /// <param name="name">
      /// The name of member
      /// </param>
      /// <param name="value">
      /// A <see cref="bool"/> that represents the member's value.
      /// </param>
      public JsonMember(string name, bool value) {
        name_ = name;
        value_ = new JsonBoolean(value);
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="JsonMember"/> class by
      /// using the member name and a <see cref="float"/> as member value.
      /// </summary>
      /// <param name="name">
      /// The name of member
      /// </param>
      /// <param name="value">
      /// A <see cref="float"/> that represents the member's value.
      /// </param>
      public JsonMember(string name, float value) {
        name_ = name;
        value_ = new JsonFloat(value);
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="JsonMember"/> class by
      /// using the member name and a <see cref="long"/>integer as member value.
      /// </summary>
      /// <param name="name">
      /// The name of member
      /// </param>
      /// <param name="value">
      /// A <see cref="float"/> that represents the member's value.
      /// </param>
      public JsonMember(string name, long value) {
        name_ = name;
        value_ = new JsonLong(value);
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="JsonMember"/> class by
      /// using the member name and a <see cref="decimal"/> as member value.
      /// </summary>
      /// <param name="name">
      /// The name of member
      /// </param>
      /// <param name="value">
      /// A <see cref="decimal"/> that represents the member's value.
      /// </param>
      public JsonMember(string name, decimal value) {
        name_ = name;
        value_ = new JsonDecimal(value);
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
  }
}
