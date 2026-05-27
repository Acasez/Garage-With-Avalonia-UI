using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_Garage_Task
{
    public static class ListExtensions
    {
        public static string ToCustomString(this List<int> list)
        {
            return string.Join(", ", list);
        }
    }
    public static class IntExtensions
    {
        public static List<int> ToList(this int value)
        {
            return new List<int> { value };
        }
    }
}
