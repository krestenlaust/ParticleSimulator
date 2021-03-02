using ParticleEngine.Particles;
using System.Collections.Generic;
using System.Numerics;

namespace ParticleEngine
{
    public abstract class ParticleGroup
    {
        public int Mass;
        public float AngleOfReposeRad;
        public List<Vector2> Particles;

        public abstract void OnCollide(Vector2 otherParticle, ParticleGroup otherParticleGroup, Vector2 particle);
    }
}
