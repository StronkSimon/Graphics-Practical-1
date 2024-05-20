using OpenTK.Mathematics;


namespace RayTracer
{
    public abstract class Primitive
    {
        public Vector3 Color { get; set; }

        public abstract bool Intersect(Ray ray, out float t);
    }
}
