using System;
using OpenTK.Mathematics;

namespace RayTracer
{
    public class Light
    {
        public Vector3 Position { get; set; }
        public Vector3 Intensity { get; set; }

        public Light(Vector3 position, Vector3 intensity)
        {
            Position = position;
            Intensity = intensity;
        }
    }
}
