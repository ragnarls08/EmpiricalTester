using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmpiricalTester.DynamicGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.DynamicGraph.Tests
{
    [TestClass()]
    public class HKMST_V1Tests
    {
        [TestMethod()]
        public void addEdgeTest()
        {
            var hkmst = new HKMST_V1(0.75);
            for (int i = 0; i < 5; i++)
                hkmst.addVertex();

            var b1 = hkmst.addEdge(0, 1);
            var b2 = hkmst.addEdge(1, 2);
            var b3 = hkmst.addEdge(2, 3);
            var b4 = hkmst.addEdge(3, 4);
            var b5 = hkmst.addEdge(4, 0);

            var b = b1 && b2 && b3 && b4 && !b5;

            Assert.IsTrue(b);
        }

        [TestMethod()]
        public void addEdgeTest2()
        {
            var hkmst = new HKMST_V1(0.75);
            for (int i = 0; i < 3; i++)
                hkmst.addVertex();

            var b1 = hkmst.addEdge(0, 1);
            var b2 = hkmst.addEdge(1, 2);
            var b3 = hkmst.addEdge(2, 3);
            
            var b = b1 && b2 && b3;

            Assert.IsTrue(b);
        }
    }
}