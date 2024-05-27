
using System;
using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;


namespace RayTracer
{
    public class Ray
    {
        public Vector3 E { get; set; }
        public Vector3 Direction { get; set; }
        public float Distance = 100;
        public Intersection Intersection { get; set; }
       

        public Ray(Vector3 e, Vector3 direction)
        {
            this.E = e;
            this.Direction = direction; 
        }
    }
}
