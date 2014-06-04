using System;
using NUnit.Framework;
using Nohros.Configuration;

namespace Nohros
{
  public class RuntimeTypeFactoryTests
  {
    public class BaseType : IBaseType
    {
    }

    public interface IBaseType
    {
    }

    public interface ITestFactory
    {
    }

    public class TestFactory : ITestFactory
    {
      #region .ctor
      public TestFactory() {
        Arg = -1;
        StringArg = string.Empty;
      }

      public TestFactory(int arg) {
        Arg = arg;
        StringArg = string.Empty;
      }

      public TestFactory(int arg, string arg2) {
        Arg = arg;
        StringArg = arg2;
      }

      public TestFactory(int arg, string arg2, int arg3) {
        Arg = arg;
        Arg3 = arg3;
        StringArg = arg2;
      }

      public TestFactory(IBaseType type) {
      }
      #endregion

      public int Arg { get; set; }
      public int Arg3 { get; set; }
      public string StringArg { get; set; }
    }

    ProviderNode node_;

    [SetUp]
    public void SetUp() {
      node_ = new ProviderNode.Builder("provider",
        typeof (TestFactory).AssemblyQualifiedName)
        .Build();
    }

    [Test]
    public void ShouldNotThrowExceptionWhenTypeLoadFail() {
      try {
        var obj = RuntimeTypeFactory<TestFactory>
          .CreateInstanceNoException(node_, string.Empty);
        Assert.That(obj, Is.Null,
          "A exception should be raised but not propagated");
      } catch {
        Assert.Fail("Any raised exception should be not propagated");
      }
    }

    [Test]
    public void ShouldThrowExceptionWhenTypeLoadFail() {
      Assert.That(() => RuntimeTypeFactory<TestFactory>
        .CreateInstance(node_, string.Empty), Throws.Exception);
    }

    [Test]
    public void ShouldCreateInstanceForDefaultConstructor() {
      object obj = RuntimeTypeFactory<ITestFactory>.CreateInstance(node_);
      Assert.That(obj, Is.AssignableTo<TestFactory>());

      obj = RuntimeTypeFactory<ITestFactory>.CreateInstanceFallback(node_);
      Assert.That(obj, Is.AssignableTo<TestFactory>());

      obj = RuntimeTypeFactory<ITestFactory>.CreateInstanceNoException(node_);
      Assert.That(obj, Is.AssignableTo<TestFactory>());
    }

    [Test]
    public void ShouldCreateInstanceForNonDefaultConstructor() {
      object obj = RuntimeTypeFactory<ITestFactory>.CreateInstance(node_, 0);
      Assert.That(obj, Is.AssignableTo<TestFactory>());
      Assert.That(((TestFactory) obj).Arg, Is.EqualTo(0));

      obj = RuntimeTypeFactory<ITestFactory>.CreateInstanceFallback(node_, 10);
      Assert.That(obj, Is.AssignableTo<TestFactory>());
      Assert.That(((TestFactory) obj).Arg, Is.EqualTo(10));

      obj = RuntimeTypeFactory<ITestFactory>.CreateInstanceNoException(node_, 20);
      Assert.That(obj, Is.AssignableTo<TestFactory>());
      Assert.That(((TestFactory) obj).Arg, Is.EqualTo(20));
    }

    [Test]
    public void ShouldUseDefaultConstructorWhenMatchIsNotFound() {
      object obj = RuntimeTypeFactory<ITestFactory>
        .CreateInstanceFallback(node_, string.Empty);
      Assert.That(obj, Is.AssignableTo<TestFactory>());
      Assert.That(((TestFactory) obj).Arg, Is.EqualTo(-1));
    }

    [Test]
    public void ShouldMatchUnorderedListofParameters() {
      TestFactory test;
      object obj = RuntimeTypeFactory<ITestFactory>.CreateInstance(node_, "a1",
        0);
      Assert.That(obj, Is.AssignableTo<TestFactory>());
      test = (TestFactory) obj;
      Assert.That(test.Arg, Is.EqualTo(0));
      Assert.That(test.StringArg, Is.EqualTo("a1"));

      obj = RuntimeTypeFactory<ITestFactory>.CreateInstanceFallback(node_, "a2",
        10);
      Assert.That(obj, Is.AssignableTo<TestFactory>());
      test = (TestFactory) obj;
      Assert.That(test.Arg, Is.EqualTo(10));
      Assert.That(test.StringArg, Is.EqualTo("a2"));

      obj = RuntimeTypeFactory<ITestFactory>.CreateInstanceNoException(node_,
        "a3", 20);
      Assert.That(obj, Is.AssignableTo<TestFactory>());
      test = (TestFactory) obj;
      Assert.That(test.Arg, Is.EqualTo(20));
      Assert.That(test.StringArg, Is.EqualTo("a3"));
    }

    [Test]
    public void ShouldRespectArgumentOrder() {
      TestFactory test;
      object obj = RuntimeTypeFactory<ITestFactory>.CreateInstance(node_, "a1",
        0, 2);
      Assert.That(obj, Is.AssignableTo<TestFactory>());
      test = (TestFactory) obj;
      Assert.That(test.Arg, Is.EqualTo(0));
      Assert.That(test.StringArg, Is.EqualTo("a1"));
      Assert.That(test.Arg3, Is.EqualTo(2));

      obj = RuntimeTypeFactory<ITestFactory>.CreateInstanceFallback(node_, "a2",
        10, 3);
      Assert.That(obj, Is.AssignableTo<TestFactory>());
      test = (TestFactory) obj;
      Assert.That(test.Arg, Is.EqualTo(10));
      Assert.That(test.StringArg, Is.EqualTo("a2"));
      Assert.That(test.Arg3, Is.EqualTo(3));

      obj = RuntimeTypeFactory<ITestFactory>.CreateInstanceNoException(node_,
        "a3", 20, 4);
      Assert.That(obj, Is.AssignableTo<TestFactory>());
      test = (TestFactory) obj;
      Assert.That(test.Arg, Is.EqualTo(20));
      Assert.That(test.StringArg, Is.EqualTo("a3"));
      Assert.That(test.Arg3, Is.EqualTo(4));
    }

    [Test]
    public void ShouldRecognizeDerivedTypes() {
      var base_type = new BaseType();
      try {
        object obj = RuntimeTypeFactory<ITestFactory>
          .CreateInstance(node_, base_type);
      } catch {
        Assert.Fail(
          "Derived type should be accepted as argument for base type parameter");
      }
    }

    [Test]
    public void ShouldDiscardNonDeclaredParameters() {
      var base_type = new BaseType();
      try {
        object obj = RuntimeTypeFactory<ITestFactory>
          .CreateInstance(node_, base_type, string.Empty);
      } catch {
        Assert.Fail("Non declared parameters should be discarded");
      }
    }
  }
}
