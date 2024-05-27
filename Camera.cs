
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
            this.lookAtDirection = lookAtDirection;
            this.upDirection = upDirection;
            this.fieldOfView = fieldOfView;
            this.aspectRatio = aspectRatio;
            this.screenCorners = new Vector3[4];
            UpdateScreenCorners();
        }

        private void UpdateScreenCorners()
        {

            Vector3 forward = lookAtDirection.Normalized();
            Vector3 right = Vector3.Cross(upDirection, forward).Normalized();
            Vector3 up = Vector3.Cross(forward, right).Normalized();

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
        public Vector3 RayDirection(float a, float b)
        {
            Vector3 u = screenCorners[1] - screenCorners[0];
            Vector3 v = screenCorners[2] - screenCorners[0];
            Vector3 p = screenCorners[0] + a * u + b * v;
            Vector3 result = (p - position).Normalized();
            return result;

        }

        public void MoveForward(float distance)
        {
            position += lookAtDirection.Normalized() * distance;
            UpdateScreenCorners();
        }

        public void MoveBackward(float distance)
        {
            position -= lookAtDirection.Normalized() * distance;
            UpdateScreenCorners();
        }

        public void MoveRight(float distance)
        {
            Vector3 right = Vector3.Cross(upDirection, lookAtDirection.Normalized()).Normalized();
            position += right * distance;
            UpdateScreenCorners();
        }

        public void MoveLeft(float distance)
        {
            Vector3 left = -Vector3.Cross(upDirection, lookAtDirection.Normalized()).Normalized();
            position += left * distance;
            UpdateScreenCorners();
        }

        public void IncreaseFOV(float delta)
        {
            fieldOfView += delta;
            UpdateScreenCorners();
        }

        public void DecreaseFOV(float delta)
        {
            fieldOfView -= delta;
            UpdateScreenCorners();
        }
    }
}