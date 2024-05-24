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
            this.camera = new Camera((0,0,-4f),(0,0,1f),(0,1f,0),1.4f,1f);
            this.screen = screen;
            
        }

        public void Render()
        {
            scene.primitives.Add(new Sphere((0, 0, 1.0f), 1.2f, (1, 0, 0)));
            scene.primitives.Add(new Sphere((-2.5f, 0, 2.0f), 1.2f, (0, 1, 0)));
            scene.primitives.Add(new Sphere((2.5f, 0, 3.0f), 1.2f, (0, 0, 1)));
            scene.lightSources.Add(new Light((3f, 0f, 0f), (255, 255, 255)));
            for (int i = 0; i < screen.width; i++)
            {
                for (int j = 0; j < screen.height; j++)
                {
                    Ray primaryRay = new Ray(camera.position, camera.RayDirection(i / (float)screen.width, j / (float)screen.height));
                    foreach (Sphere sphere in scene.primitives)
                    {
                        float Distance = sphere.Intersect(primaryRay);
                        if ((Distance < primaryRay.Distance) && (Distance > 0))
                        {
                            primaryRay.Distance = Distance;
                            primaryRay.Intersection = new Intersection(Distance, sphere);
                        }
                    }


                    Vector3 IntersectionPoint = primaryRay.E + primaryRay.Distance * primaryRay.Direction;
                    if (primaryRay.Distance < 0 || primaryRay.Distance == 100) //does not have intersection, so no need to check shadow
                    {
                        screen.pixels[i + j * screen.width] = 0;
                        continue;
                    }

                    foreach (Light light in scene.lightSources)
                    {
                        Vector3 NormalLightDirection = (IntersectionPoint - light.Position).Normalized();
                        Ray shadowRay = new Ray(light.Position, NormalLightDirection);

                        foreach (Sphere sphere in scene.primitives)
                        {
                            float Distance = sphere.Intersect(shadowRay);
                            if ((Distance < shadowRay.Distance && Distance > 0))
                            {
                                shadowRay.Distance = Distance;
                                shadowRay.Intersection = new Intersection(Distance, sphere);
                            }

                        }

                        Vector3 sIntersectionPoint = shadowRay.E + shadowRay.Distance * shadowRay.Direction;
                        if (shadowRay.Distance > 0 && shadowRay.Distance > (IntersectionPoint - light.Position).Length - 0.01f)
                        {
                            screen.pixels[i + j * screen.width] = 255 + 255 * 256 + 255 * 256 * 256;
                        }
                        else
                        {
                            screen.pixels[i + j * screen.width] = 60 + 60 * 256 + 60 * 256 * 256;
                        }
                    }
                }

            }
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
            foreach (Sphere p in scene.primitives)
            {
                for(int i = 0; i < 360; i++)
                {
                    screen.pixels[screen.TX(p.Position.X + (p.Radius * (float)Math.Cos(i))) + screen.TY(p.Position.Z + (p.Radius * (float)Math.Sin(i)))*screen.width] = 255;
                }
            }

            /*for(float i = 0; i <= (float)screen.width+0.1f; i+= (float)screen.width/20f) //random nulletjse erbij
            {
                Ray primaryRay = new Ray(camera.position, camera.RayDirection(i / (float)screen.width, 0.5f), scene);
                Ray shadowRay = new Ray(primaryRay.ray, primaryRay.ray.Normalized(), scene);
                screen.Line(screen.TX(ray.E.X), screen.TY(ray.E.Z), screen.TX(ray.ray.X), screen.TY(ray.ray.Z), 255 * 100);
                if (ray.hit == true)
                {
                    screen.Line(screen.TX(primaryRay.ray.X), screen.TY(primaryRay.ray.Z), screen.TX(-primaryRay.ray.X), screen.TY(-2f - primaryray.ray.Z), 255 * 256);
                }
                else
                {
                    screen.Line(screen.TX(ray.ray.X), screen.TY(ray.ray.Z), screen.TX(-ray.ray.X), screen.TY(-2f - ray.ray.Z), 255 * 256*256);

                }
            }*/

            for (float i = 2* (float)screen.width / 40f; i <= (float)screen.width + 0.1f; i += (float)screen.width / 40f)
            {
                Ray primaryRay = new Ray(camera.position, camera.RayDirection(i / (float)screen.width, 0.5f));
                foreach (Sphere sphere in scene.primitives)
                {
                    float Distance = sphere.Intersect(primaryRay);
                    if ((Distance < primaryRay.Distance) && (Distance > 0))
                    {
                        primaryRay.Distance = Distance;
                        primaryRay.Intersection = new Intersection(Distance, sphere);
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

                    foreach (Sphere sphere in scene.primitives)
                    {
                        float Distance = sphere.Intersect(shadowRay);
                        if ((Distance < shadowRay.Distance && Distance > 0))
                        {
                            shadowRay.Distance = Distance;
                            shadowRay.Intersection = new Intersection(Distance, sphere);
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
