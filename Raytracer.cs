using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace RayTracer
{
    public class Raytracer
    {
        public Scene scene;
        public Camera camera;
        public Surface screen;

        public Raytracer(Surface screen)
        {
            this.scene = new Scene();
            this.camera = new Camera((0,0,-4f),(0,0,1f),(0,1f,0),1.4f,(float)screen.width/(float)screen.height); //position-lookDirection-upDirection-Fov-aspectRatio
            this.screen = screen;
            
        }

        public void Render()
        {
            int width = screen.width;
            int height = screen.height;
            scene.primitives.Add(new Sphere((0, 0, 1.0f), 1.2f, (255, 0, 0)));
            scene.primitives.Add(new Sphere((-2.5f, 2f, 2.0f), 1.2f, (0, 255, 0)));
            scene.primitives.Add(new Sphere((2.5f, 0, 3.0f), 1.2f, (0, 0, 255)));
            scene.primitives.Add(new Plane((0,0,1f),5f, (230, 230, 0)));
            scene.primitives.Add(new Plane((0, -1f,0), 5f, (200, 200, 0)));
            scene.primitives.Add(new Plane((1f, 0, 0), 5f, (255, 255, 0)));
            scene.primitives.Add(new Plane((-1f, 0, 0), 5f, (255, 255, 0)));

            scene.lightSources.Add(new Light((3f, 3f, 0f), (255, 255, 255)));
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Ray primaryRay = new Ray(camera.position, camera.RayDirection(i / (float)width, (j / (float)height)));
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
                    if (primaryRay.Distance < 0 || primaryRay.Distance == 100) //does not have intersection, so no need to check shadow
                    {
                        screen.pixels[i + j * width] = 0;
                        continue;
                    }

                    foreach (Light light in scene.lightSources)
                    {
                        Vector3 NormalLightDirection = (IntersectionPoint - light.Position).Normalized();
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
                        if (shadowRay.Distance > 0 && shadowRay.Distance > (IntersectionPoint - light.Position).Length - 0.01f)
                        {
                            Vector3 color = primaryRay.Intersection.Primitive.Color;
                            screen.pixels[i + j * screen.width] = (int)color.X + (int)color.Y * 256 + (int)color.Z * 256 * 256;
                        }
                        else
                        {
                            Vector3 color = primaryRay.Intersection.Primitive.Color;
                            screen.pixels[i + j * screen.width] = (int)color.X / 255 * 60 + ((int)color.Y / 255 * 60) * 256 + ((int)color.Z / 255 * 60) * 256 * 256;
                        }
                    }
                }

            }
            // Debug();
        }

        public void Debug()
        {
            //draw camera 3x3
            screen.pixels[screen.TX(camera.position.X) + screen.TY(camera.position.Z)*screen.width] = 255;
            screen.Box(screen.TX(camera.position.X) - 1, screen.TY(camera.position.Z) - 1, screen.TX(camera.position.X) + 1, screen.TY(camera.position.Z) + 1, 255);

            //draw screenplane
            screen.Line(screen.TX(camera.screenCorners[0].X), screen.TY(camera.screenCorners[0].Z), screen.TX(camera.screenCorners[1].X), screen.TY(camera.screenCorners[1].Z),255*255*255);


            //draw primitives
           /* foreach (Primitive p in scene.primitives)
            {
                if (p = Sphere)
                {
                    for (int i = 0; i < 360; i++)
                    {
                        screen.pixels[screen.TX(p.Position.X + (p.Radius * (float)Math.Cos(i))) + screen.TY(p.Position.Z + (p.Radius * (float)Math.Sin(i))) * screen.width] = 255;
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
                    Vector3 NormalLightDirection = (IntersectionPoint - light.Position).Normalized();
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
    }
}
