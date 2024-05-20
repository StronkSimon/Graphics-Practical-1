using OpenTK.Mathematics;

namespace RayTracer
{
    public class Plane : Primitive
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
    }
}
