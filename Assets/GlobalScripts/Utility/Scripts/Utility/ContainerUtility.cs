namespace Utilities
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Assertions;

    public static partial class UnityExtensionUtility
    {
        #region array methods

        /// <summary>
        /// An array method to append another subarray. Note that it's legal to ask for more
        /// than the "from" array contains, in which case you'll receive less.
        /// </summary>
        public static T[] AppendArray<T>(this T[] array, T[] from, int start, int length)
        {
            Assert.IsTrue(start >= 0 && length >= 0, "we're taking signed int arguments because the system classes do the same");
            if (null == from || from.Length <= start || length == 0)
            {
                return array;
            }

            int appendLength = Mathf.Min(length, from.Length - start);
            if (appendLength <= 0)
            {
                return array;
            }

            int originalLength = array.Length;
            System.Array.Resize(ref array, originalLength + appendLength);
            System.Array.Copy(from, start, array, originalLength, appendLength);
            return array;
        }
        public static T[] AppendArray<T>(this T[] array, T[] from)
        {
            return AppendArray(array, from, 0, from.Length);
        }

        /// <summary>
        /// An array method to return a subarray. Note that it's legal to ask for more
        /// than the array contains, in which case you'll receive a shorter result.
        /// If the subarray is the full array you'll receive a reference to the original.
        /// </summary>
        public static T[] Subarray<T>(this T[] array, int start, int length)
        {
            Assert.IsTrue(start >= 0 && length >= 0, "we're taking signed int arguments because the system classes do the same");
            if (0 == start && array.Length <= length)
            {
                return array; // really okay to share in this circumstance?
            }
            int newLength = Mathf.Min(length, array.Length - start);
            var result = new T[newLength];
            if (newLength > 0)
            {
                System.Array.Copy(array, start, result, 0, newLength);
            }
            return result;
        }

        /// <summary>
        /// An array method to copy a subarray. Note that it's legal to ask for more
        /// than the array contains, in which case you'll receive a shorter result.
        /// </summary>
        public static T[] CopyArray<T>(this T[] array, int start, int length = int.MaxValue)
        {
            Assert.IsTrue(start >= 0 && length >= 0, "we're taking signed int arguments because the system classes do the same");
            int newLength = Mathf.Min(length, array.Length - start);
            var result = new T[newLength];
            if (newLength > 0)
            {
                System.Array.Copy(array, start, result, 0, newLength);
            }
            return result;
        }
        public static T[] CopyArray<T>(this T[] array)
        {
            return CopyArray(array, 0, array.Length);
        }

        #endregion
    }
}