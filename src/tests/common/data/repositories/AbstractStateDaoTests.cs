using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;

namespace Nohros.Data
{
  public class AbstractStateDaoTests
  {
    public class StateDao : AbstractStateDao
    {
      readonly IDictionary<string, string> states_;

      #region .ctor
      public StateDao() {
        states_ = new Dictionary<string, string>();
      }
      #endregion

      public override void SetState(string name, string state) {
        states_[name] = state;
      }

      public override bool StateByName(string name, out string state) {
        return states_.TryGetValue(name, out state);
      }
    }

    [Test]
    public void should_associate_name_with_state() {
      var dao = new StateDao();
      dao.SetState("my-state-name", "my-state");
      Assert.That(dao.StateByName("my-state-name"), Is.EqualTo("my-state"));
    }

    [Test]
    public void should_throw_no_result_exception_when_state_is_not_found() {
      var dao = new StateDao();
      Assert.Throws<NoResultException>(() => dao.StateByName("missing-state"));
    }

    [Test]
    public void should_get_the_state_associated_with_the_name() {
      var dao = new StateDao();
      dao.SetState("my-state-name", "my-state");
      Assert.That(dao.StateByName("my-state-name"), Is.EqualTo("my-state"));
    }

    [Test]
    public void should_convert_state_to_int() {
      var dao = new StateDao();
      dao.SetState("my-state-name", "50");
      int state;
      var ok = dao.StateByName("my-state-name", out state);
      Assert.That(ok, Is.True);
      Assert.That(state, Is.EqualTo(50));
    }

    [Test]
    public void should_convert_state_to_datetime()
    {
      var dao = new StateDao();
      dao.SetState("my-state-name", "2013-10-01");
      DateTime state;
      var ok = dao.StateByName("my-state-name", out state);
      Assert.That(ok, Is.True);
      Assert.That(state, Is.EqualTo(new DateTime(2013, 10, 1)));
    }
  }
}
