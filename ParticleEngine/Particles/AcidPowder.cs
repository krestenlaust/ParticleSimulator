using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ParticleEngine.Particles
{
    public class AcidPowder : ParticleGroup
    {
        public AcidPowder()
        {
            Particles = new List<Vector2>();
            Mass = 1;
            AngleOfReposeRad = 34f / 180f * (float)Math.PI; //Pres til fysik implemention - 34 grader for sand
        }

        public override void DoPhysics()
        {
            
        }

        public override void OnCollide(Vector2 otherParticle, ParticleGroup otherParticleGroup, Vector2 particle)
        {
            if (otherParticleGroup is Block || otherParticleGroup is AcidPowder)
            {
                return;
            }

            otherParticleGroup.Particles.Remove(otherParticle);
        }
    }
}
