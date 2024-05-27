using OpenTK.Mathematics;

namespace RayTracer
{
    // Represents the details of an intersection point between a ray and a primitive.
    public class Intersection
    {
        public float IntersectionDistance { get; set; } // Distance to the intersection
        public Primitive Primitive { get; set; } // The primitive that was intersected
        public Vector3 Norm { get; set; } // The normal vector at the point of intersection

        public Intersection(float intersectionDistance, Primitive primitive)
        {
            this.IntersectionDistance = intersectionDistance;
            this.Primitive = primitive;
        }
    }
}