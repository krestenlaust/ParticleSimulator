using System;
using System.Numerics;

namespace ParticleEngine.Particles
{
    public class Sand : ParticleGroup
    {
        public Sand() : base(2,
            34f / 180f * (float)Math.PI //Pres til fysik implemention - 34 grader for sand
            )
        {

        }

        public override void OnCollide(Vector2 otherParticle, ParticleGroup otherParticleGroup, Vector2 particle)
        {
        }
    }
}