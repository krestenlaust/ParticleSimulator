using System;
using System.Collections.Generic;

namespace ParticleEngine
{
    public class Sand : Particle
    {
        public Sand()
        {
            Particles = new List<System.Numerics.Vector2>();
        }

        public override void DoPhysics()
        {
            for (int i = 0; i < Particles.Count; i++)
            {

            }
        }
    }
}
