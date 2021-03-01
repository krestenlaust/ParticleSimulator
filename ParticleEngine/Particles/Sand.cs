using System;
using System.Collections.Generic;
using System.Numerics;

namespace ParticleEngine.Particles
{
    public class Sand : ParticleGroup
    {
        public Sand()
        {
            Particles = new List<Vector2>();
            Mass = 1;
            AngleOfReposeRad = 34f / 180f * (float)Math.PI; //Pres til fysik implemention - 34 grader for sand
        }

        public override void DoPhysics()
        {
            
        }
    }
}