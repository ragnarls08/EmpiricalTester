using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmpiricalTester.GraphGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.GraphGeneration.Tests
{
    [TestClass()]
    public class ConnectivityGraphTests
    {
        [TestMethod()]
        public void generateConnectivityMatrixTest()
        {
            ConnectivityGraph cGraph = new ConnectivityGraph();
            for(int i = 0; i < 5; i++)
            {
                cGraph.addVertex();
            }

            cGraph.addEdge(0, 2);
            cGraph.addEdge(0, 1);
            cGraph.addEdge(2, 1);
            cGraph.addEdge(1, 3);
            cGraph.addEdge(3, 4);

            var connectivity = cGraph.generateConnectivityMatrix();

            Assert.Fail();
        }
    }
}