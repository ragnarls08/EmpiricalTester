using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmpiricalTester.Algorithms;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.Algorithms.Tests
{
    [TestClass()]
    public class MedianTests
    {
        [TestMethod()]
        public void MomTest()
        {
            Random r = new Random();
            List<int> x = new List<int>(5000);
            List<int> result = new List<int>(5000);
            int size = 5000;

            for (int i = 0; i < size; i++)
            {
                x.Add(r.Next(0, 100));
            }

            for (int i = 0; i < size; i++)
            {
                result.Add(x.Mom(i, Comparer<int>.Default));
            }

            x.Sort();
            for (int i = 0; i < size; i++)
            {
                if (result[i] != x[i])
                    Assert.Fail($"{x[i]} != {result[i]}");
            }

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void MomRandomTest()
        {
            Random r = new Random();
            List<int> x = new List<int>(5000);
            List<int> result = new List<int>(5000);
            int size = 5000;

            for (int i = 0; i < size; i++)
            {
                x.Add(r.Next(0, 100));
            }

            for (int i = 0; i < size; i++)
            {
                result.Add(x.MomRandom(i, Comparer<int>.Default));
            }

            x.Sort();
            for (int i = 0; i < size; i++)
            {
                if (result[i] != x[i])
                    Assert.Fail($"{x[i]} != {result[i]}");
            }

            Assert.IsTrue(true);
        }
    }
}