using System;
using System.Collections.Generic;
using UnityEngine;
using EventCollection = System.Collections.Generic.Dictionary<System.Type, System.Collections.Generic.List<EventListener<object>>>;

public class EventListener<T>
{
	public GameObject target;
	public Action<T> callback;
}

public class Events : Singleton<Events>
{
	private EventCollection eventCollection = new EventCollection();

	public static void Suscribe(GameObject suscriberObject, Type messageType, Action<object> action)
	{
		if (!Instance.eventCollection.ContainsKey(messageType))
		{
			var newEventListenerList = new List<EventListener<object>>();
			newEventListenerList.Add(new EventListener<object>() { callback = action, target = suscriberObject });
			Instance.eventCollection.Add(messageType, newEventListenerList);
		}
		else
		{
			var currentActionList = Instance.eventCollection[messageType];
			currentActionList.Add(new EventListener<object>() { callback = action, target = suscriberObject });
		}
	}

	public static void Trigger(Type messageType, object eventData)
	{
		Debug.Log("Event:" + messageType.ToString());

		if (Instance.eventCollection.ContainsKey(messageType))
		{
			var currentActionList = Instance.eventCollection[messageType];

			EventListener<object> markedToRemove = null;

			for (int i = 0; i < currentActionList.Count; i++)
			{
				var eventTarget = currentActionList[i];

				if (eventTarget.target != null)
				{
					eventTarget.callback(eventData);
				}
				else
				{
					markedToRemove = eventTarget;
				}
			}

			if (markedToRemove != null)
			{
				currentActionList.Remove(markedToRemove);
			}
		}
	}
}
