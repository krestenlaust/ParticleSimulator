using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ParticleEngine
{
    public static class Physics
    {
        private const float GRAVITATIONAL_CONSTANT = 1;
        public static List<Particle> ParticleTypes = new List<Particle>();
        private static HashSet<Vector2> collidingDots = new HashSet<Vector2>();

        private static Random randomNumber = new Random(42352352);

        public static void Update()
        {
            collidingDots.Clear();

            foreach (var particleGroup in ParticleTypes)
            {
                foreach (var particle in particleGroup.Particles)
                {
                    collidingDots.Add(particle);
                }
            }

            foreach (var particleGroup in ParticleTypes)
            {
                for (int i = 0; i < particleGroup.Particles.Count; i++)
                {
                    //The vector that will take all the physics in and move the particle each frame the resulting force at what ever direction it should go
                    Vector2 resultingForce = new Vector2(0,0);

                    //Applies gravity
                    resultingForce += new Vector2(0, particleGroup.Mass) * GRAVITATIONAL_CONSTANT;

                    if (collidingDots.Contains(particleGroup.Particles[i] + resultingForce)) //Removes it again if it shouldn't have been applied
                    {
                        resultingForce -= new Vector2(0, particleGroup.Mass) * GRAVITATIONAL_CONSTANT;
                    }

                    //TODO: Updraft
                    Vector2 updraftVector = new Vector2(0, -1);

                    //Angle of repose
                    if (updraftVector.Y <= 0) //If the particle is going downward
                    {
                        if (particleGroup is Particles.Sand)
                        {

                        }
                        int maxLengthAway = (int)Math.Ceiling(Math.Tan(particleGroup.AngleOfReposeRad));
                        for (int n = 0; n < maxLengthAway * 2; n++)
                        {
                            Vector2 checkVector;
                            if (randomNumber.Next(2) == 1)
                            {
                                checkVector = new Vector2(n, 1);
                            }
                            else
                            {
                                checkVector = new Vector2(-n, 1);
                            }

                            /*if (checkVector < 0 || checkVector > screenwidthnoget) //Jeg har brug for at vide størrelsen på vores ting fra consoleUI eller få en nem måde at spørge den om en vector er indenfor.
                            {

                            }*/

                            if (!collidingDots.Contains(particleGroup.Particles[i] + checkVector) && collidingDots.Contains(new Vector2(particleGroup.Particles[i].X, particleGroup.Particles[i].Y + 1)))
                            {
                                resultingForce += checkVector;
                            }
                        }
                    }

                    //Applies the resulting force
                    particleGroup.Particles[i] += resultingForce;
                }
            }
        }

        /// <summary>
        /// Instantiates a particle of type <c>T</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="position"></param>
        public static void Instantiate<T>(Vector2 position) where T : Particle, new()
        {
            Particle group = (from particleGroup in ParticleTypes
                             where particleGroup is T
                             select particleGroup).FirstOrDefault();

            if (group is null)
            {
                group = new T();
                ParticleTypes.Add(group);
            }

            group.Particles.Add(position);
        }
    }
}
