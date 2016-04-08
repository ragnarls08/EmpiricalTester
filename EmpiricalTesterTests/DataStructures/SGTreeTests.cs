using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            Assert.IsTrue(sgt.Query(two, root));
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

            bool aGTroot = sgt.Query(a, root);
            bool bGTa = sgt.Query(b, a);
            bool aGTc = sgt.Query(a, c);
            bool dGTa = sgt.Query(d, a);
            bool dGTb = sgt.Query(d, b);

            Assert.IsTrue(aGTroot && bGTa && aGTc && dGTa && dGTb);
        }

        [TestMethod()]
        public void RemoveTestRightmostIsLeft()
        {
            var sgt = new SGTree<int>(0.15);
            var root = sgt.insertFirst(1);
            var two = sgt.insertAfter(root, 2);
            var three = sgt.insertAfter(root, 3);
            var four = sgt.insertAfter(two, 4);
            sgt.insertAfter(root, 5);
            sgt.insertAfter(three, 6);
            sgt.insertAfter(two, 7);
            sgt.insertAfter(four, 8);

            // Test when Remove node has 2 children and the left child rightmost node is the left child
            //   1
            //    \
            //     6
            //    / \
            //   3   4  <--- Remove
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

            sgt.Remove(four);
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

            bool aGTroot = sgt.Query(root, a);
            bool bGTa = sgt.Query(a, b);
            bool aGTc = sgt.Query(c, a);
            bool dGTa = sgt.Query(a, d);
            bool dGTb = sgt.Query(b, d);

            Assert.IsTrue(aGTroot && bGTa && aGTc && dGTa && dGTb);
        }

        [TestMethod()]
        public void RemoveTestNoChildrenNotRoot()
        {
            SGTree<int> sgt = new SGTree<int>(0.75);

            SGTNode<int> a = new SGTNode<int>(0);
            SGTNode<int> b = new SGTNode<int>(1);
            SGTNode<int> c = new SGTNode<int>(2);
            
            sgt.insertFirst(a);
            sgt.insertBefore(a, b);
            sgt.insertBefore(b, c);
            
            sgt.Remove(c);

            bool b1 = sgt.Query(a, b);
            
            Assert.IsTrue(b1);
        }

        [TestMethod()]
        public void RemoveTestNoChildrenIsRoot()
        {
            SGTree<int> sgt = new SGTree<int>(0.75);
            SGTNode<int> a = new SGTNode<int>(0);

            sgt.insertFirst(a);
            sgt.Remove(a);

            bool b1 = sgt.Root == null;

            Assert.IsTrue(b1);
        }

        [TestMethod()]
        public void RemoveTestLeftChildNotRoot()
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
                        
            sgt.Remove(c);

            bool b1 = sgt.Query(a, b);
            bool b2 = sgt.Query(b, d);
            bool b3 = sgt.Query(a, d);

            Assert.IsTrue(b1 && b2 && b3);
        }

        [TestMethod()]
        public void RemoveTestLeftChildIsRoot()
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

            sgt.Remove(a);

            bool b1 = sgt.Query(c, d);
            bool b2 = sgt.Query(b, c);
            bool b3 = sgt.Query(b, d);

            Assert.IsTrue(b1 && b2 && b3);
        }

        [TestMethod()]
        public void RemoveTestTwoChildrenNotRootRighstmostIsLeft()
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

            sgt.Remove(b);

            bool b1 = sgt.Query(a, c);
            bool b2 = sgt.Query(a, d);
            bool b3 = sgt.Query(d, c);

            Assert.IsTrue(b1 && b2 && b3);
        }

        [TestMethod()]
        public void RemoveTestTwoChildrenIsRootRightmostIsLeft()
        {
            SGTree<int> sgt = new SGTree<int>(0.75);
            
            SGTNode<int> b = new SGTNode<int>(1);
            SGTNode<int> c = new SGTNode<int>(2);
            SGTNode<int> d = new SGTNode<int>(3);

            sgt.insertFirst(b);
            sgt.insertBefore(b, c);
            sgt.insertAfter(b, d);

            sgt.Remove(b);

            bool b1 = sgt.Query(d, c);            

            Assert.IsTrue(b1);
        }

        [TestMethod()]
        public void RemoveTestTwoChildrenIsRootRightmostIsNotLeft()
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

            sgt.Remove(a);

            bool b1 = sgt.Query(c, d);
            bool b2 = sgt.Query(c, b);

            Assert.IsTrue(b1 && b2);
        }

        [TestMethod()]
        public void RemoveTestTwoChildrenNotRootRightmostIsNotLeft()
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

            sgt.Remove(a);

            bool b1 = sgt.Query(c, d);
            bool b2 = sgt.Query(c, b);

            Assert.IsTrue(b1 && b2);
        }
        
    }
}