using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace ARDR {
	public static class CollectionUtil {
		public static V GetOrDefault<K, V>(this Dictionary<K, V> map, K key, V def = default) {
			return map.ContainsKey(key) ? map[key] : def;
		}

		public static U Get<T, U>(this Dictionary<T, U> dict, T key) {
			dict.TryGetValue(key, out var value);
			return value;
		}

		public static V GetOrPut<K, V>(this Dictionary<K, V> map, K key, V val) {
			if (!map.ContainsKey(key)) map[key] = val;
			return map[key];
		}


		public static T GetOrDefault<T>(this List<T> list, int index, T def) {
			return list.IsValidIndex(index) ? list[index] : def;
		}


		public static IEnumerable<TSource> Flatten<TSource>(this IEnumerable<IEnumerable<TSource>> source) {
			return source.SelectMany(ts => ts);
		}

		public static T Random<T>(this IEnumerable<T> source) {
			var list = source.ToList();
			return list[list.RandomIndex()];
		}

		public static int RandomIndex<T>(this IEnumerable<T> source) {
			var list = source.ToList();
			return UnityEngine.Random.Range(0, list.Count - 1);
		}


		public static bool IsValidIndex<T>(this IEnumerable<T> source, int index) {
			var list = source.ToList();
			return index >= 0 && list.Count > index && list.ToList()[index] != null;
		}

		public static IEnumerable<T> EnumValue<T>() {
			return (T[]) Enum.GetValues(typeof(T));
		}

		public static void Fill(this Array array, object value) {
			var indices = new int[array.Rank];
			Fill(array, 0, indices, value);
		}

		public static void Fill(Array array, int dimension, int[] indices, object value) {
			if (dimension < array.Rank) {
				for (var i = array.GetLowerBound(dimension); i <= array.GetUpperBound(dimension); i++) {
					indices[dimension] = i;
					Fill(array, dimension + 1, indices, value);
				}
			} else
				array.SetValue(value, indices);
		}

		public static KeyValuePair<TFrom, TTo> To<TFrom, TTo>(this TFrom from, TTo to) {
			return new KeyValuePair<TFrom, TTo>(from, to);
		}

		public static Vector2Int ToVector(this KeyValuePair<int, int> pair) {
			var (x, y) = pair;
			return new Vector2Int(x, y);
		}

		public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> source, out TKey Key,
			out TValue Value) {
			Key = source.Key;
			Value = source.Value;
		}

		public static void Deconstruct(this Vector2 source, out float x, out float y) {
			x = source.x;
			y = source.y;
		}

		public static void Deconstruct(this Vector2Int source, out int x, out int y) {
			x = source.x;
			y = source.y;
		}

		public static bool GetValue<T>(this T? nullable, out T value) where T : struct {
			value = nullable.GetValueOrDefault(default);
			return nullable.HasValue;
		}
	}

	public abstract class UnitySerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>,
		ISerializationCallbackReceiver {
		[SerializeField, HideInInspector]
		private List<TKey> keyData = new List<TKey>();

		[SerializeField, HideInInspector]
		private List<TValue> valueData = new List<TValue>();

		void ISerializationCallbackReceiver.OnAfterDeserialize() {
			Clear();
			for (var i = 0; i < keyData?.Count && i < valueData.Count; i++) {
				this[keyData[i]] = valueData[i];
			}
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize() {
			keyData?.Clear();
			valueData?.Clear();

			foreach (var item in this) {
				keyData?.Add(item.Key);
				valueData?.Add(item.Value);
			}
		}
	}

	[Serializable]
	public class DefaultDictionary<TKey, TValue> : UnitySerializedDictionary<TKey, TValue> {
		public TValue DefaultValue { get; }

		public DefaultDictionary() {
			DefaultValue = default;
		}

		public DefaultDictionary(TValue defaultValue) {
			DefaultValue = defaultValue;
		}

		public new TValue this[TKey key] {
			get => TryGetValue(key, out var t) ? t : DefaultValue;
			set => base[key] = value;
		}
	}

	public static class DefaultDictionaryUtil {
		public static DefaultDictionary<TKey, TValue> ToDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict,
			TValue defaultValue) {
			var result = new DefaultDictionary<TKey, TValue>(defaultValue);
			dict.ForEach(pair => result.Add(pair.Key, pair.Value));
			return result;
		}
	}
}
