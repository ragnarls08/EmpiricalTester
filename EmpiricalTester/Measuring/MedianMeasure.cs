using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmpiricalTester.Algorithms;

namespace EmpiricalTester.Measuring
{
    public class MedianMeasure
    {
        public static string MeasureMedian(List<int> ns,  int numberMin, int numberMax, int repeatCount)
        {
            var random = new Random(DateTime.Now.Millisecond);
            var sw = new Stopwatch();
            var measurements = new Measurements(ns.Count);

            foreach (var n in ns)
            {
                var currentMeasurement = measurements.Add(n);
                for (var i = 0; i < repeatCount; i++)
                {
                    var l1 = new List<int>(n);
                    var middle = n/2;

                    for(var x = 0; x < n; x++)
                        l1.Add(random.Next(numberMin, numberMax));

                    var l2 = new List<int>(l1);
                    var l3 = new List<int>(l1);
                    
                    sw.Start();
                    var mom = l1.Mom(middle, Comparer<int>.Default);
                    sw.Stop();                    
                    var t1 = sw.Elapsed.Ticks;
                    sw.Reset();

                    sw.Start();
                    var momRandom = l2.MomRandom(middle, Comparer<int>.Default);
                    sw.Stop();
                    var t2 = sw.Elapsed.Ticks;
                    sw.Reset();
                    
                    sw.Start();
                    var quickSelect = l3.QuickSelect(middle, Comparer<int>.Default);
                    sw.Stop();
                    var t3 = sw.Elapsed.Ticks;
                    sw.Reset();

                    currentMeasurement.sum(t1, t2, t3);
                }
            }

            measurements.UpdateStats();

            return measurements.ToString();
        }

        private class Measurements
        {
            private List<Measurement> _measurements;
             
            public Measurements(int ns)
            {
                _measurements = new List<Measurement>(ns);
            }

            public Measurement Add(int n)
            {
                var m = new Measurement(n);
                _measurements.Add(m);
                return m;
            }

            public void UpdateStats()
            {
                _measurements.ForEach(i => i.CalculateAverages());
            }

            public override string ToString()
            {
                return _measurements.Aggregate("", (current, measurement) => current + measurement.ToString());
            }
        }

        private class Measurement
        {
            private int _n;
            private int _count;
            private long _sum1;
            private long _sum2;
            private long _sum3;

            private long Average1 { get; set; }
            private long Average2 { get; set; }
            private long Average3 { get; set; }

            public int Count() => _count;

            public Measurement(int n)
            {
                _n = n;
            }      

            public void sum(long a, long b, long c)
            {
                _sum1 += a;
                _sum2 += b;
                _sum3 += c;
                _count++;
            }

            public void CalculateAverages()
            {
                Average1 = _sum1/_count;
                Average2 = _sum2 / _count;
                Average3 = _sum3 / _count;
            }

            public override string ToString()
            {
                return
                    $"Number of Elements: {_n}\n" +
                    $"\tMoM:         {Average1}\n" +
                    $"\tMoMRandom:   {Average2}\n" +
                    $"\tQuickSelect: {Average3}\n";
            }
        }
    }
}
