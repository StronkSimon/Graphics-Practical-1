
﻿using System;
using OpenTK.Mathematics;

namespace RayTracer
{
    public class Camera
    {

        public Vector3 position { get; set; }
        public Vector3 lookAtDirection { get; set; }
        public Vector3 upDirection { get; set; }
        public float fieldOfView { get; set; }
        public float aspectRatio { get; set; }
        //public Plane screenPlane { get; set; }
        public Vector3[] screenCorners { get; private set; }

        public Camera(Vector3 position, Vector3 lookAtDirection, Vector3 upDirection, float fieldOfView, float aspectRatio)

        {
            this.position = position;
            this.lookAtDirection = lookAtDirection.Normalized();
            this.upDirection = upDirection.Normalized();
            this.fieldOfView = fieldOfView;
            this.aspectRatio = aspectRatio;
            this.screenCorners = new Vector3[4];
            UpdateScreenCorners();
        }

        private void UpdateScreenCorners()
        {
            float vertAngleRad = MathHelper.DegreesToRadians(fieldOfView);
            float horizAngleRad = MathHelper.DegreesToRadians(fieldOfView * aspectRatio);

            Vector3 forward = lookAtDirection.Normalized();
            Vector3 right = Vector3.Cross(forward, upDirection).Normalized();
            Vector3 up = Vector3.Cross(right, forward).Normalized();

            Vector3 center = position + forward * fieldOfView;
            Vector3 topLeft = center + up - aspectRatio * right;
            Vector3 topRight = center + up + aspectRatio * right;
            Vector3 bottomLeft = center - up - aspectRatio * right;
            Vector3 bottomRight = center - up + aspectRatio * right;

            screenCorners[0] = topLeft;
            screenCorners[1] = topRight;
            screenCorners[2] = bottomLeft;
            screenCorners[3] = bottomRight;
            
        }
    }
}