using System;
using System.Collections.Generic;
using System.Linq;
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
            this.camera = new Camera((0,0,0),(0,0,1),(0,1,0),1,1,1);
            this.screen = screen;

        }

        public void Render()
        {

        }

    }
}
