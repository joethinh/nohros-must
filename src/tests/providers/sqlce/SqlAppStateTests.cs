using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using NUnit.Framework;
using Nohros.Data.Providers;

namespace Nohros.Data.SqlServer
{
  public class SqlAppStateTests
  {
    SqlAppState states_;

    [SetUp]
    public void SetUp() {
      var factory = new SqlCeConnectionProviderFactory();
      var options = new Dictionary<string, string> {
        {"server", "states.sdf"}
      };
      var provider = factory.CreateProvider(options) as SqlCeConnectionProvider;
      states_ = new SqlAppState(provider);
      states_.Initialize();
    }

    [Test]
    public void should_get_the_state_associated_with_given_key() {
      states_.Set("my_state", 10);
      states_.Set("my_state", (long) 10);
      states_.Set("my_state", (decimal) 10);
      states_.Set("my_state", (short) 10);
      states_.Set("my_state", (double) 10);
      states_.Set("my_state", Guid.Empty);
      states_.Set("my_state", "10");
      states_.Set("my_state", DateTime.Today);

      var i = states_.Get<int>("my_state");
      var l = states_.Get<long>("my_state");
      var d = states_.Get<decimal>("my_state");
      var s = states_.Get<short>("my_state");
      var f = states_.Get<double>("my_state");
      var g = states_.Get<Guid>("my_state");
      var str = states_.Get<string>("my_state");
      var date = states_.Get<DateTime>("my_state");

      Assert.That(i, Is.EqualTo(10));
      Assert.That(l, Is.EqualTo(10));
      Assert.That(d, Is.EqualTo(10));
      Assert.That(s, Is.EqualTo(10));
      Assert.That(f, Is.EqualTo(10));
      Assert.That(g, Is.EqualTo(Guid.Empty));
      Assert.That(str, Is.EqualTo("10"));
      Assert.That(date, Is.EqualTo(DateTime.Today));
    }
  }
}
