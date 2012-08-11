using System;
using Nohros.Collections;
using Nohros.Logging;

namespace Nohros.Configuration
{
  public partial class MustConfiguration
  {
    public class Builder
    {
      LogLevel log_level_;
      LoginModulesNode login_modules_;
      DictionaryValue properties_;
      ProvidersNode providers_;
      RepositoriesNode repositories_;
      XmlElementsNode xml_elements_;

      #region .ctor
      /// <summary>
      /// Initializes a new instance of the <see cref="Builder"/> class.
      /// </summary>
      public Builder() {
        properties_ = new DictionaryValue();
        repositories_ = new RepositoriesNode();
        providers_ = new ProvidersNode();
        login_modules_ = new LoginModulesNode();
        xml_elements_ = new XmlElementsNode();
      }
      #endregion

      public Builder SetRepositories(RepositoriesNode repositories) {
        repositories_ = repositories;
        return this;
      }

      public Builder SetProviders(ProvidersNode providers) {
        providers_ = providers;
        return this;
      }

      public Builder SetLoginModules(LoginModulesNode login_modules) {
        login_modules_ = login_modules;
        return this;
      }

      public Builder SetXmlElements(XmlElementsNode xml_elements) {
        xml_elements_ = xml_elements;
        return this;
      }

      public Builder SetLogLevel(LogLevel level) {
        log_level_ = level;
        return this;
      }

      public LoginModulesNode LoginModules {
        get { return login_modules_; }
      }

      public DictionaryValue Properties {
        get { return properties_; }
      }

      public ProvidersNode Providers {
        get { return providers_; }
      }

      public RepositoriesNode Repositories {
        get { return repositories_; }
      }

      public XmlElementsNode XmlElements {
        get { return xml_elements_; }
      }
    }
  }
}
