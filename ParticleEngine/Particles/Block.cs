using System.Numerics;

namespace ParticleEngine.Particles
{
    public class Block : ParticleGroup
    {
        public Block() : base(0, 0)
        {
        }

        public override void OnCollide(Vector2 otherParticle, ParticleGroup otherParticleGroup, Vector2 particle)
        {
        }
    }
}