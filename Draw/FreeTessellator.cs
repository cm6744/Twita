using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twita.Maths.Structs;

namespace Twita.Draw
{

	//Only provide Vao drawing.
	//Get your own version if you want to touch deeper.
	public abstract class FreeTessellator
	{

		public MatrixStack Matrices = new MatrixStack();
		public affine Transform = new affine();
		public ShaderProgram Program;

		//Invoke this before you start each object rendering with your own write verts.
		public abstract void CheckTransformAndCap();

		public abstract void Write(float v);

		public abstract void Write(vec2 vec);

		public abstract void Write(vec3 vec);

		public abstract void Write(vec4 vec);

		public abstract void Write(params float[] arr);

		public abstract void WriteTransformed(vec2 vec);

		public abstract void NewVertex(int v);

		public abstract void PrimitiveMode(Primitives primitives);

		public abstract void UseShader(ShaderProgram program);

		public abstract void Flush();

	}

	public enum Primitives
	{
		Triangles,
		Lines,
		Point
	}

}
