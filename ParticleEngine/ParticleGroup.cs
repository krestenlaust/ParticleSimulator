using System.Collections.Generic;
using System.Numerics;

namespace ParticleEngine
{
    public abstract class ParticleGroup
    {
        public readonly int Mass;
        public readonly float AngleOfReposeRad;
        public readonly List<Vector2> Particles;

        public ParticleGroup(int mass, float angleOfReposeRad)
        {
            Particles = new List<Vector2>();
            Mass = mass;
            AngleOfReposeRad = angleOfReposeRad;
        }

        public abstract void OnUpdate(List<ParticleGroup> particleGroups);

        public abstract void OnCollide(Vector2 otherParticle, ParticleGroup otherParticleGroup, Vector2 particle);
    }
}
