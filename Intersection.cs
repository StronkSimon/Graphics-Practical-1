using System;
using System.Reflection.Metadata;
using OpenTK.Mathematics;

namespace RayTracer
{
	public class Intersection
	{
		public float IntersectionDistance { get; set; }
        public Primitive Primitive { get; set; }
        public Vector3 Norm { get; set; }

        public Intersection(float intersectionDistance,Primitive primitive)
		{
			this.IntersectionDistance = intersectionDistance;
			this.Primitive = primitive;
			
		}
	}
}
