using System.Collections.Generic;
using UnityEngine.Events;

public class UnityObjectEvent : UnityEvent<object> { };

/// <summary>
/// EventManager class for managing events/actions with data/orNot
/// </summary>
public class EventManager
{
    public static EventManager Instance { get; } = new EventManager();

    private static Dictionary<string, UnityEvent> events;
    private static Dictionary<string, UnityObjectEvent> eventsWithData;

    static EventManager()
    {
        events = new Dictionary<string, UnityEvent>();
        eventsWithData = new Dictionary<string, UnityObjectEvent>();
    }

    /// <summary>
    /// Start listerner to an event without extra data
    /// </summary>
    /// <param name="eventName">Name of the event</param>
    /// <param name="action">Action to execute</param>
    public static void StartListening(string eventName, UnityAction action)
    {
        if (events.TryGetValue(eventName, out UnityEvent auxEvent))
        {
            auxEvent.AddListener(action);
        }
        else
        {
            auxEvent = new UnityEvent();
            auxEvent.AddListener(action);
            events.Add(eventName, auxEvent);
        }
    }

    /// <summary>
    /// Start listener to an event with extra data
    /// </summary>
    /// <param name="eventName">Name of the event</param>
    /// <param name="action">Action to execute with the extra data</param>
    public static void StartListening(string eventName, UnityAction<object> action)
    {
        if (eventsWithData.TryGetValue(eventName, out UnityObjectEvent auxEvent))
        {
            auxEvent.AddListener(action);
        }
        else
        {
            auxEvent = new UnityObjectEvent();
            auxEvent.AddListener(action);
            eventsWithData.Add(eventName, auxEvent);
        }
    }

    /// <summary>
    /// Stop a specific listener from an event
    /// </summary>
    /// <param name="eventName">Name of the event</param>
    /// <param name="action">Action to stop</param>
    public static void Stoplistening(string eventName, UnityAction action)
    {
        if (events.TryGetValue(eventName, out UnityEvent auxEvent))
        {
            auxEvent.RemoveListener(action);
        }
    }

    /// <summary>
    /// Stop a specific listener from an event with data
    /// </summary>
    /// <param name="eventName">Name of the event</param>
    /// <param name="action">Action to stop</param>
    public static void Stoplistening(string eventName, UnityAction<object> action)
    {
        if (eventsWithData.TryGetValue(eventName, out UnityObjectEvent auxEvent))
        {
            auxEvent.RemoveListener(action);
        }
    }

    /// <summary>
    /// Stop all listeners from a specific event
    /// </summary>
    /// <param name="eventName">Name of the event</param>
    /// <param name="isDataBased">If it is an data based event or not</param>
    public static void StopAllListeners(string eventName, bool isDataBased)
    {
        if (!isDataBased)
        {
            if (events.TryGetValue(eventName, out UnityEvent auxEvent))
            {
                auxEvent.RemoveAllListeners();
            }
        }
        else
        {
            if (eventsWithData.TryGetValue(eventName, out UnityObjectEvent auxEvent))
            {
                auxEvent.RemoveAllListeners();
            }
        }
    }

    /// <summary>
    /// Trigger a speficic event
    /// </summary>
    /// <param name="eventName">Name of the event</param>
    public static void TriggerEvent(string eventName)
    {
        if (events.TryGetValue(eventName, out UnityEvent auxEvent))
        {
            auxEvent.Invoke();
        }
    }

    /// <summary>
    /// Trigger a specific event with data
    /// </summary>
    /// <param name="eventName">Name of the event</param>
    /// <param name="data">Data to send</param>
    public static void TriggerEvent(string eventName, object data)
    {
        if (eventsWithData.TryGetValue(eventName, out UnityObjectEvent auxEvent))
        {
            auxEvent.Invoke(data);
        }
    }
}
