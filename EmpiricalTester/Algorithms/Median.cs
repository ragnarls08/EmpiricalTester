using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpiricalTester.Algorithms
{
    public static class Median
    {
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
