using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace EmpiricalTester.Measuring
{
    class OrderMaintenance
    {
        public void runSequence(string outFile, List<int> ns, List<double> ps, List<double> alphas, int repeatCount)
        {
            Measurements m = new Measurements();
            Random random = new Random(DateTime.Now.Millisecond);
            Stopwatch sw = new Stopwatch();

            foreach (double alpha in alphas)
            {
                foreach (int n in ns)
                {
                    for (int x = 0; x < repeatCount; x++)
                    {
                        Console.WriteLine(alpha + "a      " + 0 + "p       " + n);
                        // create or reset stuff
                        var sgt = new DataStructures.SGTree<int>(alpha);
                        var sgtNodes = new List<DataStructures.SGTNode<int>>(n);
                        
                        for(int i = 1; i < n; i++)
                        {
                            sgtNodes.Add(new DataStructures.SGTNode<int>(i));
                        }
                        sw.Reset();
                        sw.Start();
                        var prev = sgt.insertFirst(0);



                        //sgt.insertAllAfter(prev, sgtNodes);

                        sw.Stop();
                        m.add(n, 0, alpha, sw.ElapsedTicks);
                    }                    
                }
            }

            m.writeToFile(outFile);
        }

        public void run(string outFile, List<int> ns, List<double> ps, List<double> alphas, int repeatCount)
        {
            Measurements m = new Measurements();
            Random random = new Random(DateTime.Now.Millisecond);
            Stopwatch sw = new Stopwatch();

            foreach (double alpha in alphas)
            {
                foreach(double p in ps)
                {
                    foreach (int n in ns) 
                    {
                        for(int x = 0; x < repeatCount; x++)
                        {
                            Console.WriteLine(alpha + "a      " + p + "p       " + n);
                            // create or reset stuff
                            var sgt = new DataStructures.SGTree<int>(alpha);
                            var sgtNodes = new List<DataStructures.SGTNode<int>>(n);
                            sw.Reset();
                            sw.Start();
                            var prev = sgt.insertFirst(0);
                            sgtNodes.Add(prev);
                            for (int i = 0; i < n; i++)
                            {
                                // sequential probability 
                                if (random.Next(0, 100) / 100.0 < p)
                                {
                                    prev = sgt.insertAfter(prev, i); 
                                    sgtNodes.Add(prev);
                                }
                                else
                                {
                                    int at = random.Next(0, i);
                                    prev = sgt.insertAfter(sgtNodes[at], i);
                                    sgtNodes.Add(prev);
                                }
                            }                            
                            sw.Stop();
                            m.add(n, p, alpha, sw.ElapsedTicks);                            
                        }                        
                    }
                }
            }

            m.writeToFile(outFile);
        }

        private class Measurements
        {
            private List<Measurement> items;

            public Measurements()
            {
                items = new List<Measurement>();
            }

            public void add(int n, double p, double a, long ticks)
            {
                var m = items.Find(item => item.A == a && item.P == p);
                if (m == null)
                {
                    items.Add(new Measurement(n, p, a, ticks));
                }
                else
                    m.addTicks(n, ticks);
            }

            public void writeToFile(string outFile)
            {
                var lines = new List<string>();
                string xLabels = "\t" + items[0].N.ConvertAll(item => item.ToString()).Aggregate((a, b) => a + ("\t" + b));
                lines.Add(xLabels);

                foreach(var curr in items)
                {
                    lines.Add(
                        curr.A.ToString() + "a / " + curr.P.ToString() + "p" +
                        "\t" + curr.SumTicks.ConvertAll(item => (item / ((double)curr.SumCount[0])).ToString()).Aggregate((a,b) => a + "\t" + b)
                        );
                }

                File.WriteAllLines(outFile, lines);
            }

            private class Measurement
            {
                public Measurement(int n, double p, double a, long ticks)
                {
                    N = new List<int>() { n };
                    P = p;
                    A = a;
                    SumTicks = new List<long>() { ticks };
                    SumCount = new List<int>() { 0 };
                }

                public void addTicks(int n, long ticks)
                {
                    var index = N.FindIndex(item => item == n);
                    if (index == -1)
                    {
                        N.Add(n);                        
                        SumTicks.Add(ticks);
                        SumCount.Add(0);
                    }
                    else
                    {
                        SumTicks[index] += ticks;
                        SumCount[index]++;
                    }
                              
                }

                public List<int> N { get; set; }
                public double P { get; set; }
                public double A { get; set; }
                public List<long> SumTicks { get; set; }
                public List<int> SumCount { get; set; }
            }
        }
    }
}
