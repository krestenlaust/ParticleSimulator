using System;
using System.Collections.Generic;
using System.Numerics;

namespace ParticleEngine.Particles
{
    public class Sand : Particle
    {
        public Sand()
        {
            Particles = new List<Vector2>();
            Mass = 1;
            AngleOfReposeRad = 34 / 180 * (float)Math.PI; //Pres til fysik implemention - 34 grader for sand
        }

        public override void DoPhysics()
        {
            
        }
    }
}
