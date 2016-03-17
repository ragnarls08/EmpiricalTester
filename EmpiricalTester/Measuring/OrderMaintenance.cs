using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.Measuring
{
    class OrderMaintenance
    {
        public void run(int n, double p, List<double> alphas)
        {
            double alphaMin = 0.75;
            Random random = new Random(DateTime.Now.Millisecond);
            Stopwatch sw = new Stopwatch();

            var sgt = new DataStructures.SGTree<int>(alphaMin);
            var list = new List<int>(n);
            var listNodes = new List<LinkedListNode<int>>(n);
            var sgtNodes = new List<DataStructures.SGTNode<int>>(n);
            
            

            sw.Start();
            var prev = sgt.insertFirst(0);
            sgtNodes.Add(prev);
            for(int i = 1; i < n; i++)
            {
                // sequential probability 
                if(random.Next(0, 100) / 100.0 < p)
                {
                    prev = sgt.insert(prev, i);
                    sgtNodes.Add(prev);
                }
                else
                {
                    int at = random.Next(0, i);
                    prev = sgt.insert(sgtNodes[at], i);
                    sgtNodes.Add(prev);
                }
            }
            sw.Stop();

            var sgtElapsed = sw.Elapsed;
            sw.Reset();
            

            sw.Start();
            //var lprev = list.AddFirst(0);
            //listNodes.Add(lprev);
            for (int i = 0; i < n; i++)
            {
                // sequential probability 
                if (random.Next(0, 100) / 100.0 < p)
                {
                    list.Add(i);
                    //lprev = list.AddAfter(lprev, i);
                    //listNodes.Add(lprev);
                }
                else
                {
                    int at = random.Next(0, i);
                    //lprev = list.AddAfter(listNodes[at], i);
                    //listNodes.Add(lprev);
                    list.Insert(at, i);
                }
            }
            sw.Stop();

            var listElapsed = sw.Elapsed;

            Console.WriteLine(sgtElapsed.Ticks);
            Console.WriteLine(listElapsed.Ticks);
        }
    }
}
