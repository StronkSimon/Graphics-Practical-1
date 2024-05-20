using OpenTK.Mathematics;

namespace RayTracer
{
    public class Camera
    {
        public Vector3 Position { get; set; }
        public Vector3 LookAtDirection { get; set; }
        public Vector3 UpDirection { get; set; }
        public float FieldOfView { get; set; }
        public float AspectRatio { get; set; }
        public float NearPlane { get; set; }
        public Vector3[] ScreenCorners { get; private set; }

        public Camera(Vector3 position, Vector3 lookAtDirection, Vector3 upDirection, float fieldOfView, float aspectRatio, float nearPlane)
        {
            Position = position;
            LookAtDirection = lookAtDirection.Normalized();
            UpDirection = upDirection.Normalized();
            FieldOfView = fieldOfView;
            AspectRatio = aspectRatio;
            NearPlane = nearPlane;
            ScreenCorners = new Vector3[4];
            UpdateScreenCorners();
        }

        private void UpdateScreenCorners()
        {
            float vertAngleRad = MathHelper.DegreesToRadians(FieldOfView);
            float horizAngleRad = MathHelper.DegreesToRadians(FieldOfView * AspectRatio);

            float halfHeight = NearPlane * MathF.Tan(vertAngleRad / 2);
            float halfWidth = NearPlane * MathF.Tan(horizAngleRad / 2);

            Vector3 forward = LookAtDirection.Normalized();
            Vector3 right = Vector3.Cross(forward, UpDirection).Normalized();
            Vector3 up = Vector3.Cross(right, forward).Normalized();

            Vector3 center = Position + forward * NearPlane;
            Vector3 topLeft = center + up * halfHeight - right * halfWidth;
            Vector3 topRight = center + up * halfHeight + right * halfWidth;
            Vector3 bottomLeft = center - up * halfHeight - right * halfWidth;
            Vector3 bottomRight = center - up * halfHeight + right * halfWidth;

            ScreenCorners[0] = topLeft;
            ScreenCorners[1] = topRight;
            ScreenCorners[2] = bottomLeft;
            ScreenCorners[3] = bottomRight;
        }
    }
}