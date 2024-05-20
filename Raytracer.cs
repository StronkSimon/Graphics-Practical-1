using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
            this.camera = new Camera((0,0,-4f),(0,0,1f),(0,1f,0),1.5f,1f);
            this.screen = screen;
            
        }

        public void Render()
        {
            scene.primitives.Add(new Sphere((0, 0, 1.0f), 1f, (1, 0, 0)));
            scene.primitives.Add(new Sphere((-2.0f, 0, 1.0f), 1f, (0, 1, 0)));
            scene.primitives.Add(new Sphere((2.0f, 0, 1.0f), 1f, (0, 0, 1)));
            Debug();
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
        }

    }
}
