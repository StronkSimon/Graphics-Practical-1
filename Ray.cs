
using System;
using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;


namespace RayTracer
{
    public class Ray
    {
        public Vector3 E { get; set; }
        public Vector3 Direction { get; set; }
       // public Vector3 ray { get; set; }
        public float Distance = 100;
        public Intersection Intersection { get; set; }
       // public bool Hit;

        public Ray(Vector3 e, Vector3 direction)
        {
            this.E = e;
            this.Direction = direction; 
        }

        /*public void PrimaryIntersect(Scene scene)
        {
            foreach(Sphere sphere in scene.primitives)
            {
                Vector3 c = sphere.Position - this.E;
                float tt = Vector3.Dot(c, this.Direction);
                Vector3 q = c - tt * this.Direction;
                float p2 = q.LengthSquared;

                if (!(p2 > sphere.Radius * sphere.Radius))
                {
                    tt -= MathF.Sqrt(sphere.Radius * sphere.Radius - p2);

                    if ((tt < Distance) && (tt > 0))
                    {
                        Distance = tt;
                        this.IntersectionPoint = this.E + Distance * this.Direction;
                        this.Intersection = new Intersection(Distance, sphere, this.IntersectionPoint.Normalized());
                    }
                }
            }
            
        }*/

        /*public void ShadowIntersect(Scene scene)
        {
            foreach (Sphere sphere in scene.primitives)
            {
                
                foreach (Light light in scene.lightSources)
                {
                    Vector3 c = sphere.Position - light.Position;
                    Vector3 d = this.intersection.IntersectionRay - light.Position;
                    float tt = Vector3.Dot(c, d.Normalized());
                    if (tt < d.Length && tt > 0)
                    {
                        Vector3 q = c - tt * d.Normalized();
                        float p2 = q.LengthSquared;

                        if (!(p2 > sphere.Radius * sphere.Radius))
                        {
                            this.hit = false;
                        }
                        else
                        {
                            this.hit = true;
                        }
                    }
                }
            }
        }*/
    }
}
