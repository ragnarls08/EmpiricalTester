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
    public class PearceKellyTests
    {
        [TestMethod()]
        public void PKAddEdgeTest()
        {
            var pk = new PearceKelly();
            for(int i = 0; i < 3; i++)
                pk.AddVertex();

            var b1 = pk.AddEdge(0, 1);
            var b2 = pk.AddEdge(1, 2);
            var b3 = pk.AddEdge(2, 0);
            
            Assert.Fail();
        }
    }
}