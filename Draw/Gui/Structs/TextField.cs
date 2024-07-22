using System;
using System.Text;
using Twita.Common;
using Twita.Input;
using Twita.Maths.Structs;

namespace Twita.Draw.Gui.Structs
{

	public class TextField : Component
	{

		public static InputObserver A_CODE = Platform.InputState.Observe(Keycode.KEY_A);
		public static InputObserver C_CODE = Platform.InputState.Observe(Keycode.KEY_C);
		public static InputObserver V_CODE = Platform.InputState.Observe(Keycode.KEY_V);
		public static InputObserver LD_CODE = Platform.InputState.Observe(Keycode.KEY_LEFT);
		public static InputObserver RD_CODE = Platform.InputState.Observe(Keycode.KEY_RIGHT);
		public static InputObserver DEL_CODE = Platform.InputState.Observe(Keycode.KEY_BACKSPACE);

		public static TextField ListeningField = null;
		public static int CURSOR_SHINE_TIME = Platform.DTps / 3;

		bool selectAll;
		bool cursorOn;
		public Icon[] Icons = new Icon[3];
		int pointer;
		StringBuilder text = new StringBuilder();
		string emptyDisplay;

		int clock;

		public void SetDisplayForEmpty(string v)
		{
			emptyDisplay = v;
		}

		public override void Update(TickSchedule schedule)
		{
			clock++;
		}

		public override void Input(InputState input, mutvec2 cursor)
		{
			cursorOn = false;

			bool boundIn = Bound.Contains(cursor);
			bool pressed = LEFT_CODE.Pressed();

			if(boundIn)
			{
				cursorOn = true;

				if(pressed)
				{
					ListeningField = this;
				}
			}

			if(ListeningField == this)
			{
				//Common Input Operations
				string txt = input.GetTextInput();
				if(!string.IsNullOrEmpty(txt))
				{
					insert(txt);
					input.ConsumeTextInput();
				}
				//Process Clipboard Operations
				if(CTRL_CODE.Holding())
				{
					if(V_CODE.Pressed())
					{
						insert(input.GetClipboardText());
					}
					if(A_CODE.Pressed())
					{
						selectAll = !selectAll;
					}
					if(C_CODE.Pressed() && selectAll)
					{
						input.PushToClipboard(text.ToString());
						selectAll = false;
					}
				}
				//END

				//Pointer Move Operations
				if(LD_CODE.Pressed() || LD_CODE.HoldTime() > FastActTime)
				{
					pointer = Math.Max(0, pointer - 1);
				}
				if(RD_CODE.Pressed() || RD_CODE.HoldTime() > FastActTime)
				{
					pointer = Math.Min(text.Length, pointer + 1);
				}
				//Backspace Operations
				if(DEL_CODE.Pressed() || DEL_CODE.HoldTime() > FastActTime)
				{
					if(text.Length > 0 && pointer != 0)
					{
						pointer = Math.Max(0, pointer - 1);
						text.Remove(pointer, 1);
					}
					if(selectAll)
					{
						pointer = 0;
						text.Clear();
						selectAll = false;
					}
				}
			}
		}

		private void insert(string txt)
		{
			text.Insert(pointer, txt);
			pointer += txt.Length;
		}

		public override void ReloadData()
		{
			Text = PersistentData.Get<string>("Content");
			pointer = PersistentData.Get<int>("Pointer");
		}

		public override void SaveData()
		{
			PersistentData.Set("Content", text.ToString());
			PersistentData.Set("Pointer", pointer);
		}

		public override void Draw(DrawBatch batch)
		{
			if(ListeningField == this)
			{
				batch.Draw(Icons[2], Bound);
			}
			else if(cursorOn)
			{
				batch.Draw(Icons[1], Bound);
			}
			else
			{
				batch.Draw(Icons[0], Bound);
			}

			if(text.Length == 0)
			{
				renderInBox(batch, emptyDisplay, new vec4(1, 1, 1, 0.2f));
			}
			else
			{
				renderInBox(batch, Text, selectAll ? new vec4(0.8f, 1f, 1f, 1f) : new vec4(1, 1, 1, 1));
			}
		}

		private void renderInBox(DrawBatch batch, string textIn, vec4 color)
		{
			batch.Color4(color);
			batch.Draw(textIn, Bound.xcentral + 6, Bound.ycentral + 2, Bound.w);

			if(ListeningField == this && clock % CURSOR_SHINE_TIME > CURSOR_SHINE_TIME / 2)
			{
				GlyphBounds bounds = batch.Font.GetBounds(textIn.Substring(0, pointer), Bound.w);
				//do not use emptyDisplay
				batch.NormalizeColor();
				float x = Bound.xcentral + bounds.Width + 6;
				float y = Bound.ycentral + 2;
				batch.FillTex(x, y, 1, bounds.Height);
			}

			batch.NormalizeColor();
		}

		public string Text
		{
			get => text.ToString();
			set
			{
				text.Clear();
				text.Append(value);
				pointer = 0;
			}
		}

		public void Activate()
		{
			ListeningField = this;
		}

	}

}