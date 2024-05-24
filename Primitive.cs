
﻿using System;
using OpenTK.Mathematics;

namespace RayTracer
{

    public abstract class Primitive
    {
        public Vector3 Color { get; set; }


        public abstract float Intersect(Ray ray);
    }

    public class Sphere : Primitive
    {
        public Vector3 Position { get; set; }
        public float Radius { get; set; }

        public Sphere(Vector3 position, float radius, Vector3 color)
        {
            Position = position;
            Radius = radius;
            Color = color;
        }

        public override float Intersect(Ray ray)
        {
            Vector3 c = Position - ray.E;
            float t = Vector3.Dot(c, ray.Direction);
            Vector3 q = c - t * ray.Direction;
            float p2 = q.LengthSquared;

            if (!(p2 > Radius * Radius))
            {
                t -= MathF.Sqrt(Radius * Radius - p2);
                return t;
            }
            else
            {
                return t = ray.Distance;
            }
        }
    }

    /*public class Plane : Primitive
    {
        public Vector3 Normal { get; set; }
        public float DistanceToOrigin { get; set; }

        public Plane(Vector3 normal, float distanceToOrigin, Vector3 color)
        {
            Normal = normal;
            DistanceToOrigin = distanceToOrigin;
            Color = color;
        }

        public override bool Intersect(Ray ray, out float t)
        {
            float denom = Vector3.Dot(this.Normal, ray.Direction);
            if (Math.Abs(denom) > 1e-6) // Ensure not parallel
            {
                Vector3 p0 = -this.Normal * this.DistanceToOrigin; // Point on the plane
                Vector3 p0l0 = p0 - ray.Origin;
                t = Vector3.Dot(p0l0, this.Normal) / denom;
                return t >= 0;
            }
            t = 0;
            return false;
        }
    }*/
}