using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.Utilities;

namespace PeraCore.Runtime {
	public static class CollectionUtil {
		public static bool IsNullOrEmpty<T>(this IList<T> list) {
			if (list == null) return true;
			if (list.Count == 0) return true;
			return false;
		}

		public static V GetOrDefault<K, V>(this Dictionary<K, V> map, K key, V def) {
			return map.ContainsKey(key) ? map[key] : def;
		}

		public static V GetOrPut<K, V>(this Dictionary<K, V> map, K key, V val) {
			if (!map.ContainsKey(key)) map[key] = val;
			return map[key];
		}

		public static T GetNullable<T>([ItemCanBeNull] this List<T> list, int index) {
			return list.IsValidIndex(index) ? list[index] : default;
		}

		public static T GetOrDefault<T>(this List<T> list, int index, T def) {
			return list.IsValidIndex(index) ? list[index] : def;
		}

		public static T GetOrPut<T>(this List<T> list, int index, T def) {
			if (list.IsValidIndex(index)) {
				return list[index];
			}
			list[index] = def;
			return def;
		}

		public static IEnumerable<TSource> Flatten<TSource>(this IEnumerable<IEnumerable<TSource>> source) {
			return source.SelectMany(ts => ts);
		}

		public static T RandomOrNull<T>(this IEnumerable<T> source) {
			var list = source.ToList();
			return list.IsNullOrEmpty() ? default : list[list.RandomIndex()];
		}

		public static T Random<T>(this IEnumerable<T> source) {
			var list = source.ToList();
			return list[list.RandomIndex()];
		}

		public static int RandomIndex<T>(this IEnumerable<T> source) {
			var list = source.ToList();
			return UnityEngine.Random.Range(0, list.Count);
		}


		public static bool IsValidIndex<T>(this IEnumerable<T> source, int index) {
			var list = source.ToList();
			return index >= 0 && list.Count > index && list.ToList()[index] != null;
		}
	}
}
