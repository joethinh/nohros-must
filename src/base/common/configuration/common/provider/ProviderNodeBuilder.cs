using System;
using System.Collections.Generic;
using System.IO;

namespace Nohros.Configuration
{
  public partial class ProviderNode
  {
    /// <summary>
    /// A builder for <see cref="ProviderNode"/> class.
    /// </summary>
    public class Builder {
      readonly ProviderNode node_;

      /// <summary>
      /// Initializes a new instance of the <see cref="Builder"/> class by
      /// using the provider name and type.
      /// </summary>
      /// <param name="name">
      /// A string that uniquely identifies the provider within an application.
      /// </param>
      /// <param name="type">
      /// The provider's fully qualified assembly name.
      /// </param>
      public Builder(string name, string type) {
        if (name == null || type == null) {
          throw new ArgumentNullException(type == null ? "type" : "location");
        }
        node_ = new ProviderNode(name, type);
      }

      /// <summary>
      /// Sets the path of the location where the provider assembly can be
      /// found.
      /// </summary>
      /// <param name="location">
      /// The path of the location where the provider assembly can be
      /// found.
      /// </param>
      /// <returns>
      /// A <see cref="Builder"/> that builds a provider that can be found at
      /// path <paramref name="location"/>.
      /// </returns>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="location"/> is <c>null</c>.
      /// </exception>
      public Builder SetLocation(string location) {
        if (location == null) {
          throw new ArgumentNullException("location");
        }
        if (!Path.IsPathRooted(location)) {
          node_.location_ = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            location);
        } else {
          node_.location_ = location;
        }
        return this;
      }

      /// <summary>
      /// Sets the name of the group on which the provider belongs to.
      /// </summary>
      /// <param name="group">
      /// The name of the group which the provider belongs to.
      /// </param>
      /// <returns>
      /// A <see cref="Builder"/> that builds a provider that belongs to the
      /// group <paramref name="group"/>.
      /// </returns>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="group"/> is <c>null</c>.
      /// </exception>
      public Builder SetGroup(string group) {
        if (group == null) {
          throw new ArgumentNullException("group");
        }
        node_.group_ = group;
        return this;
      }

      /// <summary>
      /// Sets the provider's options.
      /// </summary>
      /// <param name="options">
      /// The provider's options.
      /// </param>
      /// <returns>
      /// A <see cref="Builder"/> that builds a provider that is associated
      /// with the <paramref name="options"/> collection.
      /// </returns>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="options"/> is <c>null</c>.
      /// </exception>
      public Builder SetOptions(IDictionary<string,string> options) {
        if (options == null) {
          throw new ArgumentNullException("options");
        }
        node_.options = options;
        return this;
      }

      /// <summary>
      /// Builds a new <see cref="ProviderNode"/>
      /// </summary>
      /// <returns></returns>
      public ProviderNode Build() {
        ProviderNode node = new ProviderNode(node_.name, node_.type_,
          node_.location_);
        node.options = node_.options;
        node.group_ = node_.group_;
        return node;
      }
    }
  }
}
