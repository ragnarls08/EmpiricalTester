using System;
using System.Collections.Generic;
using System.Linq;

namespace EmpiricalTester.Algorithms
{
    public static class Median
    {
        public static T Mom<T>(this IList<T> list, int i, Comparison<T> comp) where T : IComparable<T>
        {
            
            if (list.Count <= 2)
                return list.First();
            
            return Select(list.ToArray(), i, 0, list.Count - 1, comp);
        }

        public static T Select<T>(this T[] list, int i, int start, int end, Comparison<T> comp) where T : IComparable<T>
        {
            if (start == end)
                return list[start];
            
            int swap_index = start;
            for (int x = start; x <= end; x += 5)
            {
                if (end >= x + 4)
                {
                    Array.Sort(list, x, 5);
                    Swap(list, swap_index, x + 2);
                    swap_index++;
                }
            }
            int remainder = ((end + 1) - start) % 5;
            Array.Sort(list, end - remainder, remainder + 1);
            Swap(list, swap_index, end - (remainder / 2));
            swap_index++; // ?

            T median = Select(list, i, start, swap_index, comp);
            int r = list.Partition(comp, start, end);
            int k = r - start + 1;

            if (i == k)
                return list[i];
            else if (i < k)
                return Select(list, i, start, r - 1, comp);
            else
                return Select(list, i, r - 1, end, comp);

        }
        /*
        public static T Select<T>(this T[] list, int i, int start, int end) where T : IComparable<T>
        {
            if (end - start <= 5)
            {
                Array.Sort(list, start, end - start);
                return list[i];
            }

            int swap_index = start;
            for (int x = start; x <= end; x += 5)
            {
                if (end >= x + 4)
                {
                    Array.Sort(list, x, 5);
                    Swap(list, swap_index, x + 2);
                    swap_index++;
                }
            }

            int newN = start + ((swap_index - start) / 2);
            T median = Select(list, newN, start, swap_index - 1); // finds the median of medians

            int countL = 0;
            int countS = 0;
            for(int x = start; x <= end; x++)
            {
                int c = median.CompareTo(list[x]);
                if (c < 0)
                    countL++;
                if (c > 0)
                    countS++;
            }

            T[] smaller = new T[countS];
            T[] bigger = new T[countL];
            countL = 0;
            countS = 0;
            for(int x = start; x <= end; x++)
            {
                int c = median.CompareTo(list[x]);
                if (c < 0)
                    bigger[countL++] = list[x];                                    
                if (c > 0)
                    smaller[countS++] = list[x];
            }

            //list = new T[countS + countL + 1];
            //smaller.CopyTo(list, 0);
            //list[countS] = median;
            //bigger.CopyTo(list, countS + 1);


            if (i <= countL)
                return Select(bigger, i, 0, countL-1);
            else if (i - 1 == countL)
                return median;
            else
                return Select(smaller, i - countL - 1, 0, countS-1);

        }
        */

            /*
            public static T Select<T>(this T[] list, int i, int start, int end, Comparison<T> comp) where T : IComparable<T>
            {
                if(end - start <= 5)
                {
                    Array.Sort(list, start, end - start);
                    return list[i];                
                }

                int swap_index = start;
                for(int x = start; x < end; x+=5)
                {
                    if(end >= x+4)
                    {
                        Array.Sort(list, x, 5);
                        Swap(list, swap_index, x + 2);
                        swap_index++;
                    }                
                }

                int newN = start + ((swap_index - start) / 2);
                T median = Select(list, newN, start, swap_index -1, comp); // finds the median of medians

                int location = pivot(list, median, start, end, comp);

                if (location == i)
                    return list[i];
                else if (i > newN)
                    return Select(list, i, newN + 1, end, comp);
                else
                    return Select(list, i, start, newN, comp);

            }

            private static int pivot<T>(this T[] list, T value, int start, int end, Comparison<T> comp) where T : IComparable<T>
            {
                int tPos = start;
                int tCurr = 0;
                for(int i = start; i <= end; i++)
                {
                    if (list[i].CompareTo(value) < 0)
                        tPos++;
                    if (value.CompareTo(list[i]) == 0)
                        tCurr = i;
                }
                tPos--;

                int swapPos = tPos + 1;
                Swap(list, tPos, tCurr); // move t to its final position
                while (swapPos <= end && start <= end)
                {
                    if(value.CompareTo(list[start]) <= 0)
                    {
                        while (swapPos <= end && value.CompareTo(list[swapPos]) < 0)
                            swapPos++;
                        if(swapPos <= end)
                        {
                            Swap(list, start, swapPos);
                            swapPos++;
                        }                    
                    }

                    start++;
                }

                return tPos;
            }          */

        private static void Swap<T>(T[] list, int a, int b)
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
