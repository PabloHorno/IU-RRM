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

namespace WindowsGame2
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private Model modeloMano;
        private Vector3 posicion = Vector3.One;
        private float zoom = 200.0f;
        private Vector3 rotacion = Vector3.Zero;
        private Matrix simuladorWorldRotation;
        private float velocidad = 10.0f;

        GraphicsDeviceManager graphics;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            modeloMano = this.Content.Load<Model>("3D hand");
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {

            KeyboardState keyboardState = Keyboard.GetState(PlayerIndex.One);
            if (keyboardState.IsKeyDown(Keys.Escape))
                this.Exit();
            //rotacion += Vector3.One;
            if (keyboardState.IsKeyDown(Keys.Add))
                velocidad += 0.1f;
            if (keyboardState.IsKeyDown(Keys.Subtract))
                velocidad -= 0.1f;
            if (keyboardState.IsKeyDown(Keys.W))
                rotacion.X += velocidad * 0.1f;
            if (keyboardState.IsKeyDown(Keys.S))
                rotacion.X -= velocidad*0.1f;
            if (keyboardState.IsKeyDown(Keys.A))
                rotacion.Y += velocidad * 0.1f;
            if (keyboardState.IsKeyDown(Keys.D))
                rotacion.Y -= velocidad * 0.1f;
            if (keyboardState.IsKeyDown(Keys.Q))
                rotacion.Z += velocidad * 0.1f;
            if (keyboardState.IsKeyDown(Keys.E))
                rotacion.Z -= velocidad * 0.1f;
            if (keyboardState.IsKeyDown(Keys.H))
                rotacion = Vector3.Zero;

            simuladorWorldRotation =
                Matrix.CreateRotationX(MathHelper.ToRadians(rotacion.X)) *
                Matrix.CreateRotationY(MathHelper.ToRadians(rotacion.Y)) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(rotacion.Z));

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.DarkGreen);

            DrawModel(modeloMano);

            base.Draw(gameTime);
        }
        private void DrawModel(Model m)
        {
            Matrix[] transforms = new Matrix[m.Bones.Count];
            float aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            m.CopyAbsoluteBoneTransformsTo(transforms);
            Matrix projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60.0f),
                aspectRatio, 1.0f, 10000.0f);
            Matrix view = Matrix.CreateLookAt(new Vector3(10.0f, 0.0f, zoom),
                Vector3.Zero, Vector3.Up);

            foreach (ModelMesh mesh in m.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = view;
                    effect.Projection = projection;
                    effect.World = simuladorWorldRotation *
                        transforms[mesh.ParentBone.Index] *
                        Matrix.CreateTranslation(Vector3.Zero);
                }
                mesh.Draw();
            }
        }
    }
}
