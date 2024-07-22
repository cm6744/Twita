using System.Drawing;
using System;
using Twita.Maths;
using Twita.Maths.Structs;

namespace Twita.Draw
{

	public abstract class DrawBatch
	{

		public static int DefaultSize = 1024 * 8;

		public affine Transform = new affine();
		public matrix4 Projection = new matrix4();
		public MatrixStack Matrices = new MatrixStack();

		public bool FlipX;
		public bool FlipY;

		public vec4[] Color = new vec4[4];

		public int _DrawCalls = 0;
		public vec4 ViewportArray;

		public Font Font;
		public FontCarver FontCarver = new DefaultFontCarver();

		public Texture Texfil;

		public VertexAppender[] VertAppenders = new VertexAppender[4];
		public UniformAppender UniformAppender;

		public ShaderProgram Program;
		
		public abstract void Draw(Texture texture, float x, float y, float width, float height, float srcX, float srcY,
			float srcWidth, float srcHeight);

		public void Draw(Texture tex, float x, float y, float sx, float sy, float sw, float sh)
		{
			Draw(tex, x, y, sw, sh, sx, sy, sw, sh);
		}

		public void Draw(Texture tex, float x, float y, float w, float h)
		{
			Draw(tex, x, y, w, h, 0, 0, tex.Width, tex.Height);
		}

		public void Draw(Texture tex, float x, float y)
		{
			Draw(tex, x, y, tex.Width, tex.Height, 0, 0, tex.Width, tex.Height);
		}

		public void Draw(Texture tex, box4 rect, float sx, float sy, float sw, float sh)
		{
			Draw(tex, rect.x, rect.y, rect.w, rect.h, sx, sy, sw, sh);
		}

		public void Draw(Texture tex, box4 rect)
		{
			Draw(tex, rect.x, rect.y, rect.w, rect.h);
		}

		public void Draw(Icon icon, float x, float y, float w, float h)
		{
			icon.Draw(this, x, y, w, h);
		}

		public void Draw(Icon icon, box4 rect)
		{
			Draw(icon, rect.x, rect.y, rect.w, rect.h);
		}

		//This method use the textured mode to present a rectangle.
		//Sometimes this is a much better choice.
		public void FillTex(float x, float y, float width, float height)
		{
			Draw(Texfil, x, y, width, height);
		}

		public abstract void CheckTransformAndCap();

		public abstract void Write(float v);

		public abstract void Write(vec2 vec);

		public abstract void Write(vec3 vec);

		public abstract void Write(vec4 vec);

		public abstract void Write(params float[] arr);

		public abstract void WriteTransformed(vec2 vec);

		public abstract void NewVertex(int v);

		public abstract void NewIndex(int v);

		public abstract void Flush();

		public void Color4(float r, float g, float b, float a = 1)
		{
			Color[0] = Color[1] = Color[2] = Color[3] = new vec4(r, g, b, a);
		}

		public void Color4(vec3 vec3, float a = 1)
		{
			Color[0] = Color[1] = Color[2] = Color[3] = new vec4(vec3.x, vec3.y, vec3.z, a);
		}

		public void Color4(vec4 color)
		{
			Color[0] = Color[1] = Color[2] = Color[3] = color;
		}

		public void Merge4(float r, float g, float b, float a = 1)
		{
			Color[0] = Color[1] = Color[2] = Color[3] *= new vec4(r, g, b, a);
		}

		public void Merge4(vec3 vec3, float a = 1)
		{
			Color[0] = Color[1] = Color[2] = Color[3] *= new vec4(vec3.x, vec3.y, vec3.z, a);
		}

		public void Merge4(vec4 color)
		{
			Color[0] = Color[1] = Color[2] = Color[3] *= color;
		}

		public void NormalizeColor()
		{
			Color[0] = Color[1] = Color[2] = Color[3] = new vec4(1, 1, 1, 1);
		}

		public abstract void Viewport(vec4 viewport);
		public abstract void Viewport(float x, float y, float w, float h);
		public abstract void Scissor(PerspectiveCamera camera, vec4 viewport, float x, float y, float w, float h);
		public abstract void ScissorEnd();
		public abstract void Clear();
		public abstract void UseCamera(PerspectiveCamera camera);
		public abstract void EndCamera(PerspectiveCamera camera);
		public abstract void UseShader(ShaderProgram program);
		public abstract void UseDefaultShader();

		public void UseVertAppenders(VertexAppender[] appds)
		{
			Array.Copy(appds, VertAppenders, 4);
		}

		public void EndVertAppenders()
		{
			VertAppenders[0] = VertAppenders[1] = VertAppenders[2] = VertAppenders[3] = null;
		}

		//Font Rendering:

		public void Draw(string text, float x, float y, float maxWidth = int.MaxValue)
		{
			FontCarver.Draw(this, Font, text, x, y, maxWidth);
		}

		public void Draw(string text, float x, float y, Align align, float maxWidth = int.MaxValue)
		{
			GlyphBounds bounds = Font.GetBounds(text);

			switch(align)
			{
				case Align.LEFT:
					Draw(text, x, y, maxWidth);
					break;
				case Align.RIGHT:
					Draw(text, (x - bounds.Width), y, maxWidth);
					break;
				case Align.CENTER:
					Draw(text, (x - bounds.Width / 2.0F), y, maxWidth);
					break;
			}
		}

	}

	public delegate void VertexAppender(DrawBatch batch);
	public delegate void UniformAppender(DrawBatch batch);

}