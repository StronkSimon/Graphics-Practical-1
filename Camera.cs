using OpenTK.Mathematics;

namespace RayTracer
{
    public class Camera
    {
        // Position of the camera in 3D space.
        public Vector3 position { get; set; }

        // The direction the camera is looking at.
        public Vector3 lookAtDirection { get; set; }

        // The up direction vector for the camera, used for defining the camera's orientation.
        public Vector3 upDirection { get; set; }

        // Field of view of the camera in radians.
        public float fieldOfView { get; set; }

        // Aspect ratio of the camera's view (width/height).
        public float aspectRatio { get; set; }

        // Coordinates of the four corners of the camera's view screen in 3D space.
        public Vector3[] screenCorners { get; private set; }

        // Constructor to initialize the camera with specified properties.
        public Camera(Vector3 position, Vector3 lookAtDirection, Vector3 upDirection, float fieldOfView, float aspectRatio)
        {
            this.position = position;
            this.lookAtDirection = lookAtDirection;
            this.upDirection = upDirection;
            this.fieldOfView = fieldOfView;
            this.aspectRatio = aspectRatio;
            this.screenCorners = new Vector3[4];
            UpdateScreenCorners();
        }

        // Updates the coordinates of the screen corners based on the current camera configuration.
        private void UpdateScreenCorners()
        {
            float halfFov = (float)Math.Tan(fieldOfView / 2.0f);
            float screenHeight = halfFov * 2.0f;
            float screenWidth = screenHeight * aspectRatio;

            Vector3 forward = lookAtDirection.Normalized();
            Vector3 right = Vector3.Cross(upDirection, forward).Normalized();
            Vector3 up = Vector3.Cross(forward, right).Normalized();

            Vector3 center = position + forward;
            Vector3 topLeft = center + (up * screenHeight / 2.0f) - (right * screenWidth / 2.0f);
            Vector3 topRight = center + (up * screenHeight / 2.0f) + (right * screenWidth / 2.0f);
            Vector3 bottomLeft = center - (up * screenHeight / 2.0f) - (right * screenWidth / 2.0f);
            Vector3 bottomRight = center - (up * screenHeight / 2.0f) + (right * screenWidth / 2.0f);

            screenCorners[0] = topLeft;
            screenCorners[1] = topRight;
            screenCorners[2] = bottomLeft;
            screenCorners[3] = bottomRight;
        }

        // Computes the ray direction from the camera position to a point on the screen defined by parameters a and b.
        public Vector3 RayDirection(float a, float b)
        {
            Vector3 u = screenCorners[1] - screenCorners[0];
            Vector3 v = screenCorners[2] - screenCorners[0];
            Vector3 p = screenCorners[0] + a * u + b * v;
            Vector3 result = (p - position).Normalized();
            return result;
        }

        // Moves the camera forward in the direction it is facing by a given distance.
        public void MoveForward(float distance)
        {
            position += lookAtDirection.Normalized() * distance;
            UpdateScreenCorners();
        }

        // Moves the camera backward in the direction it is facing by a given distance.
        public void MoveBackward(float distance)
        {
            position -= lookAtDirection.Normalized() * distance;
            UpdateScreenCorners();
        }

        // Moves the camera to the right perpendicular to its look direction and up direction.
        public void MoveRight(float distance)
        {
            Vector3 right = Vector3.Cross(upDirection, lookAtDirection.Normalized()).Normalized();
            position += right * distance;
            UpdateScreenCorners();
        }

        // Moves the camera to the left perpendicular to its look direction and up direction.
        public void MoveLeft(float distance)
        {
            Vector3 left = -Vector3.Cross(upDirection, lookAtDirection.Normalized()).Normalized();
            position += left * distance;
            UpdateScreenCorners();
        }

        // Moves the camera upward relative to its current up direction.
        public void MoveUp(float distance)
        {
            position += upDirection.Normalized() * distance;
            UpdateScreenCorners();
        }

        // Moves the camera downward relative to its current up direction.
        public void MoveDown(float distance)
        {
            position -= upDirection.Normalized() * distance;
            UpdateScreenCorners();
        }

        // Increases the field of view by a given angle, respecting the upper limit of Pi radians.
        public void IncreaseFOV(float delta)
        {
            fieldOfView = Math.Min(fieldOfView + delta, (float)Math.PI);
            UpdateScreenCorners();
        }

        // Decreases the field of view by a given angle, respecting the lower limit of 0.1 radians.
        public void DecreaseFOV(float delta)
        {
            fieldOfView = Math.Max(fieldOfView - delta, 0.1f);
            UpdateScreenCorners();
        }
    }
}
