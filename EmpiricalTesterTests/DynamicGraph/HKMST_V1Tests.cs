using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmpiricalTester.DynamicGraph.Tests
{
    [TestClass()]
    public class HkmstV1Tests
    {
        [TestMethod()]
        public void AddEdgeTest()
        {
            var hkmst = new HKMSTV1(0.75);
            for (var i = 0; i < 5; i++)
                hkmst.AddVertex();

            var b1 = hkmst.AddEdge(0, 1);
            var b2 = hkmst.AddEdge(1, 2);
            var b3 = hkmst.AddEdge(2, 3);
            var b4 = hkmst.AddEdge(3, 4);
            var b5 = hkmst.AddEdge(4, 0);

            var b = b1 && b2 && b3 && b4 && !b5;

            Assert.IsTrue(b);
        }

        [TestMethod()]
        public void AddEdgeTest2()
        {
            var hkmst = new HKMSTV1(0.75);
            for (int i = 0; i < 5; i++)
                hkmst.AddVertex();

            var b1 = hkmst.AddEdge(1, 0);
            var b2 = hkmst.AddEdge(0, 4);
            var b3 = hkmst.AddEdge(4, 1);
            var b4 = hkmst.AddEdge(1, 3);

            var b = b1 && b2 && !b3 && b4;

            Assert.IsTrue(b);
        }

        [TestMethod()]
        public void AddEdgeTest3()
        {
            var hkmst = new HKMSTV1(0.75);
            for (int i = 0; i < 5; i++)
                hkmst.AddVertex();

            var b1 = hkmst.AddEdge(2, 4);
            var b2 = hkmst.AddEdge(0, 4);
            var b3 = hkmst.AddEdge(3, 2);
            var b4 = hkmst.AddEdge(1, 0);
            var b5 = hkmst.AddEdge(1, 4);
            var b6 = hkmst.AddEdge(2, 1);

            var b = b1 && b2 && b3 && b4 && b5 && b6;

            Assert.IsTrue(b);
        }
    }
}