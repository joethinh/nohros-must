using System;
using NUnit.Framework;
using Nohros.Security.Auth;

namespace Nohros.Security
{
  public class PrincipalTests
  {
    [Test]
    public void two_principals_with_the_same_id_should_be_equals() {
      var p1 = new Principal<int>(56);
      var p2 = new Principal<int>(56);
      Assert.That(p1, Is.EqualTo(p2));
    }

    [Test]
    public void principal_should_not_be_equals_to_null() {
      var p1 = new Principal<string>("principal");
      Assert.That(p1, Is.Not.EqualTo(null));
    }

    [Test]
    public void two_principals_with_the_same_id_should_have_the_same_hash_code() {
      var guid = Guid.NewGuid();
      var p1 = new Principal<Guid>(guid);
      var p2 = new Principal<Guid>(guid);
      Assert.That(p1.GetHashCode(), Is.EqualTo(p2.GetHashCode()));
    }

    [Test]
    public void two_principals_with_distinct_ids_should_be_not_equals() {
      var p1 = new Principal<int>(56);
      var p2 = new Principal<int>(60);
      Assert.That(p1, Is.Not.EqualTo(p2));
    }

    [Test]
    public void
      two_principals_with_distinct_ids_should_not_have_the_same_hash_code() {
      var p1 = new Principal<Guid>(Guid.NewGuid());
      var p2 = new Principal<Guid>(Guid.NewGuid());
      Assert.That(p1.GetHashCode(), Is.Not.EqualTo(p2.GetHashCode()));
    }
  }
}
