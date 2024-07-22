using Twita.Codec;
using Twita.Common;
using Twita.Input;
using Twita.Maths.Structs;

namespace Twita.Draw.Gui.Structs
{

	public abstract class Component
	{

		public static int FastActTime = Platform.DTps * 3 / 2;

		public static InputObserver LEFT_CODE = Platform.InputState.Observe(Keycode.BUTTON_LEFT);
		public static InputObserver RIGHT_CODE = Platform.InputState.Observe(Keycode.BUTTON_RIGHT);
		public static InputObserver CTRL_CODE = Platform.InputState.Observe(Keycode.KEY_LEFT_CONTROL);
		public static InputObserver SHIFT_CODE = Platform.InputState.Observe(Keycode.KEY_LEFT_SHIFT);
		
		public box4 Bound = new box4();
		public BinaryCompound PersistentData = new BinaryCompound();
		public int IdxInScreen;

		public virtual void ReloadData() { }

		public virtual void SaveData() { }

		public virtual void Update(TickSchedule schedule) { }

		public virtual void Draw(DrawBatch batch) { }

		public virtual void Input(InputState input, mutvec2 cursor) { }

	}

}