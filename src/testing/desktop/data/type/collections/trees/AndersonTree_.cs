using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Nohros.Data;
using Nohros.Data.Collections;

namespace Nohros.Test.Data.Tree
{
    [TestFixture]
    public class AndersonTree_
    {
        [Test]
        public void Add()
        {
            AndersonTree<int, int> tree = new AndersonTree<int, int>();
            tree.Add(0, 0);
            tree.Add(1, 1);

            // split must perform a left rotation and increase
            // the level of the node with key 1.
            tree.Add(2, 2);
            AndersonTreeNode<int, int> node = tree.FindNode(1);
            Assert.AreEqual(node.Level, 2);
            Assert.AreEqual(node.Left.Level, 1);
            Assert.AreEqual(node.Right.Level, 1);
            Assert.AreEqual(node.Left.Value, 0);
            Assert.AreEqual(node.Right.Value, 2);

            tree.Add(3, 3);
            tree.Add(4, 4);
            tree.Add(5, 5);
            tree.Add(6, 6);

            // must be balanced
            node = tree.FindNode(3);
            Assert.AreEqual(node.Value, 3);
            Assert.AreEqual(node.Left.Value, 1);
            Assert.AreEqual(node.Left.Left.Value, 0);
            Assert.AreEqual(node.Left.Right.Value, 2);
            Assert.AreEqual(node.Right.Value, 5);
            Assert.AreEqual(node.Right.Left.Value, 4);
            Assert.AreEqual(node.Right.Right.Value, 6);

            Assert.IsTrue(tree.ContainsKey(0));
            Assert.IsTrue(tree.ContainsKey(1));
            Assert.IsFalse(tree.ContainsKey(10));
            Assert.IsNotNull(tree[4]);

            Assert.Throws(typeof(KeyNotFoundException), delegate() { int v = tree[10]; });
            Assert.Throws(typeof(ArgumentException), delegate() { tree.Add(0, 0); });

            // set value
            Assert.DoesNotThrow(delegate() { tree[0] = 20; });
            int i = tree[0];
            Assert.AreEqual(i, 20);
        }

        [Test]
        public void Clear()
        {
            AndersonTree<int, int> tree = new AndersonTree<int, int>();
            tree.Add(0, 0);
            tree.Add(1, 1);
            tree.Add(2, 2);
            tree.Add(3, 3);
            tree.Add(4, 4);
            tree.Add(5, 5);
            tree.Add(6, 6);

            tree.Clear();

            Assert.AreEqual(tree.Count, 0);
        }

        [Test]
        public void InOrderTraversal()
        {
            AndersonTree<int, int> tree = new AndersonTree<int, int>();
            tree.Add(0, 0);
            tree.Add(1, 1);
            tree.Add(2, 2);
            tree.Add(3, 3);
            tree.Add(4, 4);
            tree.Add(5, 5);
            tree.Add(6, 6);

            List<AndersonTreeNode<int, int>> array = new List<AndersonTreeNode<int,int>>(6);

            tree.InOrderTreeWalk(delegate(AndersonTreeNode<int, int> node) {
                array.Add(node);
                return true;
            });

            Assert.AreEqual(array[0].Value, 0);
            Assert.AreEqual(array[1].Value, 1);
            Assert.AreEqual(array[2].Value, 2);
            Assert.AreEqual(array[3].Value, 3);
            Assert.AreEqual(array[4].Value, 4);
            Assert.AreEqual(array[5].Value, 5);
            Assert.AreEqual(array[6].Value, 6);

            // unordered inert
            tree.Clear();
            tree.Add(4, 4);
            tree.Add(0, 0);
            tree.Add(1, 1);
            tree.Add(6, 6);
            tree.Add(2, 2);
            tree.Add(5, 5);
            tree.Add(3, 3);

            Assert.AreEqual(array[0].Value, 0);
            Assert.AreEqual(array[1].Value, 1);
            Assert.AreEqual(array[2].Value, 2);
            Assert.AreEqual(array[3].Value, 3);
            Assert.AreEqual(array[4].Value, 4);
            Assert.AreEqual(array[5].Value, 5);
            Assert.AreEqual(array[6].Value, 6);
        }

        #region TreeOrderedVisitor
        class TreeOrderedVisitor : IVisitor<int> {

            public TreeOrderedVisitor() {
                values_ = new List<int>();
            }

            List<int> values_;

            public void Visit(int value, object state) {
                values_.Add(value);
            }

            public bool IsCompleted {
                get { return false; }
            }

            public object State {
                get { return null; }
            }

            public List<int> Values {
                get { return values_; }
            }
        }
        #endregion

        [Test]
        public void InOrderAccept() {
            AndersonTree<int, int> tree = new AndersonTree<int,int>();
            tree.Add(0, 0);
            tree.Add(1, 1);
            tree.Add(2, 2);
            tree.Add(3, 3);
            tree.Add(4, 4);
            tree.Add(5, 5);
            tree.Add(6, 6);

            TreeOrderedVisitor visitor = new TreeOrderedVisitor();
            tree.Accept(new InOrderVisitor<int>(visitor), null, false);

            Assert.AreEqual(7, visitor.Values.Count);

            for (int i = 0, j = visitor.Values.Count; i < j; i++) {
                Assert.AreEqual(tree[i], visitor.Values[i]);
            }
        }

        [Test]
        public void ReverseInOrderAccept() {
            AndersonTree<int, int> tree = new AndersonTree<int, int>();
            tree.Add(0, 0);
            tree.Add(1, 1);
            tree.Add(2, 2);
            tree.Add(3, 3);
            tree.Add(4, 4);
            tree.Add(5, 5);
            tree.Add(6, 6);

            TreeOrderedVisitor visitor = new TreeOrderedVisitor();
            tree.Accept(new InOrderVisitor<int>(visitor), null, true);

            Assert.AreEqual(7, visitor.Values.Count);

            for (int i = 0, j = visitor.Values.Count; i < j; i++) {
                Assert.AreEqual(tree[tree.Count - i - 1], visitor.Values[i]);
            }
        }

        [Test]
        public void CopyTo()
        {
            AndersonTree<int, int> tree = new AndersonTree<int, int>();
            tree.Add(0, 0);
            tree.Add(1, 1);
            tree.Add(2, 2);
            tree.Add(3, 3);
            tree.Add(4, 4);
            tree.Add(5, 5);
            tree.Add(6, 6);

            KeyValuePair<int, int>[] array = new KeyValuePair<int,int>[7];

            Assert.DoesNotThrow(delegate() { tree.CopyTo(array, 0); });
            Assert.AreEqual(array[0].Value, 0);
            Assert.AreEqual(array[2].Value, 2);
            Assert.AreEqual(array[3].Value, 3);
            Assert.AreEqual(array[6].Value, 6);

            Assert.Throws<ArgumentOutOfRangeException>(delegate() { tree.CopyTo(array, -1); });
            Assert.Throws<ArgumentException>(delegate() { tree.CopyTo(array, 3); });

            KeyValuePair<int, int>[] array2 = new KeyValuePair<int, int>[10];
            Array.Copy(array, array2, array.Length);

            tree[0] = 20;
            tree[1] = 20;
            Assert.DoesNotThrow(delegate() { tree.CopyTo(array2, 3); });
            Assert.AreEqual(array2[0].Value, 0);
            Assert.AreEqual(array2[1].Value, 1);
            Assert.AreEqual(array2[2].Value, 2);
            Assert.AreEqual(array2[3].Key, 0);
            Assert.AreEqual(array2[3].Value, 20);
            Assert.AreEqual(array2[4].Key, 1);
            Assert.AreEqual(array2[4].Value, 20);
        }

        [Test]
        public void Remove()
        {
            AndersonTree<int, int> tree = new AndersonTree<int, int>();
            tree.Add(0, 0);
            tree.Add(1, 1);
            tree.Add(2, 2);
            tree.Add(3, 3);
            tree.Add(4, 4);
            tree.Add(5, 5);
            tree.Add(6, 6);
            tree.Add(7, 7);

            // removing zero will cause a break in the levels between 1 and nil,
            // so the level of 1 is decreased to 1. Then break is between 1 and 3,
            // so the level of 3 is decreased to 2.
            tree.Remove(0);
            AndersonTreeNode<int, int> node = tree.FindNode(3);

            // levels
            Assert.AreEqual(node.Level, 2);
            Assert.AreEqual(node.Left.Level, 1);
            Assert.AreEqual(node.Left.Right.Level, 1);
            Assert.AreEqual(node.Right.Level, 2);
            Assert.AreEqual(node.Right.Left.Level, 1);
            Assert.AreEqual(node.Right.Right.Level, 1);

            // values
            Assert.AreEqual(node.Value, 3);
            Assert.AreEqual(node.Left.Value, 1);
            Assert.AreEqual(node.Left.Right.Value, 2);
            Assert.AreEqual(node.Right.Value, 5);
            Assert.AreEqual(node.Right.Left.Value, 4);
            Assert.AreEqual(node.Right.Right.Value, 6);

            Assert.IsTrue(tree.Remove(1));
            Assert.IsFalse(tree.ContainsKey(1));
            Assert.Throws(typeof(KeyNotFoundException), delegate() { int val = tree[1]; });

            // the real remove test
            node = tree.FindNode(3);
            
            // levels
            Assert.AreEqual(node.Level, 2);
            Assert.AreEqual(node.Left.Level, 1);
            Assert.AreEqual(node.Right.Level, 2);
            Assert.AreEqual(node.Right.Left.Level, 1);
            Assert.AreEqual(node.Right.Right.Level, 1);
            Assert.AreEqual(node.Right.Right.Right.Level, 1);

            // values
            Assert.AreEqual(node.Value, 3);
            Assert.AreEqual(node.Left.Value, 2);
            Assert.AreEqual(node.Right.Value, 5);
            Assert.AreEqual(node.Right.Left.Value, 4);
            Assert.AreEqual(node.Right.Right.Value, 6);
            Assert.AreEqual(node.Right.Right.Right.Value, 7);
        }

        [Test]
        public void Count()
        {
            AndersonTree<int, int> tree = new AndersonTree<int, int>();
            tree.Add(0, 0);
            Assert.AreEqual(tree.Count, 1);
        }
    }
}
