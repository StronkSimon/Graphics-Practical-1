
﻿using System;
using OpenTK.Mathematics;

namespace RayTracer
{

public abstract class Primitive
    {
        public Vector3 Color { get; set; }


       /* public abstract bool Intersect(Ray ray, out float t);*/
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

        /*public override bool Intersect(Ray ray, out float t)
        {
            Vector3 oc = ray.Origin - Position;  // Vector from ray origin to sphere center
            float a = Vector3.Dot(ray.Direction, ray.Direction);
            float b = 2.0f * Vector3.Dot(oc, ray.Direction);
            float c = Vector3.Dot(oc, oc) - Radius * Radius;
            float discriminant = b * b - 4 * a * c;

            if (discriminant < 0)
            {
                t = 0;
                return false;  // No intersection
            }
            else
            {
                // Calculating the smallest positive t (closest intersection point)
                float t0 = (-b - MathF.Sqrt(discriminant)) / (2 * a);
                float t1 = (-b + MathF.Sqrt(discriminant)) / (2 * a);
                t = (t0 < t1 && t0 >= 0) ? t0 : t1;
                return t >= 0;
            }
        }*/
    }

    public class Plane : Primitive
    {
        public Vector3 Normal { get; set; }
        public float DistanceToOrigin { get; set; }

        public Plane(Vector3 normal, float distanceToOrigin, Vector3 color)
        {
            Normal = normal;
            DistanceToOrigin = distanceToOrigin;
            Color = color;\
            
        }

       /* public override bool Intersect(Ray ray, out float t)
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
        }*/
    } 
}