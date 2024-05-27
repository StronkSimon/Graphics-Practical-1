
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

        public abstract Vector3 PrimitiveColor(Vector3 intersection);
    }

    public class Sphere : Primitive //Sphere primitive
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
            Vector3 c = Position - ray.E;               //get the vector from the sphere center to the start of the ray
            float t = Vector3.Dot(c, ray.Direction);    //use the cector c onto the ray direction to find the closest intersection from the ray
            Vector3 q = c - t * ray.Direction;          //calculate the perpendicular distance vector from the ray to the sphere center
            float p2 = q.LengthSquared;                 //get the squared length

            if (!(p2 > Radius * Radius))                // if the squared perpendicular distance is less than or equal to the squared radius,                                       
            {                                           // the ray intersects the sphere
                t -= MathF.Sqrt(Radius * Radius - p2);
                return t;                               // adjust t taking in mind the distance from the closest intersection
            }
            else
            {
                return t = ray.Distance; 
            }
        }

        public override Vector3 IntersectToNorm(Vector3 intersect)
        {
            Vector3 norm = intersect - this.Position; //get the normal of the sphere intersectoion
            return Vector3.Normalize(norm);
        }

        public override Vector3 PrimitiveColor(Vector3 intersection)
        {
            return this.Color;
        }


    }

    public class Plane : Primitive
    {
        public Vector3 Normal { get; set; }
        public float DistanceToOrigin { get; set; }
        bool CheckBoard { get; set; }

        public Plane(Vector3 normal, float distanceToOrigin, Vector3 color, float specularity, bool checkboard)
        {
            Normal = normal;
            DistanceToOrigin = distanceToOrigin;
            Color = color;
            Specularity = specularity;
            CheckBoard = checkboard;
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
            return Vector3.Normalize(-intersect); //return the normal
        }

        public override Vector3 PrimitiveColor(Vector3 intersection)
        {
            if (CheckBoard)
            {
                float u = intersection.X / 1f; //calculate location
                float v = intersection.Z / 1f;

                
                bool isDark = ((Math.Floor(u) + Math.Floor(v)) % 2) == 0; //check if the current block is dark or light

                
                if (isDark)
                {
                    return this.Color*0.2f; // dark color
                }
                else
                {
                    return this.Color; // light color
                }
            }
            else
            {
                return this.Color; // normal color
            }
        }

    }
}