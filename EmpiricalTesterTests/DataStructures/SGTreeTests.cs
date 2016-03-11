using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmpiricalTester.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.DataStructures.Tests
{
    [TestClass()]
    public class SGTreeTests
    {
        [TestMethod()]
        public void insertTestBasic()
        {
            SGTree<int> sgt = new SGTree<int>();
            SGTNode<int> root = sgt.insertFirst(1);
            SGTNode<int> two = sgt.insert(root, 2);

            Assert.IsTrue(sgt.query(two, root));
        }
        [TestMethod()]
        public void insertTestNotSequential()
        {
            // a > root
            // b > a
            // a > c
            // d > a && d > b

            SGTree<int> sgt = new SGTree<int>();
            SGTNode<int> root = sgt.insertFirst(1);
            SGTNode<int> a = sgt.insert(root, 2);
            SGTNode<int> b = sgt.insert(a, 3);
            SGTNode<int> c = sgt.insert(root, 4);
            SGTNode<int> d = sgt.insert(b, 5);

            bool aGTroot = sgt.query(a, root);
            bool bGTa = sgt.query(b, a);
            bool aGTc = sgt.query(a, c);
            bool dGTa = sgt.query(d, a);
            bool dGTb = sgt.query(d, b);

            Assert.IsTrue(aGTroot && bGTa && aGTc && dGTa && dGTb);
        }

        [TestMethod()]
        public void removeTestRightmostIsLeft()
        {
            SGTree<int> sgt = new SGTree<int>();
            SGTNode<int> root = sgt.insertFirst(1);
            SGTNode<int> two = sgt.insert(root, 2);
            SGTNode<int> three = sgt.insert(root, 3);
            SGTNode<int> four = sgt.insert(two, 4);
            SGTNode<int> five = sgt.insert(root, 5);
            SGTNode<int> six = sgt.insert(three, 6);
            SGTNode<int> seven = sgt.insert(two, 7);
            SGTNode<int> eight = sgt.insert(four, 8);

            // Test when remove node has 2 children and the left child rightmost node is the left child
            //   1
            //    \
            //     6
            //    / \
            //   3   4  <--- remove
            //  /   / \
            // 5   7   8
            //    /
            //   2
            //
            // should result in
            //   1
            //    \
            //     6
            //    / \
            //   3   7  
            //  /   / \
            // 5   2   8
            //    

            sgt.remove(four);
            bool a = root.Value == 1;
            bool b = root.Right.Value == 6;
            bool c = root.Right.Left.Value == 3 && root.Right.Left.Left.Value == 5;
            bool d = root.Right.Right.Value == 7 && root.Right.Right.Left.Value == 2 && root.Right.Right.Right.Value == 8;


            Assert.IsTrue(a && b && c && d);
        }
    }
}