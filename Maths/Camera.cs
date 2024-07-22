using Twita.Maths.Structs;

namespace Twita.Maths
{

	public class PerspectiveCamera
	{

		public vec2 Center = new vec2();
		public float ScaleX = 1.0f;
		public float ScaleY = 1.0f;
		public float Rotation = 0.0f;
		public rect4 Viewport = new rect4();
		public affine CombinedAffine = new affine();
		public affine InvertedAffine = new affine();
		public affine ProjectionAffine = new affine();
		protected affine TranslationAffine = new affine();
		public bool ShouldSeeCenter = false;

		public bool IsInSight(float x, float y, float w, float h, ref vec4 vp)
		{
			tmp.x = x;
			tmp.y = y;
			Project(tmp, vp);
			w /= ScaleX;
			h /= ScaleY;
			float vx = Center.x - Viewport.w / 2;
			float vy = Center.y - Viewport.h / 2;
			return tmp.x + w > vx && tmp.y + h > vy && tmp.x < Viewport.w && tmp.y < Viewport.h;
		}

		public void Push()
		{
			if(ShouldSeeCenter)
			{
				Center = new vec2(Viewport.w / 2.0f, Viewport.h / 2.0f);
			}

			float hw = Viewport.w / 2.0f;
			float hh = Viewport.h / 2.0f;
			ProjectionAffine.ToOrtho(ScaleX * -hw, ScaleX * hw, ScaleY * -hh, ScaleY * hh);
			TranslationAffine.Set(-Center.x, -Center.y, Rotation, 1.0f, 1.0f);
			CombinedAffine.Set(ProjectionAffine);
			CombinedAffine.Mul(TranslationAffine);
			InvertedAffine.Set(CombinedAffine);
			InvertedAffine.Invert();
			ShouldSeeCenter = false;
		}

		public void ToCenter()
		{
			ShouldSeeCenter = true;
		}

		public void Project(mutvec2 v, vec4 scrCoords)
		{
			CombinedAffine.ApplyTo(v);
			v.x = scrCoords.z * (v.x + 1) / 2 + scrCoords.x;
			v.y = scrCoords.w * (v.y + 1) / 2 + scrCoords.y;
		}

		public void Unproject(mutvec2 v, vec4 scrCoords)
		{
			v.x = 2.0f * (v.x - scrCoords.x) / scrCoords.z - 1.0f;
			v.y = 2.0f * (v.y - scrCoords.y) / scrCoords.w - 1.0f;
			InvertedAffine.ApplyTo(v);
		}

		private readonly mutvec2 tmp = new mutvec2();

		public float ToWldX(float x, vec4 scrCoords)
		{
			tmp.x = x;
			Unproject(tmp, scrCoords);
			return tmp.x;
		}

		public float ToWldY(float y, vec4 scrCoords)
		{
			tmp.y = y;
			Unproject(tmp, scrCoords);
			return tmp.y;
		}

		public float ToScrX(float x, vec4 scrCoords)
		{
			tmp.x = x;
			Project(tmp, scrCoords);
			return tmp.x;
		}

		public float ToScrY(float y, vec4 scrCoords)
		{
			tmp.y = y;
			Project(tmp, scrCoords);
			return tmp.y;
		}

	}

}