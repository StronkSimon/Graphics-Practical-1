namespace RayTracer
{
    public class MyApplication
    {
        // member variables
        //public Surface screen;
        public Raytracer raytracer;
        // constructor
        public MyApplication(Surface screen)
        {
            //this.screen = screen;
            this.raytracer = new Raytracer(screen);
        }
        // initialize
        public void Init()
        {

        }
        // tick: renders one frame
        public void Tick()
        {
            raytracer.Render();
        }
    }
}