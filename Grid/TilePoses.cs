using System;
using Twita.Maths;

namespace Twita.Grid
{

	public static class Posing
	{

		public static TilePos Offset(TilePos pos, int dx, int dy, int dz = 0)
		{
			return new TilePos(pos.TileX + dx, pos.TileY + dy, pos.TileZ + dz);
		}

		public static float Distance(IPos pos1, IPos pos2)
		{
			return Mth.Sqrt(Mth.Pow(pos1.x - pos2.x, 2) + Mth.Pow(pos1.y - pos2.y, 2));
		}

		public static float PointRad(IPos pos1, IPos pos2)
		{
			return Mth.AtanRad(pos2.y - pos1.y, pos2.x - pos1.x);
		}

		public static float PointDeg(IPos pos1, IPos pos2)
		{
			return Mth.AtanDeg(pos2.y - pos1.y, pos2.x - pos1.x);
		}

		public static int ToCoord(int tile)
		{
			return Mth.Floor(tile / 16f);
		}

	}

	//1 Chunk = 16 Tile;
	//1 Grid = 16 Pixel;
	//1 Pixel => N Precise;
	public interface IPos
	{

		public float x { get; }
		public float y { get; }
		public int TileX { get; }
		public int TileY { get; }
		public int UnitX { get; }
		public int UnitY { get; }

		//Default impl at z = 1.
		public int TileZ => 1;

	}

	public struct PrecisePos : IPos
	{

		public PrecisePos(IPos grid)
		{
			x = grid.x;
			y = grid.y;
		}

		public PrecisePos(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public float x { get; }
		public float y { get; }
		public int TileX => Mth.Floor(x);
		public int TileY => Mth.Floor(y);
		public int UnitX => Mth.Floor(TileX / 16f);
		public int UnitY => Mth.Floor(TileY / 16f);

		public override bool Equals(object obj)
		{
			return obj is PrecisePos other && Equals(other);
		}

		public bool Equals(PrecisePos other)
		{
			return x.Equals(other.x) && y.Equals(other.y);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(x, y);
		}

		public static PrecisePos operator +(PrecisePos p1, PrecisePos p2)
		{
			return new PrecisePos(p1.x + p2.x, p1.y + p2.y);
		}

		public static PrecisePos operator -(PrecisePos p1, PrecisePos p2)
		{
			return new PrecisePos(p1.x - p2.x, p1.y - p2.y);
		}

		public static PrecisePos operator -(PrecisePos p)
		{
			return new PrecisePos(-p.x, -p.y);
		}

		public static bool operator ==(PrecisePos p1, PrecisePos p2)
		{
			return p1.Equals(p2);
		}

		public static bool operator !=(PrecisePos p1, PrecisePos p2)
		{
			return !p1.Equals(p2);
		}

	}

	public struct TilePos : IPos
	{

		public static int DefaultZDep = 1;

		public TilePos(IPos grid, int z)
		{
			TileX = grid.TileX;
			TileY = grid.TileY;
			TileZ = z;
		}

		public TilePos(IPos grid)
		{
			TileX = grid.TileX;
			TileY = grid.TileY;
			TileZ = grid is TilePos g2 ? g2.TileZ : DefaultZDep;
		}

		public TilePos(int x, int y, int z)
		{
			TileX = x;
			TileY = y;
			TileZ = z;
		}

		public TilePos(int x, int y)
		{
			TileX = x;
			TileY = y;
			TileZ = DefaultZDep;
		}

		public float x => TileX + 0.5f;
		public float y => TileY + 0.5f;
		public int TileX { get; }
		public int TileY { get; }
		public int TileZ { get; }
		public int UnitX => Mth.Floor(TileX / 16f);
		public int UnitY => Mth.Floor(TileY / 16f);

		public override bool Equals(object obj)
		{
			return obj is TilePos other && Equals(other);
		}

		public bool Equals(TilePos other)
		{
			return TileX == other.TileX && TileY == other.TileY && TileZ == other.TileZ;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(TileX, TileY, TileZ);
		}

		public static TilePos operator +(TilePos p1, TilePos p2)
		{
			return new TilePos(p1.TileX + p2.TileX, p1.TileY + p2.TileY, p1.TileZ + p2.TileZ);
		}

		public static TilePos operator -(TilePos p1, TilePos p2)
		{
			return new TilePos(p1.TileX - p2.TileX, p1.TileY - p2.TileY, p1.TileZ - p2.TileZ);
		}

		public static TilePos operator -(TilePos p)
		{
			return new TilePos(-p.TileX, -p.TileY, -p.TileZ);
		}

		public static bool operator ==(TilePos p1, TilePos p2)
		{
			return p1.Equals(p2);
		}

		public static bool operator !=(TilePos p1, TilePos p2)
		{
			return !p1.Equals(p2);
		}

	}

	public struct UnitPos : IPos
	{

		public UnitPos(IPos grid)
		{
			UnitX = grid.UnitX;
			UnitY = grid.UnitY;
		}

		public UnitPos(int x, int y)
		{
			UnitX = x;
			UnitY = y;
		}

		public float x => TileX;
		public float y => TileY;
		public int TileX => UnitX * 16;
		public int TileY => UnitY * 16;
		public int UnitX { get; }
		public int UnitY { get; }

		public override bool Equals(object obj)
		{
			return obj is UnitPos other && Equals(other);
		}

		public bool Equals(UnitPos other)
		{
			return UnitX == other.UnitX && UnitY == other.UnitY;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(UnitX, UnitY);
		}

		public static UnitPos operator +(UnitPos p1, UnitPos p2)
		{
			return new UnitPos(p1.UnitX + p2.UnitX, p1.UnitY + p2.UnitY);
		}

		public static UnitPos operator -(UnitPos p1, UnitPos p2)
		{
			return new UnitPos(p1.UnitX - p2.UnitX, p1.UnitY - p2.UnitY);
		}

		public static UnitPos operator -(UnitPos p)
		{
			return new UnitPos(-p.UnitX, -p.UnitY);
		}

		public static bool operator ==(UnitPos p1, UnitPos p2)
		{
			return p1.Equals(p2);
		}

		public static bool operator !=(UnitPos p1, UnitPos p2)
		{
			return !p1.Equals(p2);
		}

	}

}