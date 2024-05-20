using System;

namespace RayTracer
{
	public class Scene
	{
		public List<Primitive> primitives = new List<Primitive>();
		public List<Light> lightSources = new List<Light>();
		public Sphere one;
        public Sphere two;
        public Sphere three;
		public Scene()
		{
        }
    }
}
