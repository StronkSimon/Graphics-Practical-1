using System.Drawing;
using OpenTK.Mathematics;

namespace RayTracer
{
    // Core class for performing ray tracing operations in a scene.
    public class Raytracer
    {
        // Scene to be rendered.
        public Scene scene;

        // Camera from which the scene is viewed.
        public Camera camera;

        // Surface to render the image onto.
        public Surface screen;

        // Current recursion depth for reflection calculations.
        private int RecursionCount { get; set; }

        // Maximum allowed recursion depth to prevent infinite recursion in reflective scenes.
        private int RecursionCap = 10;

        // Initializes a new Raytracer object with a specified screen.
        public Raytracer(Surface screen)
        {
            this.camera = new Camera(new Vector3(0, 0, -4f), new Vector3(0, 0, 1f), new Vector3(0, 1f, 0), 1.2f, (float)screen.width / (float)screen.height);
            this.screen = screen;
        }

        // Displays camera's position and field of view on the screen.
        public void DisplayCameraInfo()
        {
            string position = $"Pos: {camera.position.X:F2}, {camera.position.Y:F2}, {camera.position.Z:F2}";
            string fov = $"FOV: {camera.fieldOfView * (180 / Math.PI):F2} Degrees";
            screen.Print(position, 10, 20, Color.Red.ToArgb());
            screen.Print(fov, 10, 50, Color.Blue.ToArgb());
        }

        // Renders the scene to the screen.
        public void Render()
        {
            this.scene = new Scene();
            int width = screen.width;
            int height = screen.height;

            // Initialize scene primitives and lights.
            InitializeScene();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    // Create a primary ray for each pixel to check for intersections with primitives.
                    Ray primaryRay = new Ray(camera.position, camera.RayDirection(i / (float)width, j / (float)height));
                    foreach (Primitive primitive in scene.primitives)
                    {
                        float Distance = primitive.Intersect(primaryRay);
                        if (Distance < primaryRay.Distance && Distance > 0)
                        {
                            primaryRay.Distance = Distance;
                            primaryRay.Intersection = new Intersection(Distance, primitive);
                        }
                    }

                    // Determine the color of the pixel based on intersections.
                    Vector3 IntersectionPoint = primaryRay.E + primaryRay.Distance * primaryRay.Direction;
                    if (primaryRay.Distance < 0 || primaryRay.Distance == 100)
                    {
                        screen.pixels[i + j * width] = 0; // No intersection, render as black.
                        continue;
                    }

                    RecursionCount = 0;
                    Vector3 FinalColor = RefColor(primaryRay); // Calculate color considering reflections.

                    // Set the pixel color on the screen.
                    screen.Plot(i, j, (int)(FinalColor.X * 255) + ((int)(FinalColor.Y * 255) << 8) + ((int)(FinalColor.Z * 255) << 16));
                }
            }

            DisplayCameraInfo(); // Display camera information.
            // Debug(); // Un-comment this line for debugging purposes.
        }

        // Initialize scene with primitives and light sources.
        private void InitializeScene()
        {
            scene.primitives.Add(new Sphere(new Vector3(0, 0, 0), 1.2f, new Vector3(1f, 1f, 1f), 1f));
            scene.primitives.Add(new Sphere((-2.5f, 2f, 2.0f), 1.2f, (0, 1f, 0), 0f));
            scene.primitives.Add(new Sphere((2.5f, 2f, 3.0f), 2f, (0f, 0f, 1f), 0.5f));
            scene.primitives.Add(new Sphere((0f, 0, -6f), 1f, (1f, 1f, 1f), 1f));
            scene.primitives.Add(new Plane((0, 0, 1f), 5f, (230f / 255f, 230f / 255f, 0), 0, false));
            scene.primitives.Add(new Plane((0, -1f, 0), 5f, (200f / 255f, 200f / 255f, 0), 0f, true));
            scene.primitives.Add(new Plane((0, 1f, 0), 10f, (200f / 255f, 200f / 255f, 0), 0f, true));
            scene.primitives.Add(new Plane((1f, 0, 0), 5f, (1f, 1f, 0), 0.5f, false));
            scene.primitives.Add(new Plane((-1f, 0, 0), 5f, (1f, 1f, 0), 0.5f, false));
            scene.primitives.Add(new Plane((0, 0f, -1f), 20f, (200f / 255f, 200f / 255f, 0), 0f, false));

            scene.lightSources.Add(new Light(new Vector3(4f, 1f, 0f), new Vector3(1f, 1f, 1f)));
            scene.lightSources.Add(new Light((-2f, 2f, -1f), (1f, 1f, 1f)));
            scene.lightSources.Add(new Light((0f, 4f, 4f), (1f, 1f, 1f)));
        }

        // Calculates the reflected color from intersections.
        private Vector3 RefColor(Ray ray)
        {
            RecursionCount++;
            if (RecursionCount > RecursionCap) return GetColor(ray); // Cap recursion to prevent infinite loops.

            Primitive RayPrimitive = ray.Intersection.Primitive;
            Vector3 IntersectionPoint = ray.E + ray.Distance * ray.Direction;

            if (RayPrimitive.Specularity == 1f)
            {
                // Compute reflection for specular surfaces.
                Vector3 Normal = RayPrimitive.IntersectToNorm(IntersectionPoint);
                Vector3 R = Vector3.Normalize(ray.Direction - 2 * Vector3.Dot(ray.Direction, Normal) * Normal);
                Ray reflectionRay = new Ray(IntersectionPoint, R);
                foreach (Primitive primitive in scene.primitives)
                {
                    float Distance = primitive.Intersect(reflectionRay);
                    if (Distance < reflectionRay.Distance && Distance > 0)
                    {
                        reflectionRay.Distance = Distance;
                        reflectionRay.Intersection = new Intersection(Distance, primitive);
                    }
                }

                return RayPrimitive.PrimitiveColor(IntersectionPoint) * RefColor(reflectionRay); // Recursive call for reflection.
            }
            else
            {
                return GetColor(ray); // Non-reflective surfaces.
            }
        }

        // Calculates the base color of the intersection.
        private Vector3 GetColor(Ray ray)
        {
            RecursionCount = 0;
            Vector3 IntersectionPoint = ray.E + ray.Distance * ray.Direction;
            Primitive RayPrimitive = ray.Intersection.Primitive;
            Vector3 BaseColor = RayPrimitive.PrimitiveColor(IntersectionPoint) * 0.08f; // Ambient light component.

            // Calculate lighting contributions.
            foreach (Light light in scene.lightSources)
            {
                // Shadow calculation.
                bool InShadow = false;
                Vector3 NormalLightDirection = Vector3.Normalize(IntersectionPoint - light.Position);
                Ray shadowRay = new Ray(light.Position, NormalLightDirection);

                foreach (Primitive primitive in scene.primitives)
                {
                    float Distance = primitive.Intersect(shadowRay);
                    if (Distance > 0 && Distance < shadowRay.Distance)
                    {
                        if (Distance < (IntersectionPoint - light.Position).Length - 0.001f)
                        {
                            InShadow = true;
                            break;
                        }
                        else
                        {
                            shadowRay.Distance = Distance;
                            shadowRay.Intersection = new Intersection(Distance, primitive);
                        }
                    }
                }

                if (!InShadow)
                {
                    // Calculate diffuse and specular components.
                    float SpecularCoefficient = RayPrimitive.Specularity;
                    Vector3 color = RayPrimitive.PrimitiveColor(IntersectionPoint);
                    Vector3 Normal = RayPrimitive.IntersectToNorm(IntersectionPoint);

                    Vector3 R = -NormalLightDirection - 2 * Vector3.Dot(-NormalLightDirection, Normal) * Normal;
                    float distance = (IntersectionPoint - light.Position).Length;
                    float attenuation = 1.0f / (0.6f + .5f * distance + 0.5f * (distance * distance)); // Light attenuation.

                    Vector3 Brightness = attenuation * light.Intensity;

                    float kd = MathF.Max(0, Vector3.Dot(Normal, -NormalLightDirection)); // Diffuse component.
                    float ks = MathF.Max(0, MathF.Pow(Vector3.Dot(ray.Direction, R.Normalized()), 400f)); // Specular component.

                    BaseColor += Brightness * color * kd + Brightness * SpecularCoefficient * ks;
                }
            }
            return BaseColor; // Final color with lighting effects.
        }
    }
}