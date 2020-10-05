namespace Common.ArrayList
{
    using System.Collections.Generic;
    public static class ExGenericLenght
    {
        public static bool CheckOutOfRange<T>(this List<T> list, int index) => (index >= 0) && (index < list.Count);
        public static bool CheckOutOfRange<T>(this T[] list, int index) => (index >= 0) && (index < list.Length);
    }
}
