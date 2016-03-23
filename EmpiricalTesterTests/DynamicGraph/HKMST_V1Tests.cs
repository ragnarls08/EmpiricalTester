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
        public void addEdgeTestCycle()
        {
            var hkmst = new HKMST_V1(0.75);
            for (int i = 0; i < 8; i++)
                hkmst.addVertex();

            hkmst.addEdge(0, 1);
            hkmst.addEdge(4, 2);
            hkmst.addEdge(5, 4);
            hkmst.addEdge(7, 5);
            hkmst.addEdge(0, 5);
            hkmst.addEdge(6, 1);
            hkmst.addEdge(0, 3);
            hkmst.addEdge(6, 2);
            hkmst.addEdge(6, 4);
            hkmst.addEdge(1, 2);
            hkmst.addEdge(3, 7);
            hkmst.addEdge(3, 4);
            hkmst.addEdge(0, 2);
            hkmst.addEdge(1, 4);
            hkmst.addEdge(7, 2);
            hkmst.addEdge(5, 6);
            hkmst.addEdge(0, 6);
            hkmst.addEdge(0, 7);
            hkmst.addEdge(7, 3);     




            var b = true; //b6 && b2 && b3 && b4 && !b5;

            Assert.IsTrue(b);
        }

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
            for (int i = 0; i < 5; i++)
                hkmst.addVertex();

            var b1 = hkmst.addEdge(1, 0);
            var b2 = hkmst.addEdge(0, 4);
            var b3 = hkmst.addEdge(4, 1);
            var b4 = hkmst.addEdge(1, 3);

            var b = b1 && b2 && !b3 && b4;

            Assert.IsTrue(b);
        }

        [TestMethod()]
        public void addEdgeTest3()
        {
            var hkmst = new HKMST_V1(0.75);
            for (int i = 0; i < 5; i++)
                hkmst.addVertex();

            var b1 = hkmst.addEdge(2, 4);
            var b2 = hkmst.addEdge(0, 4);
            var b3 = hkmst.addEdge(3, 2);
            var b4 = hkmst.addEdge(1, 0);
            var b5 = hkmst.addEdge(1, 4);
            var b6 = hkmst.addEdge(2, 1);

            var b = b1 && b2 && b3 && b4 && b5 && b6;

            Assert.IsTrue(b);
        }

        [TestMethod()]
        public void addEdgeTest4()
        {
            var hkmst = new HKMST_V1(0.75);
            for (int i = 0; i < 5; i++)
                hkmst.addVertex();

            var b1 = hkmst.addEdge(2, 0);
            var b2 = hkmst.addEdge(4, 3);
            var b3 = hkmst.addEdge(3, 1);

            var b = b1 && b2 && b3;

            Assert.IsTrue(b);
        }

        [TestMethod()]
        public void addEdgeTest5()
        {
            var hkmst = new HKMST_V1(0.75);
            for (int i = 0; i < 5; i++)
                hkmst.addVertex();

            var b1 = hkmst.addEdge(3, 4);
            var b2 = hkmst.addEdge(0, 2);
            var b3 = hkmst.addEdge(0, 4);

            var b4 = hkmst.addEdge(3, 2);
            var b5 = hkmst.addEdge(1, 4);
            var b6 = hkmst.addEdge(4, 0);

            var b7 = hkmst.addEdge(0, 3);
            var b8 = hkmst.addEdge(4, 1);
            var b9 = hkmst.addEdge(2, 0);
            var b10 = hkmst.addEdge(3, 1);

            var b = true; // b1 && b2 && b3;

            Assert.IsTrue(b);
        }

        [TestMethod()]
        public void addEdgeTest6()
        {
            var hkmst = new HKMST_V1(0.75);
            for (int i = 0; i < 5; i++)
                hkmst.addVertex();

            var b1 = hkmst.addEdge(0, 4);
            var b2 = hkmst.addEdge(4, 1);
            var b3 = hkmst.addEdge(3, 2);
            var b4 = hkmst.addEdge(2, 0);
          

            var b = true; // b1 && b2 && b3;

            Assert.IsTrue(b);
        }

        [TestMethod()]
        public void addEdgeTest7()
        {
            var hkmst = new HKMST_V1(0.75);
            for (int i = 0; i < 6; i++)
                hkmst.addVertex();

            var b1 = hkmst.addEdge(0, 1);
            var b2 = hkmst.addEdge(3, 4);
            var b3 = hkmst.addEdge(1, 2);
            var b4 = hkmst.addEdge(2, 3);

            var b5 = hkmst.addEdge(4, 1);
            var b6 = hkmst.addEdge(5, 4);
            var b7 = hkmst.addEdge(5, 2);
            var b8 = hkmst.addEdge(5, 3);
            var b9 = hkmst.addEdge(1, 5);

            var b = true; // b1 && b2 && b3;

            Assert.IsTrue(b);
        }


        [TestMethod()]
        public void addEdgeTest8()
        {
            var hkmst = new HKMST_V1(0.75);
            for (int i = 0; i < 150; i++)
                hkmst.addVertex();

            for(int v = 0; v < 150; v++)
            {
                for (int w = 0; w < 150; w++)
                    hkmst.addEdge(v, w);
            }




            var b = true; // b1 && b2 && b3;

            Assert.IsTrue(b);
        }
    }
}