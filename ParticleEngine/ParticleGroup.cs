using System.Collections.Generic;
using System.Numerics;

namespace ParticleEngine
{
    public abstract class ParticleGroup
    {
        public readonly float Density;
        /// <summary>
        /// The angle of repose that determines how much to the side particles can slide when ontop of other particles. Measured in radians.
        /// </summary>
        public readonly float ReposeAngle;
        public readonly HashSet<Vector2> Particles;

        public ParticleGroup(float density, float reposeAngleRadian)
        {
            Particles = new HashSet<Vector2>();
            Density = density;
            ReposeAngle = reposeAngleRadian;
        }

        public abstract void OnCollide(Vector2 otherParticle, ParticleGroup otherParticleGroup, Vector2 particle);
    }
}
