#region File Description
//-----------------------------------------------------------------------------
// ExplosionSmokeParticleSystem.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion


namespace Sandbox_Games
{
    /// <summary>
    /// ExplosionSmokeParticleSystem is a specialization of ParticleSystem which
    /// creates a circular pattern of smoke. It should be combined with
    /// ExplosionParticleSystem for best effect.
    /// </summary>
    public class ExplosionSmokeParticleSystem : ParticleSystem
    {
        public ExplosionSmokeParticleSystem(Game_Base game, int howManyEffects)
            : base(game, howManyEffects)
        {            
        }

        /// <summary>
        /// Set up the constants that will give this particle system its behavior and
        /// properties.
        /// </summary>
        protected override void InitializeConstants()
        {
            textureFilename = "Sprites\\smoke";

            // less initial speed than the explosion itself
            minInitialSpeed = 2;
            maxInitialSpeed = 20;

            // acceleration is negative, so particles will accelerate away from the
            // initial velocity.  this will make them slow down, as if from wind
            // resistance. we want the smoke to linger a bit and feel wispy, though,
            // so we don't stop them completely like we do ExplosionParticleSystem
            // particles.
            minAcceleration = -1;
            maxAcceleration = -5;

            // explosion smoke lasts for longer than the explosion itself, but not
            // as long as the plumes do.
            minLifetime = 1.0f;
            maxLifetime = 2.5f;

            minScale = .10f;
            maxScale = .20f;

            // we need to reduce the number of particles on Windows Phone in order to keep
            // a good framerate
#if WINDOWS_PHONE
            minNumParticles = 5;
            maxNumParticles = 10;
#else
            minNumParticles = 10;
            maxNumParticles = 20;
#endif

            minRotationSpeed = -MathHelper.PiOver4;
            maxRotationSpeed = MathHelper.PiOver4;

			blendState = BlendState.AlphaBlend;

            DrawOrder = AlphaBlendDrawOrder;
        }
    }
}
