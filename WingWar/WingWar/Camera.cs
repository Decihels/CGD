using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace WingWar
{
    class Camera
    {
        public Matrix viewMatrix; public Matrix projectionMatrix; Matrix cameraRotation = Matrix.Identity;

        public void InitializeCamera(float view, Vector3 position)
        {
            viewMatrix = Matrix.CreateLookAt(new Vector3(0f, 6f, 20f), position, cameraRotation.Up);

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
                view, 1.0f, 10000.0f);
        }

        public void UpdateCamera(float view, Vector3 position, Quaternion rotation)
        {
            Vector3 camPos = new Vector3(0, 6f, 20f);

            camPos = Vector3.Transform(camPos, Matrix.CreateFromQuaternion(rotation));

            camPos += position;

            Vector3 camUp = new Vector3(0, 1, 0);

            camUp = Vector3.Transform(camUp, Matrix.CreateFromQuaternion(rotation));

            viewMatrix = Matrix.CreateLookAt(camPos, position, camUp);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                view, 1.0f, 10000.0f);
        }
    }
}
