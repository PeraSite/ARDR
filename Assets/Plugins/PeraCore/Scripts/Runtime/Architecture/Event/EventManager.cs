using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PeraCore.Runtime {
	[ExecuteAlways]
	public static class EventManager {
		private static readonly Dictionary<Type, List<IEventListenerBase>> _subscribersList;

		static EventManager() {
			_subscribersList = new Dictionary<Type, List<IEventListenerBase>>();
		}

		public static void AddListener<TEvent>(IEventListener<TEvent> listener) where TEvent : struct {
			var eventType = typeof(TEvent);

			if (!_subscribersList.ContainsKey(eventType))
				_subscribersList[eventType] = new List<IEventListenerBase>();

			if (!SubscriptionExists(eventType, listener))
				_subscribersList[eventType].Add(listener);
		}

		public static void RemoveListener<TEvent>(IEventListener<TEvent> listener) where TEvent : struct {
			var eventType = typeof(TEvent);

			if (!_subscribersList.ContainsKey(eventType)) {
#if EVENTROUTER_THROWEXCEPTIONS
					throw new ArgumentException( string.Format( "Removing listener \"{0}\", but the event type \"{1}\" isn't registered.", listener, eventType.ToString() ) );
#else
				return;
#endif
			}

			var subscriberList = _subscribersList[eventType];

#if EVENTROUTER_THROWEXCEPTIONS
	            bool listenerFound = false;
#endif

			for (var i = 0; i < subscriberList.Count; i++) {
				if (subscriberList[i] == listener) {
					subscriberList.Remove(subscriberList[i]);
#if EVENTROUTER_THROWEXCEPTIONS
					    listenerFound = true;
#endif

					if (subscriberList.Count == 0) {
						_subscribersList.Remove(eventType);
					}

					return;
				}
			}

#if EVENTROUTER_THROWEXCEPTIONS
		        if( !listenerFound )
		        {
					throw new ArgumentException( string.Format( "Removing listener, but the supplied receiver isn't subscribed to event type \"{0}\".", eventType.ToString() ) );
		        }
#endif
		}

		public static void TriggerEvent<TEvent>(TEvent newEvent) where TEvent : struct {
			if (!_subscribersList.TryGetValue(typeof(TEvent), out var list))
#if EVENTROUTER_REQUIRELISTENER
			            throw new ArgumentException( string.Format( "Attempting to send event of type \"{0}\", but no listener for this type has been found. Make sure this.Subscribe<{0}>(EventRouter) has been called, or that all listeners to this event haven't been unsubscribed.", typeof( MMEvent ).ToString() ) );
#else
				return;
#endif

			for (var i = 0; i < list.Count; i++) {
				(list[i] as IEventListener<TEvent>).OnEvent(newEvent);
			}
		}

		private static bool SubscriptionExists(Type type, IEventListenerBase receiver) =>
			_subscribersList.TryGetValue(type, out var receivers) && receivers.Any(t => t == receiver);
	}

	public static class EventRegister {
		public static void StartListeningEvent<TEvent>(this IEventListener<TEvent> caller) where TEvent : struct {
			EventManager.AddListener(caller);
		}

		public static void StopListeningEvent<TEvent>(this IEventListener<TEvent> caller) where TEvent : struct {
			EventManager.RemoveListener(caller);
		}
	}
}
