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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameObject groundPlane = new GameObject(new Vector3(0,0,0), new Quaternion(0, 0, 0, 1), 1.0f);
        GameObject jet = new GameObject(new Vector3(0, 1, 0), new Quaternion(0, 0, 0, 1), 1.0f);

        Camera player1Cam = new Camera();

        float moveSpeed = 0.3f, turningSpeed = 0.01f, yaw = 0, pitch = 0, roll = 0;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            groundPlane.model = Content.Load<Model>("Models//plane");
            jet.model = Content.Load<Model>("Models//jet");

            float viewPort = (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height;

            player1Cam.InitializeCamera(viewPort, jet.position);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here

            float viewPort = (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height;

            player1Cam.UpdateCamera(viewPort, jet.position, jet.rotation);

            Controls(ref jet.position, jet.rotation, moveSpeed);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSalmon);

            // TODO: Add your drawing code here

            DrawGameObject(groundPlane); DrawGameObject(jet);

            base.Draw(gameTime);
        }

        void DrawGameObject(GameObject gameobject)
        {
            foreach (ModelMesh mesh in gameobject.model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    //See below standard translate,rotate, translate.
                    effect.World = //Matrix.CreateTranslation(gameobject.offset) *

                        Matrix.CreateFromQuaternion(gameobject.rotation) *

                        Matrix.CreateScale(gameobject.scale) *

                       // Matrix.CreateTranslation(gameobject.offsetInverse) *

                        Matrix.CreateTranslation(gameobject.position);


                    effect.Projection = player1Cam.projectionMatrix;
                    effect.View = player1Cam.viewMatrix;
                }

                mesh.Draw();
            }
        }

        public void Controls(ref Vector3 position, Quaternion rotationQuat, float speed)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            //Velocity
            //Z
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                Vector3 addVector = Vector3.Transform(new Vector3(0, 0, -1), rotationQuat);
                position += addVector * speed;
            }

            //Rotation
            //Yaw
            if (keyboardState.IsKeyDown(Keys.Left)) { yaw += turningSpeed; }
            if (keyboardState.IsKeyDown(Keys.Right)) { yaw -= turningSpeed; }

            //Pitch
            if (keyboardState.IsKeyDown(Keys.Down)) { pitch -= turningSpeed; }
            if (keyboardState.IsKeyDown(Keys.Up)) { pitch += turningSpeed; }

            //Roll
            if (keyboardState.IsKeyDown(Keys.A)) { roll += turningSpeed; }
            if (keyboardState.IsKeyDown(Keys.D)) { roll -= turningSpeed; }

            //Build Rotation Quaternion
            Quaternion additionalRot =
                  Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), yaw)
                * Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), pitch)
                * Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), roll);

            //Multiply it into the jet's rotation
            jet.rotation *= additionalRot;

            yaw = 0;
            pitch = 0;
            roll = 0;
        }
    }
}