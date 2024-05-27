using OpenTK.Graphics.OpenGL;

namespace RayTracer
{
    public class MyApplication
    {
        // member variables
        public Raytracer raytracer;
        // constructor
        public MyApplication(Surface screen)
        {
            this.raytracer = new Raytracer(screen);
        }
        // initialize
        public void Init()
        {
            //raytracer.Render();
        }
        // tick: renders one frame
        public void Tick()
        {
            raytracer.Render();
        }
    }
}