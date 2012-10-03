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
    class GameObject
    {
        public Model model = null;
        public Vector3 position = Vector3.Zero;
        public Quaternion rotation = Quaternion.Identity;
        public Vector3 offset = new Vector3(0.0f, 8.0f, 0.0f);
        public Vector3 offsetInverse = new Vector3(0.0f, -8.0f, 0.0f);
        public float scale = 1.0f;
    }
}
