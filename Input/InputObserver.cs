using Twita.Common;

namespace Twita.Input
{

	public interface InputObserver
	{

		void Consume();

		int HoldTime();

		bool Pressed();

		bool Holding();

	}

	public class KeyBind : InputObserver
	{

		private InputObserver observer;
		public string Key;

		public KeyBind(string key, Keycode code)
		{
			Key = key;
			Reset(code);
		}

		public void Reset(Keycode code)
		{
			observer = Platform.InputState.Observe(code);
		}

		public void Reset(int code)
		{
			observer = Platform.InputState.Observe(code);
		}

		public void Consume()
		{
			observer.Consume();
		}

		public int HoldTime()
		{
			return observer.HoldTime();
		}

		public bool Pressed()
		{
			return observer.Pressed();
		}

		public bool Holding()
		{
			return observer.Holding();
		}

	}

}