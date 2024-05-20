using System;

namespace RayTracer
{
	public class Scene
	{
		public List<Primitive> primitives = new List<Primitive>();
		public List<Light> lightSources = new List<Light>();
		public Scene()
		{
        }
    }
}
