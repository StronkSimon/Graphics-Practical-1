using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace RayTracer
{
    public class Raytracer
    {
        public Scene scene;
        public Camera camera;
        public Surface screen;
        private int RecursionCount;
        private int RecursionCap =10;

        public Raytracer(Surface screen)
        {
            this.camera = new Camera((0,0,-4f),(0,0,1f),(0,1f,0),1.2f,(float)screen.width/(float)screen.height); //position-lookDirection-upDirection-Fov-aspectRatio
            this.screen = screen;
            
        }

        public void DisplayCameraInfo()
        {
            string position = $"Pos: {camera.position.X:F2}, {camera.position.Y:F2}, {camera.position.Z:F2}";
            string fov = $"FOV: {camera.fieldOfView * (180 / Math.PI):F2} Degrees";

            screen.Print(position, 10, 20, Color.Red.ToArgb());
            screen.Print(fov, 10, 50, Color.Blue.ToArgb());
        }

        public void Render()
        {
            this.scene = new Scene();
            int width = screen.width;
            int height = screen.height;
            
            scene.primitives.Add(new Sphere((0, 0, 0), 1.2f, (1f, 1f, 1f), 1f)); 
            scene.primitives.Add(new Sphere((-2.5f, 2f, 2.0f), 1.2f, (0, 1f, 0), 0.5f));
            scene.primitives.Add(new Sphere((2.5f, 2f, 3.0f), 2f, (0f, 0f, 1f), 0.5f));
            scene.primitives.Add(new Sphere((0f, 0, -4f), 2f, (1f, 1f, 1f),1f));
            scene.primitives.Add(new Plane((0, 0, 1f), 5f, (230f / 255f, 230f / 255f, 0), 0));
            scene.primitives.Add(new Plane((0, -1f, 0), 5f, (200f / 255f, 200f / 255f, 0), 0.5f));
            scene.primitives.Add(new Plane((1f, 0, 0), 5f, (1f, 1f, 0), 0.5f));
            scene.primitives.Add(new Plane((-1f, 0, 0), 5f, (1f, 1f, 0), 0.5f));
            //scene.primitives.Add(new Plane((0, 0, -1f), 5f, (1f, 1f, 1f), 1f));

            scene.lightSources.Add(new Light((4f, 1f, 0f), (1f, 1f, 1f)));
            scene.lightSources.Add(new Light((-2f, 2f, -1f), (1f, 1f, 1f)));
            scene.lightSources.Add(new Light((0f, 4f, 4f), (1f, 1f, 1f)));
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Ray primaryRay = new Ray(camera.position, camera.RayDirection(i / (float)width, (j / (float)height))); //create a primaryRay through the screen plane to see if the pixel has a hit
                    foreach (Primitive primitive in scene.primitives)   //loop through all primitives
                    {
                        float Distance = primitive.Intersect(primaryRay);
                        if ((Distance < primaryRay.Distance) && (Distance > 0)) //if there is a collision and this collision is closer than the current ray distance it means that this is the first primitive to collide with the ray
                        {
                            primaryRay.Distance = Distance;
                            primaryRay.Intersection = new Intersection(Distance, primitive);
                        }
                    }
                    Vector3 IntersectionPoint = primaryRay.E + primaryRay.Distance * primaryRay.Direction;
                    if (primaryRay.Distance < 0 || primaryRay.Distance == 100) //does not have intersection, so no need to check shadow
                    {
                        screen.pixels[i + j * width] = 0;
                        continue;
                    }

                    RecursionCount = 0;
                    Vector3 FinalColor = RefColor(primaryRay); //The final color, With the function RefColor each pixel with a collision will loop. If the primitive is a mirror object than the reflection will be used.

                    screen.Plot(i, j, (int)(FinalColor.X * 255) + ((int)(FinalColor.Y * 255) << 8) + ((int)(FinalColor.Z * 255) << 16)); //translate to correct BGR
                }
            }
            DisplayCameraInfo();
            //Debug();
        }


        public void Debug()
        {
            //draw camera 3x3
            screen.pixels[screen.TX(camera.position.X) + screen.TY(camera.position.Z)*screen.width] = 255;
            screen.Box(screen.TX(camera.position.X) - 1, screen.TY(camera.position.Z) - 1, screen.TX(camera.position.X) + 1, screen.TY(camera.position.Z) + 1, 255);

            //draw screenplane
            screen.Line(screen.TX(camera.screenCorners[0].X), screen.TY(camera.screenCorners[0].Z), screen.TX(camera.screenCorners[1].X), screen.TY(camera.screenCorners[1].Z),255*255*255);


            //draw primitives
            /*for (int i = 0; i< scene.primitives.Count; i++)
            {
                Primitive p = scene.primitives[i];
                if (p = Sphere) dynamiccast
                {
                    for (int j = 0; j < 360; j++)
                    {
                        screen.pixels[screen.TX(p.Position.X + (p.Radius * (float)Math.Cos(j))) + screen.TY(p.Position.Z + (p.Radius * (float)Math.Sin(j))) * screen.width] = 255;
                    }
                }
            }*/

            for (float i = 0; i <= (float)screen.width + 0.1f; i += (float)screen.width / 40f)
            {
                Ray primaryRay = new Ray(camera.position, camera.RayDirection(i / (float)screen.width, 0.5f));
                foreach (Primitive primitive in scene.primitives)
                {
                    float Distance = primitive.Intersect(primaryRay);
                    if ((Distance < primaryRay.Distance) && (Distance > 0))
                    {
                        primaryRay.Distance = Distance;
                        primaryRay.Intersection = new Intersection(Distance, primitive);
                    }
                }

                Vector3 IntersectionPoint = primaryRay.E + primaryRay.Distance * primaryRay.Direction;
                screen.Line(screen.TX(primaryRay.E.X), screen.TY(primaryRay.E.Z), screen.TX(IntersectionPoint.X), screen.TY(IntersectionPoint.Z), 255 * 100);
                if (primaryRay.Distance < 0 || primaryRay.Distance == 100) //does not have intersection, so no need to check shadow
                {
                    continue;
                }

                foreach (Light light in scene.lightSources)
                {
                    Vector3 NormalLightDirection = Vector3.Normalize(IntersectionPoint - light.Position);
                    Ray shadowRay = new Ray(light.Position, NormalLightDirection);

                    foreach (Primitive primitive in scene.primitives)
                    {
                        float Distance = primitive.Intersect(shadowRay);
                        if ((Distance < shadowRay.Distance) && (Distance > 0))
                        {
                            shadowRay.Distance = Distance;
                            shadowRay.Intersection = new Intersection(Distance, primitive);
                        }
                    }

                    Vector3 sIntersectionPoint = shadowRay.E + shadowRay.Distance * shadowRay.Direction;
                    if (shadowRay.Distance > 0 && shadowRay.Distance > (IntersectionPoint - light.Position).Length-0.01f) {
                        screen.Line(screen.TX(shadowRay.E.X), screen.TY(shadowRay.E.Z), screen.TX(sIntersectionPoint.X), screen.TY(sIntersectionPoint.Z), 255 * 256);
                    }
                    else
                    {
                        screen.Line(screen.TX(shadowRay.E.X), screen.TY(shadowRay.E.Z), screen.TX(sIntersectionPoint.X), screen.TY(sIntersectionPoint.Z), 255 * 256 * 256);
                    }
                }
            }
        }

        public Vector3 RefColor(Ray ray)
        {
            RecursionCount++;
            if (RecursionCount > RecursionCap) return GetColor(ray);
            Primitive RayPrimitive = ray.Intersection.Primitive;
            Vector3 IntersectionPoint = ray.E + ray.Distance * ray.Direction;
            if (RayPrimitive.Specularity == 1f)
            {
                Vector3 Normal = RayPrimitive.IntersectToNorm(IntersectionPoint);
                Vector3 R = Vector3.Normalize(ray.Direction - 2 * Vector3.Dot(ray.Direction, Normal) * Normal);
                Ray reflectionRay = new Ray(IntersectionPoint, R);
                foreach (Primitive primitive in scene.primitives)   //loop through all primitives
                {
                    float Distance = primitive.Intersect(reflectionRay);
                    if (Distance < reflectionRay.Distance && Distance > 0) //if there is a collision and this collision is closer than the current ray distance it means that this is the first primitive to collide with the ray
                    {
                        reflectionRay.Distance = Distance;
                        reflectionRay.Intersection = new Intersection(Distance, primitive);
                    }
                }
                if (reflectionRay.Distance >= 50) //does not have intersection, so no need to check shadow
                {
                    return Vector3.Zero;
                }

                return RayPrimitive.Color * RefColor(reflectionRay); //return the Reflected color combined with the color of the primitive
            }
            else
            {
                return GetColor(ray); //if the primitve is not a mirror it will, use the getcolor function to calculate the correct color
            }
        }

        public Vector3 GetColor(Ray ray)
        {
            RecursionCount = 0;
            Vector3 IntersectionPoint = ray.E + ray.Distance * ray.Direction;
            Primitive RayPrimitive = ray.Intersection.Primitive;
            Vector3 BaseColor = RayPrimitive.Color * 0.08f; //Sets the Basecolor consisting of only the ambient color  
            
            foreach (Light light in scene.lightSources) //looping all the lights
            {
                bool InShadow = false;
                Vector3 NormalLightDirection = Vector3.Normalize(IntersectionPoint - light.Position); //create a normal from the light towards the intersection point to check if there is a primitive between the light and the intersection point
                Ray shadowRay = new Ray(light.Position, NormalLightDirection); //shadowRay to check this

                foreach (Primitive primitive in scene.primitives) //loop through all primitives
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
                    float SpecularCoefficient = RayPrimitive.Specularity; //current specular
                    Vector3 color = RayPrimitive.Color;
                    Vector3 Normal = RayPrimitive.IntersectToNorm(IntersectionPoint);


                    // Calculate the light contribution (diffuse and specular components)
                    Vector3 R = -NormalLightDirection - 2 * Vector3.Dot(-NormalLightDirection, Normal) * Normal;

                    float distance = (IntersectionPoint - light.Position).Length;
                    float attenuation = 1.0f / (0.6f+.5f*distance +0.5f*(distance*distance)); //Calculate the LightDistance

                    Vector3 Brightness = attenuation * light.Intensity; //Calculate the Brightness component
                    
                    float kd = MathF.Max(0, Vector3.Dot(Normal, -NormalLightDirection)); //Calculate the diffuse component
                    float ks = MathF.Max(0, MathF.Pow(Vector3.Dot(ray.Direction, R.Normalized()), 400f)); // Calculate the specular component

                    BaseColor += Brightness * color * kd + Brightness * SpecularCoefficient*ks;// Calculate final color
                }
                
            }
            return BaseColor;
        }
    }
}
