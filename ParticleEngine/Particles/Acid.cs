using System;
using System.Collections.Generic;
using System.Numerics;

namespace ParticleEngine.Particles
{
    public class Acid : ParticleGroup
    {
        public Acid() : base(
            1,
            34f / 180f * (float)Math.PI)
        {

        }

        public override void OnCollide(Vector2 otherParticle, ParticleGroup otherParticleGroup, Vector2 particle)
        {
            if (otherParticleGroup is Acid || otherParticleGroup is Block)
                return;

            otherParticleGroup.Particles.Remove(otherParticle);
            Particles.Remove(particle);
        }
    }
}
