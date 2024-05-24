using System;
using System.Reflection.Metadata;
using OpenTK.Mathematics;

namespace RayTracer
{
	public class Intersection
	{
		public float IntersectionDistance;
		public Primitive Primitive;

		public Intersection(float intersectionDistance,Primitive primitive)
		{
			this.IntersectionDistance = intersectionDistance;
			this.Primitive = primitive;
		}
	}
}
