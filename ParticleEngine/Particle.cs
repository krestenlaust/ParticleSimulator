using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ParticleEngine
{
    public class Particle
    {
        public float Mass { get; protected set; }
        public List<Vector2> particles { get; protected set; }
    }
}