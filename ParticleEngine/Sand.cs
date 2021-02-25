using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleEngine
{
    public class Sand : Particle
    {
        public Sand()
        {
            Particles = new List<System.Numerics.Vector2>();
        }

        public override void DoPhyics()
        {
            for (int i = 0; i < Particles.Count; i++)
            {
                //Particles[i].Y += 
            }
        }
    }
}
