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
    public class Particle
    {
        public Vector2 velocity { get; set; }
        public Vector2 maxVelocity { get; set; }
        public Vector2 position { get; set; }
        public Vector2 acceleration { get; set; }
        public Vector2 gravAcceleration { get; set; }
        public Vector2 maxAcceleration { get; set; }
        public Texture2D mySprite { get; set; }
        public Rectangle myRectangle;
        public string mySpritePath = "Sprites\\Circle";

        ParticleGenerator owner;

        public Particle(ParticleGenerator inOwner)
        {
            owner = inOwner;
            position = inOwner.position;
            velocity = new Vector2((inOwner.myGame.randomizer.Next(0, 50)) * ((inOwner.myGame.randomizer.Next(2) < 1) ? -1 : 1),
                                    (inOwner.myGame.randomizer.Next(0, 50)) * ((inOwner.myGame.randomizer.Next(2) < 1) ? -1 : 1));

            maxVelocity = new Vector2(300, 300);
            gravAcceleration = new Vector2(0, 250);
            maxAcceleration = new Vector2(300, 300);
            // use the owner to load our sprite
            mySprite = owner.myGame.Content.Load<Texture2D>(mySpritePath);
            myRectangle = new Rectangle((int)position.X, (int)position.Y, 24, 24);
        }

        public virtual void Update(GameTime gameTime)
        {
            velocity += (gravAcceleration + acceleration) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            myRectangle.X = (int)position.X;
            myRectangle.Y = (int)position.Y;

            // Bounce
            float myBounce = (float)(owner.myGame.randomizer.NextDouble()) * 0.2f + 0.5f;
            if (position.X + myRectangle.Width > owner.myGame.GraphicsDevice.Viewport.Width)
            {
                position = new Vector2(owner.myGame.GraphicsDevice.Viewport.Width - myRectangle.Width, position.Y);
                velocity = new Vector2(velocity.X * -myBounce, velocity.Y);
            }
            else if (position.X - myRectangle.Width < 0)
            {
                position = new Vector2(myRectangle.Width, position.Y);
                velocity = new Vector2(velocity.X * -myBounce, velocity.Y);
            }

            if (position.Y + myRectangle.Height > owner.myGame.GraphicsDevice.Viewport.Height)
            {
                position = new Vector2(position.X, owner.myGame.GraphicsDevice.Viewport.Height - myRectangle.Height);
                velocity = new Vector2(velocity.X, velocity.Y * -myBounce);
            }
            else if (position.Y - myRectangle.Height < 0)
            {
                position = new Vector2(position.X, myRectangle.Height);
                velocity = new Vector2(velocity.X, velocity.Y * -myBounce);
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            Color myColor = Color.White;
            byte newColorValue = (byte)(255 - 255 * (velocity / maxVelocity).Length());
            myColor.R = newColorValue;
            myColor.B = newColorValue;
            if (owner != null)
            {
                owner.myGame.spriteBatch.Draw(mySprite, myRectangle, myColor);
            }
        }
    }


    public class ParticleModifier : DrawableGameComponent
    {
        public BaseGame myGame;
        public Vector2 position;
        public float force;
        public float drawRadius = 24.0f;
        public float drawScale = 1.0f;

        public ParticleModifier(BaseGame game, Vector2 inPosition) :
            base(game)
        {
            position = inPosition;
            myGame = game;
        }
    }

    public class ParticleGenerator : ParticleModifier
    {
        List<Particle> particleList;

        public ParticleGenerator(BaseGame game, Vector2 inPosition) :
            base(game, inPosition)
        {
            particleList = new List<Particle>();
        }

        public void RemoveForce(float inForce, Vector2 inPosition)
        {
            AddForce(-inForce, inPosition);
        }

        public void AddForce(float inForce, Vector2 inPosition)
        {
            for (int i = 0; i < particleList.Count; i++)
            {
                particleList[i].velocity -= Vector2.Normalize(inPosition - particleList[i].position) * inForce * (1 / Math.Max(Vector2.Distance(particleList[i].position, inPosition), 1.0f));
            }
        }

        public void SpawnParticles(int num)
        {
            for (int i = 0; i < num; i++)
            {
                Particle newParticle = new Particle(this);
                particleList.Add(newParticle);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < particleList.Count; i++)
            {
                particleList[i].Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            myGame.spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            for (int i = 0; i < particleList.Count; i++)
            {
                particleList[i].Draw(gameTime);
            }
            myGame.spriteBatch.End();
        }
    }

    public class ParticleAttractor : ParticleModifier
    {
        Texture2D mySprite;

        public ParticleAttractor(BaseGame game, Vector2 inPosition) :
            base(game, inPosition)
        {
            mySprite = game.Content.Load<Texture2D>("Sprites\\Circle");
            drawScale = drawRadius / mySprite.Width;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (myGame != null)
            {
                for (int i = 0; i < myGame.partGenList.Count; i++)
                {
                    myGame.partGenList[i].RemoveForce(force, position);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            myGame.spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            myGame.spriteBatch.Draw(mySprite, position, new Rectangle(0, 0, mySprite.Width, mySprite.Height), Color.Red, 0.0f, new Vector2(mySprite.Width/2.0f, mySprite.Height/2.0f), drawScale,SpriteEffects.None,0.0f);
            myGame.spriteBatch.End();
        }
    }

    public class ParticleRepulsor : ParticleModifier
    {


        Texture2D mySprite;

        public ParticleRepulsor(BaseGame game, Vector2 inPosition) :
            base(game, inPosition)
        {
            mySprite = game.Content.Load<Texture2D>("Sprites\\Circle");
            drawScale = drawRadius / mySprite.Width;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (myGame != null)
            {
                for (int i = 0; i < myGame.partGenList.Count; i++)
                {
                    myGame.partGenList[i].AddForce(force, position);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            myGame.spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            myGame.spriteBatch.Draw(mySprite, position, new Rectangle(0, 0, mySprite.Width, mySprite.Height), Color.Blue, 0.0f, new Vector2(mySprite.Width / 2.0f, mySprite.Height / 2.0f), drawScale, SpriteEffects.None, 0.0f);
            myGame.spriteBatch.End();
        }
    }
}
