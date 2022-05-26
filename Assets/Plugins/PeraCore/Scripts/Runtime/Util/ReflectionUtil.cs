using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PeraCore.Runtime {
	public static class ReflectionUtil {
		public static IEnumerable<Type> GetAllTypeImplements<T>() {
			return System.Reflection.Assembly.GetExecutingAssembly()
				.GetTypes()
				.Where(type => typeof(T).IsAssignableFrom(type) && !type.IsAbstract);
		}

		public static IEnumerable<Type> GetAllTypeImplements(Type targetType) {
			return System.Reflection.Assembly.GetExecutingAssembly()
				.GetTypes()
				.Where(type => targetType.IsAssignableFrom(type) && !type.IsAbstract);
		}

		public static IEnumerable<Type> FilterType(object value) {
			if (value == null) return new List<Type>();
			var valueType = value.GetType();
			var genericArguments = valueType.GetGenericArguments();

			switch (value) {
				default: {
					var baseType = valueType.BaseType;
					return GetAllTypeImplements(baseType);
				}

				case IList _:
				case IDictionary _: {
					return genericArguments.SelectMany(GetAllTypeImplements);
				}
			}
		}
	}
}
