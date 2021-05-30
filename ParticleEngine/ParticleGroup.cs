using System.Collections.Generic;
using System.Numerics;

namespace ParticleEngine
{
    public abstract class ParticleGroup
    {
        public readonly float Density;
        /// <summary>
        /// Den vinkel en bloktype tjekker, om den kan rykke hen ved. I radianer.
        /// </summary>
        public readonly float ReposeAngle;
        public readonly HashSet<Vector2> Particles;

        public ParticleGroup(float density, float reposeAngleRadian)
        {
            Particles = new HashSet<Vector2>();
            Density = density;
            ReposeAngle = reposeAngleRadian;
        }

        public abstract void OnUpdate(List<ParticleGroup> particleGroups);
        public abstract void OnCollide(Vector2 otherParticle, ParticleGroup otherParticleGroup, Vector2 particle);
    }
}
