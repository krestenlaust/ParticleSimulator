using System;
using System.Numerics;

namespace ParticleEngine.Particles
{
    public class Water : ParticleGroup
    {
        public Water() : base(1, 89 / 180f * (float)Math.PI)
        {

        }

        public override void OnCollide(Vector2 otherParticle, ParticleGroup otherParticleGroup, Vector2 particle)
        {
        }
    }
}
