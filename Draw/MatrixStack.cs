using System.Collections.Generic;
using Twita.Maths;
using Twita.Maths.Structs;

namespace Twita.Draw
{

	public class MatrixStack
	{

		public MatrixStack()
		{
			RecreateMatsForLen(128);
			Stack.Push(Top);
		}

		public void RecreateMatsForLen(int len)
		{
			if(TempMats == null || TempMats.Length < len)
			{
				TempMats = new affine[len];
				for(int i = 0; i < TempMats.Length; i++) TempMats[i] = new affine();
			}
		}

		public bool Changed;
		public affine Top = new affine();

		affine[] TempMats;
		Stack<affine> Stack = new Stack<affine>();

		public bool IsEmpty => Stack.Count == 1;//Top0 is never removed.

		public void Push()
		{
			int take = Stack.Count;
			if(take < TempMats.Length)
			{
				affine aff = TempMats[take];
				aff.Identity();
				Push(aff);
				return;
			}

			Push(new affine());
		}

		void Push(affine aff)
		{
			Top = aff;
			Top.Set(Stack.Peek());
			Stack.Push(Top);
			Changed = true;
		}

		public void Pop()
		{
			Stack.Pop();
			Top = Stack.Peek();
			Changed = true;
		}

		public void Load(affine affine)
		{
			Top.Set(affine);
			Changed = true;
		}

		public void RotateRad(float f)
		{
			Top.Rotate(f);
			Changed = true;
		}

		public void RotateDeg(float f)
		{
			Top.Rotate(Mth.Rad(f));
			Changed = true;
		}

		public void RotateRad(float f, float x, float y)
		{
			Translate(x, y);
			RotateRad(f);
			Translate(-x, -y);
		}

		public void RotateDeg(float f, float x, float y)
		{
			Translate(x, y);
			RotateDeg(f);
			Translate(-x, -y);
		}

		public void Translate(float x, float y)
		{
			Top.Translate(x, y);
			Changed = true;
		}

		public void Scale(float x, float y)
		{
			Top.Scale(x, y);
			Changed = true;
		}

	}

}