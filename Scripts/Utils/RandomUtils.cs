﻿using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace UDBase.Utils {
	/// <summary>
	/// Set of random helper methods
	/// </summary>
	public static class RandomUtils {
		static int Range(int min, int max) => UnityEngine.Random.Range(min, max);

		/// <summary>
		/// Returns random value in [min, max] interval without exclusion items
		/// </summary>
		public static int RangeExcluded(int min, int max, int[] exclusions) {
			Assert.IsFalse(IsAllRangeExcluded(min, max, exclusions));
			var value = min;
			do {
				value = Range(min, max);
			} while ( IsValueContains(value, exclusions) );
			return value;
		}

		/// <summary>
		/// Returns random value from List collection
		/// </summary>
		public static T GetItem<T>(List<T> items) {
			if( (items != null) && (items.Count > 0) ) {
				return items[Range(0, items.Count)];
			}
			return default(T);
		}

		/// <summary>
		/// Returns random value from Array collection
		/// </summary>
		public static T GetItem<T>(T[] items) {
			if ( (items != null) && (items.Length > 0) ) {
				return items[Range(0, items.Length)];
			}
			return default(T);
		}

		/// <summary>
		/// Returns random value from Enum collection
		/// </summary>
		public static T GetEnumValue<T>() {
			Assert.IsTrue(typeof(T).IsEnum);

			var values = Enum.GetValues(typeof(T));
			Assert.IsTrue(values.Length > 0);

			var index = Range(0, values.Length);
			return (T)values.GetValue(index);
		}

		static bool IsValueContains(int value, int[] exclusions) {
			for ( var i = 0; i < exclusions.Length; i++ ) {
				if ( value == exclusions[i] ) {
					return true;
				}
			}
			return false;
		}

		static bool IsAllRangeExcluded(int min, int max, int[] exclusions) {
			for ( var i = min; i < max; i++ ) {
				if ( !IsValueContains(i, exclusions) ) {
					return false;
				}
			}
			return true;
		}
	}
}
