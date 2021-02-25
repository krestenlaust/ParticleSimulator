using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ParticleEngine
{
    public abstract class Particle
    {
        public int Mass;

        public List<Vector2> Particles;

        public abstract void DoPhysics();
    }
}
