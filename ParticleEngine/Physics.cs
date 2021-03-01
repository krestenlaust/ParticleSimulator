using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ParticleEngine
{
    public static class Physics
    {
        private const float GRAVITATIONAL_CONSTANT = 1;
        public static List<ParticleGroup> ParticleTypes = new List<ParticleGroup>();
        private static Dictionary<Vector2, ParticleGroup> collidingDots = new Dictionary<Vector2, ParticleGroup>();

        private static Random randomNumber = new Random(42352352);

        public static void Update()
        {
            collidingDots.Clear();

            foreach (var particleGroup in ParticleTypes)
            {
                foreach (var particle in particleGroup.Particles)
                {
                    collidingDots[particle] = particleGroup;
                }
            }

            Stack<(Vector2, ParticleGroup, Vector2, ParticleGroup)> parameters = new Stack<(Vector2, ParticleGroup, Vector2, ParticleGroup)>();

            foreach (var particleGroup in ParticleTypes)
            {
                for (int i = 0; i < particleGroup.Particles.Count; i++)
                {
                    //The vector that will take all the physics in and move the particle each frame the resulting force at what ever direction it should go
                    Vector2 resultingForce = new Vector2(0,0);

                    //Applies gravity
                    resultingForce += new Vector2(0, particleGroup.Mass) * GRAVITATIONAL_CONSTANT;

                    //Removes it again if it shouldn't have been applied
                    if (collidingDots.TryGetValue(particleGroup.Particles[i] + resultingForce, out ParticleGroup otherParticleGroup))
                    {
                        parameters.Push((particleGroup.Particles[i] + resultingForce, otherParticleGroup,
                            particleGroup.Particles[i], particleGroup));

                        resultingForce -= new Vector2(0, particleGroup.Mass) * GRAVITATIONAL_CONSTANT;
                    }

                    //TODO: Updraft
                    Vector2 updraftVector = new Vector2(0, -1);

                    //Angle of repose
                    if (updraftVector.Y <= 0) //If the particle is going downward
                    {
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

                            /*if (checkVector < 0 || checkVector > screenwidthnoget) 
                            {

                            }*/

                            if (!collidingDots.Contains(particleGroup.Particles[i] + checkVector) && 
                                collidingDots.Contains(particleGroup.Particles[i] + new Vector2(0, 1)))
                            {
                                resultingForce += checkVector;
                            }
                        }
                    }

                    //Applies the resulting force
                    particleGroup.Particles[i] += resultingForce;
                }
            }

            while (parameters.Count > 0)
            {
                (Vector2 otherParticle, ParticleGroup otherGroup, Vector2 particle, ParticleGroup group) = parameters.Pop();

                group.OnCollide(otherParticle, otherGroup, particle);
                otherGroup.OnCollide(particle, group, otherParticle);
            }
        }

        /// <summary>
        /// Instantiates a particle of type <c>T</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="position"></param>
        public static void Instantiate<T>(Vector2 position) where T : ParticleGroup, new()
        {
            ParticleGroup group = (from particleGroup in ParticleTypes
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
