
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

    public class Triangle : Primitive
    {
        public Vector3 V1 { get; set; }
        public Vector3 V2 { get; set; }
        public Vector3 V3 { get; set; }

        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 color)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
            Color = color;
        }

        public override float Intersect(Ray ray)
        {
            const float EPSILON = 0.0000001f;
            Vector3 edge1, edge2, h, s, q;
            float a, f, u, v, t;

            edge1 = V2 - V1;
            edge2 = V3 - V1;
            h = Vector3.Cross(ray.Direction, edge2);
            a = Vector3.Dot(edge1, h);

            if (a > -EPSILON && a < EPSILON)
                return -1;    // This ray is parallel to this triangle.

            f = 1.0f / a;
            s = ray.E - V1;
            u = f * Vector3.Dot(s, h);

            if (u < 0.0 || u > 1.0)
                return -1;

            q = Vector3.Cross(s, edge1);
            v = f * Vector3.Dot(ray.Direction, q);

            if (v < 0.0 || u + v > 1.0)
                return -1;

            // At this stage we can compute t to find out where the intersection point is on the line.
            t = f * Vector3.Dot(edge2, q);

            if (t > EPSILON) // ray intersection
                return t;

            // This means that there is a line intersection but not a ray intersection.
            return -1;
        }
    }
}