using System.Collections.Generic;
using System.Numerics;

namespace ParticleEngine
{
    public abstract class ParticleGroup
    {
        public int Mass;
        public float AngleOfReposeRad;
        public List<Vector2> Particles;

        public abstract void DoPhysics();
    }
}
