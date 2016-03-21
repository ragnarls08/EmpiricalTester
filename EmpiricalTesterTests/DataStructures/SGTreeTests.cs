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
            SGTree<int> sgt = new SGTree<int>(0.75);
            SGTNode<int> root = sgt.insertFirst(1);
            SGTNode<int> two = sgt.insertAfter(root, 2);

            Assert.IsTrue(sgt.query(root, two));
        }
        [TestMethod()]
        public void insertTestNotSequential()
        {
            // root > a
            // a > b
            // c > a
            // a > d && b > d

            SGTree<int> sgt = new SGTree<int>(0.75);
            SGTNode<int> root = sgt.insertFirst(1);
            SGTNode<int> a = sgt.insertAfter(root, 2);
            SGTNode<int> b = sgt.insertAfter(a, 3);
            SGTNode<int> c = sgt.insertAfter(root, 4);
            SGTNode<int> d = sgt.insertAfter(b, 5);

            bool aGTroot = sgt.query(root, a);
            bool bGTa = sgt.query(a, b);
            bool aGTc = sgt.query(c, a);
            bool dGTa = sgt.query(a, d);
            bool dGTb = sgt.query(b, d);

            Assert.IsTrue(aGTroot && bGTa && aGTc && dGTa && dGTb);
        }

        [TestMethod()]
        public void removeTestRightmostIsLeft()
        {
            SGTree<int> sgt = new SGTree<int>(0.15);
            SGTNode<int> root = sgt.insertFirst(1);
            SGTNode<int> two = sgt.insertAfter(root, 2);
            SGTNode<int> three = sgt.insertAfter(root, 3);
            SGTNode<int> four = sgt.insertAfter(two, 4);
            SGTNode<int> five = sgt.insertAfter(root, 5);
            SGTNode<int> six = sgt.insertAfter(three, 6);
            SGTNode<int> seven = sgt.insertAfter(two, 7);
            SGTNode<int> eight = sgt.insertAfter(four, 8);

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

        [TestMethod()]
        public void insertBeforeTest()
        {
            // a < root
            // b < a
            // a < c
            // d < a && d < b

            SGTree<int> sgt = new SGTree<int>(0.75);
            SGTNode<int> root = sgt.insertFirst(1);
            SGTNode<int> a = sgt.insertBefore(root, 2);
            SGTNode<int> b = sgt.insertBefore(a, 3);
            SGTNode<int> c = sgt.insertBefore(root, 4);
            SGTNode<int> d = sgt.insertBefore(b, 5);

            bool aGTroot = sgt.query(root, a);
            bool bGTa = sgt.query(a, b);
            bool aGTc = sgt.query(c, a);
            bool dGTa = sgt.query(a, d);
            bool dGTb = sgt.query(b, d);

            Assert.IsTrue(!aGTroot && !bGTa && !aGTc && !dGTa && !dGTb);
        }

        [TestMethod()]
        public void removeTestNoChildrenNotRoot()
        {
            SGTree<int> sgt = new SGTree<int>(0.75);

            SGTNode<int> a = new SGTNode<int>(0);
            SGTNode<int> b = new SGTNode<int>(1);
            SGTNode<int> c = new SGTNode<int>(2);
            
            sgt.insertFirst(a);
            sgt.insertBefore(a, b);
            sgt.insertBefore(b, c);
            
            sgt.remove(c);

            bool b1 = sgt.query(b, a);
            
            Assert.IsTrue(b1);
        }

        [TestMethod()]
        public void removeTestNoChildrenIsRoot()
        {
            SGTree<int> sgt = new SGTree<int>(0.75);
            SGTNode<int> a = new SGTNode<int>(0);

            sgt.insertFirst(a);
            sgt.remove(a);

            bool b1 = sgt.Root == null;

            Assert.IsTrue(b1);
        }

        [TestMethod()]
        public void removeTestLeftChildNotRoot()
        {
            SGTree<int> sgt = new SGTree<int>(0.75);

            SGTNode<int> a = new SGTNode<int>(0);
            SGTNode<int> b = new SGTNode<int>(1);
            SGTNode<int> c = new SGTNode<int>(2);
            SGTNode<int> d = new SGTNode<int>(3);

            sgt.insertFirst(a);
            sgt.insertBefore(a, b);
            sgt.insertBefore(b, c);
            sgt.insertBefore(c, d);
                        
            sgt.remove(c);

            bool b1 = sgt.query(b, a);
            bool b2 = sgt.query(d, b);
            bool b3 = sgt.query(d, a);

            Assert.IsTrue(b1 && b2 && b3);
        }

        [TestMethod()]
        public void removeTestLeftChildIsRoot()
        {
            SGTree<int> sgt = new SGTree<int>(0.75);

            SGTNode<int> a = new SGTNode<int>(0);
            SGTNode<int> b = new SGTNode<int>(1);
            SGTNode<int> c = new SGTNode<int>(2);
            SGTNode<int> d = new SGTNode<int>(3);

            sgt.insertFirst(a);
            sgt.insertBefore(a, b);
            sgt.insertBefore(b, c);
            sgt.insertBefore(c, d);

            sgt.remove(a);

            bool b1 = sgt.query(d, c);
            bool b2 = sgt.query(c, b);
            bool b3 = sgt.query(d, b);

            Assert.IsTrue(b1 && b2 && b3);
        }

        [TestMethod()]
        public void removeTestTwoChildrenNotRootRighstmostIsLeft()
        {
            SGTree<int> sgt = new SGTree<int>(0.75);

            SGTNode<int> a = new SGTNode<int>(0);
            SGTNode<int> b = new SGTNode<int>(1);
            SGTNode<int> c = new SGTNode<int>(2);
            SGTNode<int> d = new SGTNode<int>(3);

            sgt.insertFirst(a);
            sgt.insertBefore(a, b);
            sgt.insertBefore(b, c);
            sgt.insertAfter(b, d);

            sgt.remove(b);

            bool b1 = sgt.query(c, a);
            bool b2 = sgt.query(d, a);
            bool b3 = sgt.query(c, d);

            Assert.IsTrue(b1 && b2 && b3);
        }

        [TestMethod()]
        public void removeTestTwoChildrenIsRootRightmostIsLeft()
        {
            SGTree<int> sgt = new SGTree<int>(0.75);
            
            SGTNode<int> b = new SGTNode<int>(1);
            SGTNode<int> c = new SGTNode<int>(2);
            SGTNode<int> d = new SGTNode<int>(3);

            sgt.insertFirst(b);
            sgt.insertBefore(b, c);
            sgt.insertAfter(b, d);

            sgt.remove(b);

            bool b1 = sgt.query(c, d);            

            Assert.IsTrue(b1);
        }

        [TestMethod()]
        public void removeTestTwoChildrenIsRootRightmostIsNotLeft()
        {
            SGTree<int> sgt = new SGTree<int>(0.75);

            SGTNode<int> a = new SGTNode<int>(0);
            SGTNode<int> b = new SGTNode<int>(1);
            SGTNode<int> c = new SGTNode<int>(2);
            SGTNode<int> d = new SGTNode<int>(3);
            SGTNode<int> e = new SGTNode<int>(4);

            sgt.insertFirst(a);
            sgt.insertAfter(a, e);
            sgt.insertBefore(a, b);
            sgt.insertAfter(b, c);
            sgt.insertBefore(c, d);

            sgt.remove(a);

            bool b1 = sgt.query(d, c);
            bool b2 = sgt.query(b, c);

            Assert.IsTrue(b1 && b2);
        }

        [TestMethod()]
        public void removeTestTwoChildrenNotRootRightmostIsNotLeft()
        {
            SGTree<int> sgt = new SGTree<int>(0.75);

            SGTNode<int> x = new SGTNode<int>(10);
            SGTNode<int> a = new SGTNode<int>(0);
            SGTNode<int> b = new SGTNode<int>(1);
            SGTNode<int> c = new SGTNode<int>(2);
            SGTNode<int> d = new SGTNode<int>(3);
            SGTNode<int> e = new SGTNode<int>(4);

            sgt.insertFirst(x);
            sgt.insertAfter(x, a);
            sgt.insertAfter(a, e);
            sgt.insertBefore(a, b);
            sgt.insertAfter(b, c);
            sgt.insertBefore(c, d);

            sgt.remove(a);

            bool b1 = sgt.query(d, c);
            bool b2 = sgt.query(b, c);

            Assert.IsTrue(b1 && b2);
        }
        
    }
}