using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ParticleEngine.Particles
{
    public class Gas : ParticleGroup
    {
        public Gas() : base(-1, 60f / 180f * (float)Math.PI)
        {

        }

        public override void OnCollide(Vector2 otherParticle, ParticleGroup otherParticleGroup, Vector2 particle)
        {

        }
    }
}
