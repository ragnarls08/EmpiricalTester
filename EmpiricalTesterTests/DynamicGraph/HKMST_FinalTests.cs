using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EmpiricalTester.DynamicGraph.Tests
{
    [TestClass()]
    public class HKMST_FinalTests
    {
        [TestMethod()]
        public void addEdgeTest()
        {
            var hkmst = new HKMST_Final(0.65);
            //var hkmst = new SimpleIncremental();
            for (int i = 0; i < 7; i++)
                hkmst.addVertex();


            var b1 = hkmst.addEdge(5, 1);
            var b2 = hkmst.addEdge(2, 4);
            var b3 = hkmst.addEdge(1, 2);

            var b4 = hkmst.addEdge(5, 6);
            var b5 = hkmst.addEdge(1, 0);
            var b6 = hkmst.addEdge(6, 1);

            var b7 = hkmst.addEdge(3, 2);
            var b8 = hkmst.addEdge(3, 6);
            var b9 = hkmst.addEdge(0, 3);

            //var b10 = hkmst.addEdge(1, 6);
            //var b11 = hkmst.addEdge(2, 1);
            
            Assert.Fail();
        }
    }
}