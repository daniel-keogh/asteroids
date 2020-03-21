using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class ListUtils
    {
        public static Stack<T> CreateShuffledStack<T>(IList<T> values) where T : Object
        {
            Stack<T> stack = new Stack<T>();
            List<T> list = new List<T>(values);

            while (list.Count > 0)
            {
                var rIndex = UnityEngine.Random.Range(0, list.Count);
                var sp = list[rIndex];
                stack.Push(sp);
                list.RemoveAt(rIndex);
            }

            return stack;
        }
    }
}
