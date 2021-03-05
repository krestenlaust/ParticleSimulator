using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ParticleEngine.Particles
{
    public class AcidPowder : ParticleGroup
    {
        public AcidPowder() : base(
            1, 
            34f / 180f * (float)Math.PI
            )
        {

        }

        public override void OnCollide(Vector2 otherParticle, ParticleGroup otherParticleGroup, Vector2 particle)
        {
            if (otherParticleGroup is Block || otherParticleGroup is AcidPowder)
            {
                return;
            }

            otherParticleGroup.Particles.Remove(otherParticle);
            Particles.Remove(particle);
        }

        public override void OnUpdate(List<ParticleGroup> particleGroups)
        {

        }
    }
}
