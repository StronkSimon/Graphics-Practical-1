using System;

namespace RayTracer
{
    // Represents a scene in the ray tracer which contains primitives and light sources.
    public class Scene
    {
        public List<Primitive> primitives = new List<Primitive>();
        public List<Light> lightSources = new List<Light>();

        // Initializes a new instance of the Scene class.
        public Scene()
        {
        }
    }
}