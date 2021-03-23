using System;
using System.Collections.Generic;
using System.Numerics;

namespace ParticleEngine.Particles
{
    public class Water : ParticleGroup
    {
        public Water() : base(1, 89.9f / 180f * (float)Math.PI)
        {

        }

        public override void OnCollide(Vector2 otherParticle, ParticleGroup otherParticleGroup, Vector2 particle)
        {
        }

        public override void OnUpdate(List<ParticleGroup> particleGroups)
        {
        }
    }
}
