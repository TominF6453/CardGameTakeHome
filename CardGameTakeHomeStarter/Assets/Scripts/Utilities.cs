using System.Collections.Generic;
using UnityEngine;

namespace CardGame {
    /// <summary>
    /// A selection of utility methods and extensions that may be useful
    /// for more than one object.
    /// </summary>
    public static class Utilities {
		/// <summary>
		/// Shuffles the element order of the specified list.
		/// 
		/// Uses a standard Fisher-Yates shuffle. Loop through each item, swap its
		/// position with another random item.
		/// </summary>
		public static void Shuffle<T>( this IList<T> list ) {
			var count = list.Count;
			for ( int i = 0; i < count - 1; ++i ) {
				var r = UnityEngine.Random.Range(i, count);
				(list[i], list[r]) = (list[r], list[i]);
			}
		}

		/// <summary>
		/// Shuffles the element order of the specified list.
		/// 
		/// Uses a standard Fisher-Yates shuffle. Loop through each item, swap its
		/// position with another random item.
		/// </summary>
		/// <param name="loops">The number of shuffle iterations to perform.</param>
		public static void Shuffle<T>( this IList<T> list, int loops = 1 ) {
			for ( int i = 0; i < loops; i++ ) Shuffle( list );
		}

		/// <summary>
		/// Pop method for Lists, returns the "top" value (last) and removes it
		/// from the list.
		/// </summary>
		/// <returns>The last element of the list.</returns>
		public static T Pop<T>( this IList<T> list ) {
			T obj = list[^1];
			list.RemoveAt( list.Count - 1 );
			return obj;
		}
	}
}