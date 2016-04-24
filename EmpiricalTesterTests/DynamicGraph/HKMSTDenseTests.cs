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
    public class HKMSTDenseTests
    {
        [TestMethod()]
        public void HKMSTDenseAddEdgeTest()
        {/*
            var dense = new HKMSTDense(5);
            for (int i = 0; i < 5; i++)
                dense.AddVertex();*/
            /*
            var a1 = dense.AddEdge(3, 4);
            var a2 = dense.AddEdge(4, 1);
            var a3 = dense.AddEdge(1, 4);
            */


            /*
            var a1 = dense.AddEdge(2, 3);
            var a2 = dense.AddEdge(4, 3); 
            var a3 = dense.AddEdge(3, 0);
            var a4 = dense.AddEdge(1, 2);
            var a5 = dense.AddEdge(1, 4); 
            var a6 = dense.AddEdge(2, 4);
            var a7 = dense.AddEdge(0, 1);// cycle
            //*/

            //Assert.IsTrue(a1 && !a2 && a3 && a4 && !a5 && a6);
            Assert.IsTrue(false);
        }
    }
}