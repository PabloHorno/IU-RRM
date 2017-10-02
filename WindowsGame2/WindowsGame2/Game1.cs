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

namespace VentanaRender
{
    public class Render : Microsoft.Xna.Framework.Game
    {
        private Model modeloMano;
        private Vector3 posicion = Vector3.One;
        private float zoom = 200.0f;
        private Vector3 rotacion = Vector3.Zero;
        private Matrix simuladorWorldRotation;
        private float velocidad = 10.0f;
        SpriteFont fuente1;
        SpriteBatch spriteBatch;
        List<Matrix> peta = new List<Matrix>();

        GraphicsDeviceManager graphics;

        public Render()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "Ventana de Renderizacion Mano";
        }
        protected override void Initialize()
        {
            
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            modeloMano = this.Content.Load<Model>("3D hand");
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            fuente1 = this.Content.Load<SpriteFont>("Courier New");
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
                velocidad += 10f;
            if (keyboardState.IsKeyDown(Keys.Subtract))
                velocidad -= 10f;
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
            if (keyboardState.IsKeyDown(Keys.J))
                rotacion = new Vector3( 90, 90, 90);

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

            /*spriteBatch.Begin();
            spriteBatch.DrawString(fuente1, rotacion.ToString() + velocidad, new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2), Color.Black,
                0.0f, fuente1.MeasureString(rotacion.ToString()) / 2, 1, SpriteEffects.None, 0.5f);
            spriteBatch.End();*/
                
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
