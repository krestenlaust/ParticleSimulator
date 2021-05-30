using System.Collections.Generic;
using System.Numerics;

namespace ParticleEngine
{
    public abstract class ParticleGroup
    {
        public readonly float Density;
        public readonly float AngleOfReposeRad;
        public readonly HashSet<Vector2> Particles;

        public ParticleGroup(float density, float angleOfReposeRad)
        {
            Particles = new HashSet<Vector2>();
            Density = density;
            AngleOfReposeRad = angleOfReposeRad;
        }

        public abstract void OnUpdate(List<ParticleGroup> particleGroups);
        public abstract void OnCollide(Vector2 otherParticle, ParticleGroup otherParticleGroup, Vector2 particle);
    }
}
