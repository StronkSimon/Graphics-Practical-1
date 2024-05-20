using OpenTK.Mathematics;

namespace RayTracer
{
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

        public override bool Intersect(Ray ray, out float t)
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
        }
    }
}
