using OpenTK.Mathematics;

namespace RayTracer
{
    // Represents a ray with an origin (E) and a direction, used in ray tracing calculations.
    public class Ray
    {
        public Vector3 E { get; set; } // The starting point of the ray
        public Vector3 Direction { get; set; } // The direction of the ray
        public float Distance = 100; // Maximum traceable distance
        public Intersection Intersection { get; set; } // Stores intersection data if the ray hits a primitive

        public Ray(Vector3 e, Vector3 direction)
        {
            this.E = e;
            this.Direction = direction;
        }
    }
}