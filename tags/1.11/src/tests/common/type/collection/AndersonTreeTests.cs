using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Nohros.Collections
{
  public class AndersonTreeTests
  {
    [Test]
    public void ShouldGetTheFirstNode() {
      AndersonTree<int, string> tree = new AndersonTree<int, string>();
      Assert.AreEqual(0, tree.First.Key);

      tree.Add(10, "10");
      Assert.AreEqual(10, tree.First.Key);

      tree.Add(11, "11");
      Assert.AreEqual(10, tree.First.Key);

      tree.Add(28, "28");
      Assert.AreEqual(10, tree.First.Key);

      tree.Add(5, "5");
      Assert.AreEqual(5, tree.First.Key);

      tree.Add(50, "50");
      Assert.AreEqual(5, tree.First.Key);
    }

    [Test]
    public void ShouldGetTheLastNode() {
      AndersonTree<int, string> tree = new AndersonTree<int, string>();
      Assert.AreEqual(0, tree.First.Key);

      tree.Add(28, "28");
      Assert.AreEqual(28, tree.Last.Key);

      tree.Add(11, "11");
      Assert.AreEqual(28, tree.Last.Key);

      tree.Add(10, "10");
      Assert.AreEqual(28, tree.Last.Key);

      tree.Add(50, "50");
      Assert.AreEqual(50, tree.Last.Key);

      tree.Add(12, "12");
      Assert.AreEqual(50, tree.Last.Key);
    }

    [Test]
    public void ShouldClearTheTree() {
      AndersonTree<int, int> tree = new AndersonTree<int, int>();
      tree.Add(10, 10);
      tree.Add(12, 32);
      tree.Add(56, 56);
      tree.Add(89, 89);
      Assert.AreNotEqual(0, tree.Count);
      tree.Clear();
      Assert.AreEqual(0, tree.Count);
    }

    [Test]
    public void ShouldCopyTheTreeToArray() {
      AndersonTree<int, int> tree = new AndersonTree<int, int>();

      int[] data = new int[10];
      for (int i = 0; i < data.Length; i++) {
        data[i] = i*10;
        tree.Add(data[i], data[i]);
      }

      KeyValuePair<int, int>[] copy = new KeyValuePair<int, int>[tree.Count];
      tree.CopyTo(copy, 0);

      for (int i = 0; i < data.Length; i++) {
        Assert.AreEqual(data[i], copy[i].Value);
      }

      KeyValuePair<int, int>[] middle_copy =
        new KeyValuePair<int, int>[tree.Count*2];
      tree.CopyTo(middle_copy, tree.Count);

      for (int i = 0; i < data.Length; i++) {
        Assert.AreEqual(data[i], middle_copy[i + data.Length].Value);
      }
    }

    [Test]
    public void ShouldNotAllowDuplicates() {
      AndersonTree<int, int> tree = new AndersonTree<int, int>();
      tree.Add(10, 10);

      Assert.Throws<ArgumentException>(() => tree.Add(10, 10));
    }

    [Test]
    public void ShouldNotAcceptNullKeys() {
      AndersonTree<string, string> tree = new AndersonTree<string, string>();
      Assert.Throws<ArgumentNullException>(() => tree.Add(null, ""));
    }

    [Test]
    public void ShouldAcceptNullValues() {
      AndersonTree<string, string> tree = new AndersonTree<string, string>();
      Assert.DoesNotThrow(() => tree.Add("", null));
    }

    [Test]
    public void ShouldVisitEachNodeInAscendingOrder() {
      AndersonTree<int, int> tree = new AndersonTree<int, int>();

      Queue<int> data = new Queue<int>(10);
      for (int i = 0; i < 10; i++) {
        data.Enqueue(i*10);
        tree.Add(i*10, i*10);
      }

      var visitor =
        new InOrderVisitor<int, int>(
          new Visitor<int, int>(
            (key, value, state) => Assert.AreEqual(data.Dequeue(), value)));
      tree.Accept(visitor, false);
    }

    [Test]
    public void ShouldVisitEachNodeInDescendingOrder() {
      AndersonTree<int, int> tree = new AndersonTree<int, int>();

      Stack<int> data = new Stack<int>(10);
      for (int i = 0; i < 10; i++) {
        data.Push(i*10);
        tree.Add(i*10, i*10);
      }

      var visitor =
        new InOrderVisitor<int, int>(
          new Visitor<int, int>(
            (key, value, state) => Assert.AreEqual(data.Pop(), value)));
      tree.Accept(visitor, true);
    }
  }
}
