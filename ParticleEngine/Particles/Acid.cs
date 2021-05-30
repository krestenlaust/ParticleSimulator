using System;
using System.Collections.Generic;
using System.Numerics;

namespace ParticleEngine.Particles
{
    public class Acid : ParticleGroup
    {
        private static Random randomNumber = new Random(42352351);

        public Acid() : base(
            1,
            0 * 34f / 180f * (float)Math.PI)
        {

        }

        public override void OnCollide(Vector2 otherParticle, ParticleGroup otherParticleGroup, Vector2 particle)
        {
            /*
            if (otherParticleGroup is Block || otherParticleGroup is Acid)
            {
                Queue<(Vector2, Vector2)> flytteDimser = new Queue<(Vector2, Vector2)>();

                // Shitty liquid physics, flyt det til Physics klassen
                if (randomNumber.Next(2) == 1)
                {
                    if (!Physics.IsOccupied(new Vector2(particle.X - 1, particle.Y))
                        && !Particles.Contains(new Vector2(particle.X - 1, particle.Y)))
                    {
                        flytteDimser.Enqueue((particle, particle + new Vector2(-1, 0)));
                    }
                }
                else
                {
                    if (!Physics.IsOccupied(new Vector2(particle.X + 1, particle.Y))
                    && !Particles.Contains(new Vector2(particle.X + 1, particle.Y)))
                    {
                        flytteDimser.Enqueue((particle, particle + new Vector2(1, 0)));
                    }
                }

                while (flytteDimser.Count > 0)
                {
                    (Vector2 fra, Vector2 til) = flytteDimser.Dequeue();
                    Particles.Remove(fra);
                    Particles.Add(til);
                }

                return;
            }*/

            otherParticleGroup.Particles.Remove(otherParticle);
            Particles.Remove(particle);
        }

        public override void OnUpdate(List<ParticleGroup> particleGroups)
        {

        }
    }
}
