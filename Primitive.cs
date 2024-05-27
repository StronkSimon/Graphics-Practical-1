
﻿using System;
using OpenTK.Mathematics;

namespace RayTracer
{

    public abstract class Primitive
    {
        public Vector3 Color { get; set; }
        public float Specularity { get; set; }


        public abstract float Intersect(Ray ray);

        public abstract Vector3 IntersectToNorm(Vector3 intersect);

        public abstract Vector3 PrimitiveColor(Primitive primitive, Vector3 intersection);
    }

    public class Sphere : Primitive
    {
        public Vector3 Position { get; set; }
        public float Radius { get; set; }

        public Sphere(Vector3 position, float radius, Vector3 color, float specularity)
        {
            Position = position;
            Radius = radius;
            Color = color;
            Specularity = specularity;
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

        public override Vector3 IntersectToNorm(Vector3 intersect)
        {
            Vector3 norm = intersect - this.Position;
            return Vector3.Normalize(norm);
        }

        public override Vector3 PrimitiveColor(Primitive primitive, Vector3 intersection)
        {
            return primitive.Color;
        }


    }

    public class Plane : Primitive
    {
        public Vector3 Normal { get; set; }
        public float DistanceToOrigin { get; set; }

        public Plane(Vector3 normal, float distanceToOrigin, Vector3 color, float specularity)
        {
            Normal = normal;
            DistanceToOrigin = distanceToOrigin;
            Color = color;
            Specularity = specularity;
        }

        public override float Intersect(Ray ray)
        {
            float denom = Vector3.Dot(this.Normal, ray.Direction);
                Vector3 p0 = this.Normal * this.DistanceToOrigin; // Point on the plane
                Vector3 p0l0 = p0 - ray.E;
                float t = Vector3.Dot(p0l0, this.Normal) / denom;
            return t;
        }

        public override Vector3 IntersectToNorm(Vector3 intersect)
        {
            return Vector3.Normalize(-intersect);
        }

        public override Vector3 PrimitiveColor(Primitive primitive, Vector3 intersection)
        {
            float u = intersection.X - 0;
            float v = intersection.Z - 0;

            
            return primitive.Color;
        }

    }
}