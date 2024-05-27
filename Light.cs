using OpenTK.Mathematics;

namespace RayTracer
{
    // Represents a light source with position and intensity.
    public class Light
    {
        public Vector3 Position { get; set; } // The position of the light in the scene
        public Vector3 Intensity { get; set; } // The intensity of the light

        public Light(Vector3 position, Vector3 intensity)
        {
            Position = position;
            Intensity = intensity;
        }
    }
}