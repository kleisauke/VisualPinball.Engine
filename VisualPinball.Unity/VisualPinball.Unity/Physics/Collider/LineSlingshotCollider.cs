﻿using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using VisualPinball.Engine.Physics;
using VisualPinball.Unity.Extensions;
using VisualPinball.Unity.Physics.Collision;
using VisualPinball.Unity.VPT.Ball;
using VisualPinball.Unity.VPT.Surface;

namespace VisualPinball.Unity.Physics.Collider
{
	public struct LineSlingshotCollider : ICollider, ICollidable
	{
		private ColliderHeader _header;

		private float2 _v1;
		private float2 _v2;
		private float2 _normal;
		private float _length;

		public ColliderType Type => _header.Type;

		public static void Create(BlobBuilder builder, LineSeg src, ref BlobPtr<Collider> dest)
		{
			ref var linePtr = ref UnsafeUtilityEx.As<BlobPtr<Collider>, BlobPtr<LineSlingshotCollider>>(ref dest);
			ref var collider = ref builder.Allocate(ref linePtr);
			collider.Init(src);
		}

		private void Init(LineSeg src)
		{
			_header.Type = ColliderType.Line;
			_header.ItemType = Collider.GetItemType(src.ObjType);
			_header.Id = src.Id;
			_header.Entity = new Entity {Index = src.ItemIndex, Version = src.ItemVersion};

			_v1 = src.V1.ToUnityFloat2();
			_v2 = src.V2.ToUnityFloat2();

			CalcNormal();
		}

		private void CalcNormal()
		{
			var vT = new float2(_v1.x - _v2.x, _v1.y - _v2.y);

			// Set up line normal
			_length = math.length(vT);
			var invLength = 1.0f / _length;
			_normal.x = vT.y * invLength;
			_normal.y = -vT.x * invLength;
		}

		public float HitTest(ref CollisionEventData coll, in SurfaceData surfaceData, in BallData ball, float dTime)
		{
			return -3;
		}
	}
}