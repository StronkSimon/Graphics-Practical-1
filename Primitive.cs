using OpenTK.Mathematics;

namespace RayTracer
{
    // Base class for geometric primitives used in ray tracing.
    public abstract class Primitive
    {
        // Color of the primitive.
        public Vector3 Color { get; set; }

        // Specularity factor of the primitive, determining how shiny it appears.
        public float Specularity { get; set; }

        // Method to calculate intersection of a ray with this primitive. Returns the distance from the ray origin to the intersection point.
        public abstract float Intersect(Ray ray);

        // Method to compute the normal vector at the point of intersection.
        public abstract Vector3 IntersectToNorm(Vector3 intersect);

        // Method to determine the color of the primitive at the intersection point.
        public abstract Vector3 PrimitiveColor(Vector3 intersection);
    }

    // A sphere primitive.
    public class Sphere : Primitive
    {
        // Center position of the sphere.
        public Vector3 Position { get; set; }

        // Radius of the sphere.
        public float Radius { get; set; }

        // Constructor for sphere initializing position, radius, color, and specularity.
        public Sphere(Vector3 position, float radius, Vector3 color, float specularity)
        {
            Position = position;
            Radius = radius;
            Color = color;
            Specularity = specularity;
        }

        // Calculates ray-sphere intersection.
        public override float Intersect(Ray ray)
        {
            Vector3 c = Position - ray.E; // Vector from ray start to sphere center.
            float t = Vector3.Dot(c, ray.Direction); // Project c onto ray direction to find closest approach.
            Vector3 q = c - t * ray.Direction; // Vector from closest approach to sphere center.
            float p2 = q.LengthSquared; // Squared length of q.

            // Check if there is an intersection.
            if (!(p2 > Radius * Radius))
            {
                t -= MathF.Sqrt(Radius * Radius - p2); // Calculate the intersection point.
                return t;
            }
            return ray.Distance; // No intersection.
        }

        // Calculates normal at the point of intersection on the sphere.
        public override Vector3 IntersectToNorm(Vector3 intersect)
        {
            Vector3 norm = intersect - Position; // Vector from center to intersection.
            return Vector3.Normalize(norm); // Normalize the vector to get the normal.
        }

        // Returns the color of the sphere.
        public override Vector3 PrimitiveColor(Vector3 intersection)
        {
            return Color;
        }
    }

    // A plane primitive.
    public class Plane : Primitive
    {
        // Normal vector of the plane.
        public Vector3 Normal { get; set; }

        // Distance of the plane from the origin along its normal.
        public float DistanceToOrigin { get; set; }

        // Toggle for checkered texture pattern.
        public bool CheckBoard { get; set; }

        // Constructor for plane initializing normal, distance to origin, color, specularity, and checkboard pattern flag.
        public Plane(Vector3 normal, float distanceToOrigin, Vector3 color, float specularity, bool checkboard)
        {
            Normal = normal;
            DistanceToOrigin = distanceToOrigin;
            Color = color;
            Specularity = specularity;
            CheckBoard = checkboard;
        }

        // Calculates ray-plane intersection.
        public override float Intersect(Ray ray)
        {
            float denom = Vector3.Dot(Normal, ray.Direction);
            Vector3 p0 = Normal * DistanceToOrigin; // Calculate point on the plane using its normal and distance.
            Vector3 p0l0 = p0 - ray.E; // Vector from ray start to point on the plane.
            float t = Vector3.Dot(p0l0, Normal) / denom; // Calculate intersection distance.
            return t;
        }

        // Returns the normal of the plane.
        public override Vector3 IntersectToNorm(Vector3 intersect)
        {
            return Vector3.Normalize(Normal); // The normal is constant for any point on the plane.
        }

        // Calculates color at the intersection point on the plane.
        public override Vector3 PrimitiveColor(Vector3 intersection)
        {
            if (CheckBoard)
            {
                // Calculate texture coordinates.
                float u = intersection.X / 1f;
                float v = intersection.Z / 1f;
                bool isDark = ((Math.Floor(u) + Math.Floor(v)) % 2) == 0; // Determine color based on position.

                return isDark ? Color * 0.2f : Color; // Dark or light color based on checkered pattern.
            }
            return Color; // Single color if not checkered.
        }
    }
}