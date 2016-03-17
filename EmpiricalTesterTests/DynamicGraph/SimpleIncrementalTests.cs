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
    public class SimpleIncrementalTests
    {
        [TestMethod()]
        public void addEdgeTest()
        {
            var simple = new SimpleIncremental();
            for (int i = 0; i < 5; i++)
                simple.addVertex();

            var b1 = simple.addEdge(0, 1);
            var b2 = simple.addEdge(1, 2);
            var b3 = simple.addEdge(2, 3);
            var b4 = simple.addEdge(3, 4);
            var b5 = simple.addEdge(4, 0);

            var b = b1 && b2 && b3 && b4 && !b5;

            Assert.IsTrue(b);
        }
    }
}