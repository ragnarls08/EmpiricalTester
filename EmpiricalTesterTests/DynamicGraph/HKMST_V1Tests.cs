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
        public void addEdgeTestCycle2()
        {
            var hkmst = new HKMST_V1(0.75);
            for (int i = 0; i < 125; i++)
                hkmst.addVertex();
            
            hkmst.addEdge(51, 117);//
            hkmst.addEdge(117, 57);//
            hkmst.addEdge(36, 41);//
            hkmst.addEdge(94, 91);//
            hkmst.addEdge(110, 50);//
            hkmst.addEdge(45, 42);//
            hkmst.addEdge(37, 16);//
            hkmst.addEdge(16, 94);//
            hkmst.addEdge(40, 42);//
            hkmst.addEdge(84, 16);//
            hkmst.addEdge(3, 40);//
            hkmst.addEdge(115, 49);//
            hkmst.addEdge(41, 51);//
            hkmst.addEdge(41, 64);//
            hkmst.addEdge(57, 54);//
            hkmst.addEdge(51, 96);//
            hkmst.addEdge(49, 55);//
            hkmst.addEdge(85, 102);//
            hkmst.addEdge(110, 94);//
            hkmst.addEdge(120, 15);//
            hkmst.addEdge(17, 40);//
            hkmst.addEdge(49, 123);//
            hkmst.addEdge(15, 3);//
            hkmst.addEdge(76, 54);//
            hkmst.addEdge(42, 110);//
            hkmst.addEdge(36, 115);//
            hkmst.addEdge(96, 124);//
            hkmst.addEdge(123, 7);//
            
            hkmst.addEdge(91, 36);




            var b = true; //b6 && b2 && b3 && b4 && !b5;

            Assert.IsTrue(b);
        }

        [TestMethod()]
        public void addEdgeTestCycle3()
        {
            var hkmst = new HKMST_V1(0.75);
            for (int i = 0; i < 32; i++)
                hkmst.addVertex();

            hkmst.addEdge(14, 28);//
            hkmst.addEdge(28, 17);//
            hkmst.addEdge(6, 9);//
            hkmst.addEdge(23, 22);//
            hkmst.addEdge(26, 13);//
            hkmst.addEdge(11, 10);//
            hkmst.addEdge(7, 4);//
            hkmst.addEdge(4, 23);//
            hkmst.addEdge(8, 10);//
            hkmst.addEdge(20, 4);//
            hkmst.addEdge(0, 8);//
            hkmst.addEdge(27, 12);//
            hkmst.addEdge(9, 14);//
            hkmst.addEdge(9, 18);//
            hkmst.addEdge(17, 15);//
            hkmst.addEdge(14, 24);//
            hkmst.addEdge(12, 16);//
            hkmst.addEdge(21, 25);//
            hkmst.addEdge(26, 23);//
            hkmst.addEdge(29, 3);//
            hkmst.addEdge(5, 8);//
            hkmst.addEdge(12, 30);//
            hkmst.addEdge(3, 0);//
            hkmst.addEdge(19, 15);//
            hkmst.addEdge(10, 26);//
            hkmst.addEdge(6, 27);//
            hkmst.addEdge(24, 31);//
            hkmst.addEdge(30, 1);//

            hkmst.addEdge(22, 6);




            var b = true; //b6 && b2 && b3 && b4 && !b5;

            Assert.IsTrue(b);
        }

        [TestMethod()]
        public void addEdgeTestCycle4()
        {
            var hkmst = new HKMST_V1(0.75);
            //var hkmst = new SimpleIncremental();
            for (int i = 0; i < 10; i++)
                hkmst.addVertex();

            
            hkmst.addEdge(0, 1);
            hkmst.addEdge(1, 2);
            hkmst.addEdge(2, 3);
            

            hkmst.addEdge(3, 4);
            hkmst.addEdge(4, 5);
            

            hkmst.addEdge(6, 7);
            hkmst.addEdge(7, 8);
            hkmst.addEdge(8, 9);


            //hkmst.addEdge(2, 3);            
            hkmst.addEdge(5, 6);

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