
using System;
using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;


namespace RayTracer
{
    public class Ray
    {
        public Vector3 E { get; set; }
        public Vector3 Direction { get; set; }
        public Vector3 ray { get; set; }
        public Scene Scene { get; set; }
        public float t = 100;

        public Ray(Vector3 e, Vector3 direction, Scene scene)
        {
            this.E = e;
            this.Direction = direction;
            this.Scene = scene;
            this.Intersect(scene);
            this.ray = this.E + t * direction;
        }

        public void Intersect(Scene scene)
        {
            foreach(Sphere sphere in scene.primitives)
            {
                Vector3 c = sphere.Position - this.E;
                float tt = Vector3.Dot(c, this.Direction);
                Vector3 q = c - tt * this.Direction;
                float p2 = q.LengthSquared;

                if (!(p2 > sphere.Radius * sphere.Radius))
                {
                    tt -= (float)Math.Sqrt((double)sphere.Radius * (double)sphere.Radius - (double)p2);

                    if ((tt < t) && (tt > 0)) t = tt;
                }
            }
        }
    }
}
