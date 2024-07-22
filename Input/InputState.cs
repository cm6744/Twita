using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using Twita.Maths.Structs;

namespace Twita.Input
{

	public interface InputState
	{

		public mutvec2 Cursor { get; }

		InputObserver Observe(Keycode code);

		InputObserver Observe(int code);

		string GetClipboardText();

		void PushToClipboard(string text);

		string GetTextInput();

		void ConsumeTextInput();

		float GetCursorScroll();

		void ConsumeCursorScroll();

		ScrollDirection GetScrollDirection();

		void StartRoll();

		void EndRoll();

	}

	public enum ScrollDirection
	{

		NONE,
		UP,
		DOWN

	}

}