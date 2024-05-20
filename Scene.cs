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
			one = new Sphere((0, 0, 0), 2, (200, 200, 200));
			two = new Sphere((4, 0, 0), 2, (200, 200, 200));
            three = new Sphere((8, 0, 0), 2, (200, 200, 200));
			primitives.Add(one);
			primitives.Add(two);
			primitives.Add(three);
        }
    }
}
