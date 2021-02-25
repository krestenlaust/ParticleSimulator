using System;
using System.Collections.Generic;
using System.Numerics;

namespace ParticleEngine
{
    public class Sand : Particle
    {
        public Sand()
        {
            Particles = new List<Vector2>();
            Mass = 1;
        }

        public override void DoPhysics()
        {

        }
    }
}
