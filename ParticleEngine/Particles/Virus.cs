using System.Collections.Generic;
using System.Numerics;

namespace ParticleEngine.Particles
{
    public class Virus : ParticleGroup
    {
        public Virus() : base(1, 1)
        {

        }

        public override void OnCollide(Vector2 otherParticle, ParticleGroup otherParticleGroup, Vector2 particle)
        {
            if (otherParticleGroup is Virus || otherParticleGroup is Block || otherParticleGroup is Acid)
            {
                return;
            }

            otherParticleGroup.Particles.Remove(otherParticle);
            Particles.Add(otherParticle);
        }

        public override void OnUpdate(List<ParticleGroup> particleGroups)
        {

        }
    }
}
