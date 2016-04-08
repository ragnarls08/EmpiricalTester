using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static System.Console;

namespace EmpiricalTester.Measuring
{
    internal class OrderMaintenance
    {
        public void RunSequence(string outFile, List<int> ns, List<double> ps, List<double> alphas, int repeatCount)
        {
            var m = new Measurements();
            var sw = new Stopwatch();

            foreach (var alpha in alphas)
            {
                foreach (var n in ns)
                {
                    for (var x = 0; x < repeatCount; x++)
                    {
                        WriteLine(alpha + "a      " + 0 + "p       " + n);                       
                        // create or reset stuff
                        var sgt = new DataStructures.SGTree<int>(alpha);
                        var SGTNodes = new List<DataStructures.SGTNode<int>>(n);
                        
                        for(var i = 1; i < n; i++)
                        {
                            SGTNodes.Add(new DataStructures.SGTNode<int>(i));
                        }
                        sw.Reset();
                        sw.Start();
                        sgt.insertFirst(0);                        
                        sw.Stop();
                        m.Add(n, 0, alpha, sw.ElapsedTicks);
                    }                    
                }
            }

            m.WriteToFile(outFile);
        }

        public void Run(string outFile, List<int> ns, List<double> ps, List<double> alphas, int repeatCount)
        {
            var m = new Measurements();
            var random = new Random(DateTime.Now.Millisecond);
            var sw = new Stopwatch();

            foreach (var alpha in alphas)
            {
                foreach(var p in ps)
                {
                    foreach (var n in ns) 
                    {
                        for(var x = 0; x < repeatCount; x++)
                        {
                            WriteLine(alpha + "a      " + p + "p       " + n);
                            // create or reset stuff
                            var sgt = new DataStructures.SGTree<int>(alpha);
                            var SGTNodes = new List<DataStructures.SGTNode<int>>(n);
                            sw.Reset();
                            sw.Start();
                            var prev = sgt.insertFirst(0);
                            SGTNodes.Add(prev);
                            for (var i = 0; i < n; i++)
                            {
                                // sequential probability 
                                if (random.Next(0, 100) / 100.0 < p)
                                {
                                    prev = sgt.insertAfter(prev, i); 
                                    SGTNodes.Add(prev);
                                }
                                else
                                {
                                    var at = random.Next(0, i);
                                    prev = sgt.insertAfter(SGTNodes[at], i);
                                    SGTNodes.Add(prev);
                                }
                            }                            
                            sw.Stop();
                            m.Add(n, p, alpha, sw.ElapsedTicks);                            
                        }                        
                    }
                }
            }

            m.WriteToFile(outFile);
        }

        private class Measurements
        {
            private List<Measurement> _items;

            public Measurements()
            {
                _items = new List<Measurement>();
            }

            public void Add(int n, double p, double a, long ticks)
            {
                var m = _items.Find(item => item.A == a && item.P == p);
                if (m == null)
                {
                    _items.Add(new Measurement(n, p, a, ticks));
                }
                else
                    m.AddTicks(n, ticks);
            }

            public void WriteToFile(string outFile)
            {
                var lines = new List<string>();
                var xLabels = "\t" + _items[0].N.ConvertAll(item => item.ToString()).Aggregate((a, b) => a + ("\t" + b));
                lines.Add(xLabels);

                foreach(var curr in _items)
                {
                    lines.Add(
                        curr.A + "a / " + curr.P + "p" +
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

                public void AddTicks(int n, long ticks)
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

                public List<int> N { get; }
                public double P { get; }
                public double A { get; }
                public List<long> SumTicks { get; }
                public List<int> SumCount { get; }
            }
        }
    }
}
