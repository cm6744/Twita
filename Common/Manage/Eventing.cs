using System.Collections.Generic;
using Twita.Codec;

namespace Twita.Common.Manage
{

	public class EventType
	{

		public Event Build(BinaryCompound data)
		{
			return new Event(this, data);
		}

		public Event Build()
		{
			return new Event(this, new BinaryCompound());
		}

	}

	public class Event
	{

		public BinaryCompound Data;
		public readonly EventType Type;
		public bool Cancelled;

		public Event(EventType type, BinaryCompound data)
		{
			Type = type;
			Data = data;
			Cancelled = false;
		}

		public bool Post()
		{
			return Eventbus.Distribute(this);
		}

	}

	public delegate void EventSubscriber(Event e);

	public class Eventbus
	{

		private static readonly Dictionary<EventType, EventSubscriber> Dict =
			new Dictionary<EventType, EventSubscriber>();

		public static void Observe(EventType type, EventSubscriber subs)
		{
			Dict.TryAdd(type, subs);
			Dict[type] += subs;
		}

		public static bool Distribute(Event e)
		{
			Dict[e.Type].Invoke(e);
			return e.Cancelled;
		}

	}

}