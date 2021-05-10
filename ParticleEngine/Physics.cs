using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ParticleEngine
{
    public static class Physics
    {
        private const float GRAVITATIONAL_CONSTANT = 1;
        public static List<ParticleGroup> ParticleGroups = new List<ParticleGroup>();

        private static Dictionary<Vector2, Vector2> airPressure = new Dictionary<Vector2, Vector2>();

        private static Dictionary<Vector2, ParticleGroup> collisionMap = new Dictionary<Vector2, ParticleGroup>();
        private static Queue<(Vector2 original, ParticleGroup originalGroup, Vector2 other, ParticleGroup otherGroup)> collisions = 
            new Queue<(Vector2 original, ParticleGroup originalGroup, Vector2 other, ParticleGroup otherGroup)>();

        private static Vector2 updraftVector;

        private static Random randomNumber = new Random(42352352);

        public static void Update()
        {
            // Draw new collision map.
            collisionMap.Clear();

            // Generates collection of all particle positions to check collision and handle reactions.
            foreach (var particleGroup in ParticleGroups)
            {
                foreach (var particle in particleGroup.Particles)
                {
                    collisionMap[particle] = particleGroup;
                }
            }

            // Collection of parameters to call OnCollision, afterwards, using. They have to be executed at once since
            // OnCollision can mutate the collection of particles (remove/add particles), and you can't do that while 
            // iterating that collection ("foreach particleGroup in particleTypes").
            collisions.Clear();

            // Iterate all particles to perform physics.
            foreach (var particleGroup in ParticleGroups)
            {
                for (int i = 0; i < particleGroup.Particles.Count; i++)
                {
                    //The vector that will take all the physics in and move the particle each frame the resulting force at what ever direction it should go
                    Vector2 resultingForce = new Vector2(0,0);

                    //Applies gravity
                    resultingForce += new Vector2(0, particleGroup.Mass) * GRAVITATIONAL_CONSTANT;

                    //Removes it again if it shouldn't have been applied
                    if (IsColliding(particleGroup.Particles[i] + resultingForce, particleGroup.Particles[i], particleGroup))
                    {
                        resultingForce -= new Vector2(0, particleGroup.Mass) * GRAVITATIONAL_CONSTANT;
                    }

                    //TODO: Updraft
                    updraftVector = new Vector2(0, -1);

                    // Adding the force from the particle's repose angle
                    //resultingForce += CheckRepose(i, particleGroup);

                    resultingForce += SedimentaryForce();

                    //Applies the resulting force
                    particleGroup.Particles[i] += resultingForce;
                }
            }

            // Iterate all observed collisions and call their respective ParticleGroup handler.
            while (collisions.Count > 0)
            {
                (Vector2 otherParticle, ParticleGroup otherGroup, Vector2 particle, ParticleGroup group) = collisions.Dequeue();

                group.OnCollide(otherParticle, otherGroup, particle);
                otherGroup.OnCollide(particle, group, otherParticle);
            }

            List<ParticleGroup> groups = ParticleGroups.ToList();
            foreach (var group in groups)
            {
                group.OnUpdate(ParticleGroups);
            }
        }

        private static Vector2 SedimentaryForce()
        {

            return Vector2.Zero;
        }

        /// <summary>
        /// Returns whether a collision has occured and adds to collision queue of it has.
        /// </summary>
        /// <param name="checkPosition"></param>
        /// <param name="originalParticlePosition"></param>
        /// <param name="originalParticleGroup"></param>
        /// <returns></returns>
        private static bool IsColliding(Vector2 checkPosition, Vector2 originalParticlePosition, ParticleGroup originalParticleGroup)
        {
            if (collisionMap.TryGetValue(checkPosition, out ParticleGroup otherGroup))
            {
                // Queue if a collision has occured.
                collisions.Enqueue((originalParticlePosition, originalParticleGroup, checkPosition, otherGroup));

                return true;
            }

            return false;
        }

        /// <summary>
        /// Brug IsColliding hvis du kan.
        /// </summary>
        /// <param name="checkPosition"></param>
        /// <returns></returns>
        public static bool IsOccupied(Vector2 checkPosition) => collisionMap.ContainsKey(checkPosition);

        public static Vector2 CheckRepose(int i, ParticleGroup particleGroup)
        {
            if (updraftVector.Y <= 0) //If the particle is going downward
            {
                int maxLengthAway = (int)Math.Ceiling(Math.Tan(particleGroup.AngleOfReposeRad)); //Calculates the length from

                int dir;
                //Chooses the direction it should check first
                if (randomNumber.Next(2) == 1)
                {
                    dir = 1;
                }
                else
                {
                    dir = -1;
                }

                bool checkingDir1 = true;
                bool checkingDir2 = true;

                for (int n = 0; n < maxLengthAway * 2; n++) //Checking one spot further out each time
                {
                    Vector2 checkVector = new Vector2(dir * n, 1);
                    Vector2 otherCheckVector = new Vector2(dir * -n, 1);

                    // Checks if the checking spot have another particle underneath
                    if (IsColliding(particleGroup.Particles[i] + new Vector2(0, particleGroup.Mass), particleGroup.Particles[i], particleGroup))
                    {
                        continue;
                    }

                    // Checks if the spot is empty and that it should check this direction
                    if (checkingDir1 && !IsColliding(particleGroup.Particles[i] + checkVector, particleGroup.Particles[i], particleGroup))
                    {
                        return checkVector;
                    }
                    else if (!IsColliding(particleGroup.Particles[i] + checkVector + new Vector2(0, -1), particleGroup.Particles[i], particleGroup)) //Checks if there is room to the side in order to make it move
                    {
                        checkingDir1 = false;
                    }

                    // Checks if the spot is empty and that it should check this direction
                    if (checkingDir2 && !IsColliding(particleGroup.Particles[i] + otherCheckVector, particleGroup.Particles[i], particleGroup))
                    {
                        return otherCheckVector;
                    }
                    else if (!IsColliding(particleGroup.Particles[i] + otherCheckVector + new Vector2(0, -1), particleGroup.Particles[i], particleGroup)) //Checks if there is room to the side in order to make it move
                    {
                        checkingDir2 = false;
                    }
                }

            }
            return Vector2.Zero;
            /*// Angle of repose
            if (updraftVector.Y <= 0) //If the particle is going downward
            {
                int maxLengthAway = (int)Math.Ceiling(Math.Tan(particleGroup.AngleOfReposeRad)); //Calculates the length from

                int dir = 1;
                //Chooses the direction it should check first
                if (randomNumber.Next(2) == 1)
                {
                    dir = 1;
                }
                else
                {
                    dir = -1;
                }

                for (int n = 0; n < maxLengthAway * 2; n++)
                {
                    Vector2 checkVector = new Vector2(dir * n, 1);

                    // Checks if the checking spot is empty and that the particle actually have another particle underneath
                    if (!IsColliding(particleGroup.Particles[i] + checkVector, particleGroup.Particles[i], particleGroup) && IsColliding(particleGroup.Particles[i] + new Vector2(0, particleGroup.Mass), particleGroup.Particles[i], particleGroup))/*(!collisionMap.TryGetValue(particleGroup.Particles[i] + checkVector, out _) &&
                        collisionMap.TryGetValue(particleGroup.Particles[i] + new Vector2(0, 1), out _))
                    {
                        return checkVector;
                    }

                    checkVector = new Vector2(dir * -n, 1);

                    // Checks if the checking spot is empty and that the particle actually have another particle underneath
                    if (!IsColliding(particleGroup.Particles[i] + checkVector, particleGroup.Particles[i], particleGroup) && IsColliding(particleGroup.Particles[i] + new Vector2(0, particleGroup.Mass), particleGroup.Particles[i], particleGroup))/*(!collisionMap.TryGetValue(particleGroup.Particles[i] + checkVector, out _) &&
                        collisionMap.TryGetValue(particleGroup.Particles[i] + new Vector2(0, 1), out _))
                    {
                        return checkVector;
                    }
                    else // Checks the other direction
                    {
                        // Checks if the checking spot is empty and that the particle actually have another particle underneath
                        if (!IsColliding(particleGroup.Particles[i] + new Vector2(dir * -n, particleGroup.Mass * GRAVITATIONAL_CONSTANT), particleGroup.Particles[i], particleGroup))
                        {
                            return checkVector;
                        }
                    }
                }
            }*/
        }

        /// <summary>
        /// Swaps two particles.
        /// </summary>
        /// <param name="particle1"></param>
        /// <param name="particle2"></param>
        /// <param name="group1"></param>
        /// <param name="group2"></param>
        public static void SwapParticles(Vector2 particle1, Vector2 particle2, ParticleGroup group1, ParticleGroup group2)
        {
            group1.Particles.Remove(particle1);
            group2.Particles.Remove(particle2);
            group1.Particles.Add(particle2);
            group2.Particles.Add(particle1);
        }

        /// <summary>
        /// Instantiates a particle of type <c>T</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="position"></param>
        public static void Instantiate<T>(Vector2 position) where T : ParticleGroup, new()
        {
            ParticleGroup group = GetParticleGroup<T>();
            
            if (group is null)
            {
                group = new T();
                ParticleGroups.Add(group);
            }

            group.Particles.Add(position);
        }

        /// <summary>
        /// Gets a particlegroup of type <c>T</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ParticleGroup GetParticleGroup<T>() where T : ParticleGroup
        {
            return (from particleGroup in ParticleGroups
                    where particleGroup is T
                    select particleGroup).FirstOrDefault();
        }
    }
}
