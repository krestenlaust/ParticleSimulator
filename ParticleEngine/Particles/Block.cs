using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ParticleEngine.Particles
{
    public class Block : Particle
    {
        public Block()
        {
            Particles = new List<Vector2>();
            Mass = 0;
        }

        public override void DoPhysics()
        {
            
        }
    }
}