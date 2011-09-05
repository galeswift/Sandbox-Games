using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Sandbox_Games
{
    public class Camera_Base : GameComponent
    {
        protected Vector3 _position;
        protected Vector3 _lookAt;
        protected Vector3 _up;
        protected Matrix _viewMatrix;
        protected Matrix _projectionMatrix;
        
        private float _aspectRatio;

        public Camera_Base(Game inGame, Viewport viewport) : 
            base(inGame)
        {
            _aspectRatio = viewport.AspectRatio;
            _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                                        MathHelper.PiOver4 * inGame.GraphicsDevice.Viewport.AspectRatio * 3 / 4,
                                        _aspectRatio,
                                        0.1f,
                                        1000.0f);

            _up = Vector3.Up;            
        }

        public virtual Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public virtual Vector3 LookAt
        {
            get { return _lookAt; }
            set { _lookAt = value; }
        }
        public virtual Vector3 Up
        {
            get { return _up; }
            set { _up = value; }
        }
        public Matrix ViewMatrix
        {
            get { return _viewMatrix; }
        }
        public Matrix ProjectionMatrix
        {
            get { return _projectionMatrix; }
        }
        public Matrix ViewProjectionMatrix
        {
            get { return ViewMatrix * ProjectionMatrix; }
        }
        public override void Update( GameTime gameTime )
        {
            base.Update(gameTime);

            CalcViewMatrix(gameTime);
        }

        protected virtual void CalcViewMatrix(GameTime gameTime)
        {
            _viewMatrix = Matrix.CreateLookAt(_position, _lookAt, _up);
        }
    }
}
