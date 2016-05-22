using System;
using System.Collections.Generic;
using System.Linq;

namespace EmpiricalTester.Algorithms
{
    public static class Median
    {
        private static Random _random = new Random(DateTime.Now.Millisecond);

        public static T Mom<T>(this List<T> list, int i, IComparer<T> comp) 
        {
            return Select(list, i, 0, list.Count - 1, false, comp);
        }

        public static T MomRandom<T>(this List<T> list, int i, IComparer<T> comp) 
        {
            return Select(list, i, 0, list.Count - 1, true, comp);
        }

        


        public static T Select<T>(List<T> list, int i, int left, int right, bool isRandom, IComparer<T> comp) 
        {
            if ((right - left) < 5)
            {
                list.Sort(left, right-left+1, comp);
                return list[i];
            }
                
            if (left == right)
                return list[left];

            int swapIndex = left;
            for (var x = left; x <= right; x+=5)
            {
                list.Sort(x, x + 4 > right ? right - x + 1 : 5, comp);
                Swap(list, swapIndex++, 
                    x + 4 > right ? x + (right - x) / 2 : x + 2);
            }

            if(isRandom)
                Swap(list, left, _random.Next(left, swapIndex -1));
            else
            {
                T medianOfMedians = Select<T>(list, left + ((swapIndex - left - 1) / 2), left, swapIndex - 1, isRandom, comp);
                Swap(list, left, list.IndexOf(medianOfMedians));
            }

            var r = list.Partition(comp.Compare, left, right);
 
            if (i == r)
                return list[i];
            if (i < r)
                return Select(list, i, left, r - 1, isRandom, comp);

            return Select(list, i, r, right, isRandom, comp);
        }
        
        private static void Swap<T>(T[] list, int a, int b)
        {
            T t = list[a];
            list[a] = list[b];
            list[b] = t;
        }

        private static void Swap<T>(IList<T> list, int a, int b)
        {
            T t = list[a];
            list[a] = list[b];
            list[b] = t;
        }

        // @ http://dpatrickcaldwell.blogspot.se/2009/03/more-ilist-extension-methods.html
        public static int Partition<T>(this IList<T> list, Comparison<T> comparison, int left, int right)
        {
            int i = left;
            int j = right;

            // pick the pivot point and save it  
            T pivot = list[left];

            // until the indices cross  
            while (i < j)
            {
                // move the right pointer left until value < pivot  
                while (comparison(list[j], pivot) > 0 && i < j) j--;

                // move the right value to the left position  
                // increment left pointer  
                if (i != j) list[i++] = list[j];

                // move the left pointer to the right until value > pivot  
                while (comparison(list[i], pivot) < 0 && i < j) i++;

                // move the left value to the right position  
                // decrement right pointer  
                if (i != j) list[j--] = list[i];
            }

            // put the pivot holder in the left spot  
            list[i] = pivot;

            // return pivot location  
            return i;
        }

        public static int Partition<T>(this IList<T> list, Comparison<T> comparison)
        {
            return list.Partition(comparison, 0, list.Count - 1);
        }

        public static int Partition<T>(this IList<T> list, IComparer<T> comparer)
        {
            return list.Partition(new Comparison<T>((x, y) => comparer.Compare(x, y)));
        }

        public static int Partition<T>(this IList<T> list)
            where T : IComparable<T>
        {
            return list.Partition(new Comparison<T>((x, y) => x.CompareTo(y)));
        }

        public static T QuickSelect<T>(this IList<T> list, int k, Comparison<T> comparison, int left, int right)
        {
            // get pivot position  
            int pivot = list.Partition(comparison, left, right);

            // if pivot is less that k, select from the right part  
            if (pivot < k) return list.QuickSelect(k, comparison, pivot + 1, right);

            // if pivot is greater than k, select from the left side  
            else if (pivot > k) return list.QuickSelect(k, comparison, left, pivot - 1);

            // if equal, return the value  
            else return list[pivot];
        }

        public static T QuickSelect<T>(this IList<T> list, int k, Comparison<T> comparison)
        {
            return list.QuickSelect(k, comparison, 0, list.Count - 1);
        }

        public static T QuickSelect<T>(this IList<T> list, int k, IComparer<T> comparer)
        {
            return list.QuickSelect(k, new Comparison<T>((x, y) => comparer.Compare(x, y)));
        }

        public static T QuickSelect<T>(this IList<T> list, int k)
            where T : IComparable<T>
        {
            return list.QuickSelect(k, new Comparison<T>((x, y) => x.CompareTo(y)));
        }
    }
}
