using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleEngine.Particles
{
    class Powder : Particle
    {
        public Powder(float mass)
        {
            this.Mass = mass;
        }

        public override void DoPhysics()
        {
            
        }
    }
}