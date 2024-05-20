using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
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
            for (int i = 0; i < screen.width; i++)
            {
                for (int j = 0; j < screen.height; j++)
                {
                    Ray ray = new Ray(camera.position, camera.RayDirection((float)i / (float)screen.width, (float)j / (float)screen.height), scene);
                    if (ray.t > 10)
                    {
                        screen.pixels[i + j * screen.width] = 0;
                    }
                    else
                    {
                        screen.pixels[i + j * screen.width] = 255 - (int)(255f * ((ray.t-3f) / 6)) + (255 - (int)(255f * ((ray.t-3f) / 6))) * 256 + (255 - (int)(255f * ((ray.t-3f) / 6))) * 256 * 256;
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

            for(float i = 0; i <= (float)screen.width+0.1f; i+= (float)screen.width/10f) //random nulletjse erbij
            {
                Ray ray = new Ray(camera.position, camera.RayDirection(i / (float)screen.width, 0.5f), scene);
                screen.Line(screen.TX(ray.E.X), screen.TY(ray.E.Z), screen.TX(ray.ray.X), screen.TY(ray.ray.Z), 255 * 100);
            }
        }
    }
}
