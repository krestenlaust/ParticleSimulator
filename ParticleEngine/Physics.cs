using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ParticleEngine
{
    public static class Physics
    {
        private static List<Particle> particleTypes = new List<Particle>();
        private const int GRAVITATIONAL_CONSTANT = 10;

        public static void Update()
        {
            foreach (var particleGroup in particleTypes)
            {
                for (int i = 0; i < particleGroup.Particles.Count; i++)
                {
                    particleGroup.Particles[i] += new Vector2(0, particleGroup.Mass) * GRAVITATIONAL_CONSTANT;
                }
            }
        }
    }
}
