using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ParticleEngine
{
    public static class Physics
    {
        public const float GRAVITATIONAL_CONSTANT = 1;
        
        public static readonly List<ParticleGroup> ParticleGroups = new List<ParticleGroup>();
        private static readonly Dictionary<Vector2, ParticleGroup> particleMap = new Dictionary<Vector2, ParticleGroup>();
        private static readonly Queue<(Vector2 original, ParticleGroup originalGroup, Vector2 other, ParticleGroup otherGroup)> collisions = 
            new Queue<(Vector2 original, ParticleGroup originalGroup, Vector2 other, ParticleGroup otherGroup)>();

        private static Vector2 updraftVector;
        private static readonly Random randomNumber = new Random(42352352);
        private static bool updateInitialized = false;

        /// <summary>
        /// Instantiates a particle of type <c>T</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="position"></param>
        public static void Instantiate<T>(Vector2 position) where T : ParticleGroup, new()
        {
            ParticleGroup group = GetParticleGroup<T>();

            group.Particles.Add(position);
        }

        public static void Instantiate(Vector2 position, ParticleGroup particleGroup)
        {
            particleGroup.Particles.Add(position);
        }

        /// <summary>
        /// Brug IsColliding hvis du kan.
        /// </summary>
        /// <param name="checkPosition"></param>
        /// <returns></returns>
        public static bool IsOccupied(Vector2 checkPosition) => particleMap.ContainsKey(checkPosition);

        /// <summary>
        /// Gets a particlegroup of type <c>T</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ParticleGroup GetParticleGroup<T>() where T : ParticleGroup, new()
        {
            var group = (from particleGroup in ParticleGroups
                         where particleGroup is T
                         select particleGroup).FirstOrDefault();

            if (group is null)
            {
                group = new T();
                ParticleGroups.Add(group);
            }

            return group;
        }

        /// <summary>
        /// Removes all particles and resets internal state.
        /// </summary>
        public static void ResetPhysics()
        {
            ParticleGroups.Clear();
            updateInitialized = false;
        }

        public static void Update()
        {
            // Draw new collision map if not drawn in previous loop.
            if (!updateInitialized)
            {
                UpdateParticleMap();
                updateInitialized = true;
            }

            // Collection of parameters to call OnCollision, afterwards, using. They have to be executed at once since
            // OnCollision can mutate the collection of particles (remove/add particles), and you can't do that while 
            // iterating that collection ("foreach particleGroup in particleTypes").
            collisions.Clear();

            // Iterate all particles to perform physics.
            foreach (var particleGroup in ParticleGroups)
            {
                foreach (Vector2 particle in particleGroup.Particles.ToList())
                {
                    //Stops blocks from doing physics
                    if (particleGroup is Particles.Block)
                    {
                        continue;
                    }

                    //The vector that will take all the physics in and move the particle each frame the resulting force at what ever direction it should go
                    Vector2 resultingForce = new Vector2(0, 0);

                    //Applies gravity
                    resultingForce += new Vector2(0, GRAVITATIONAL_CONSTANT);

                    //Removes it again if it shouldn't have been applied
                    if (IsColliding(particle + resultingForce, particle, particleGroup))
                    {
                        resultingForce -= new Vector2(0, GRAVITATIONAL_CONSTANT);
                    }

                    //TODO: Updraft
                    updraftVector = new Vector2(0, -1);

                    // Adding the force from the particle's repose angle
                    //resultingForce += CheckRepose(i, particleGroup);

                    // Sedimentary force being calculated which also checks repose
                    resultingForce += SedimentaryForce(particle, particleGroup);

                    //Applies the resulting force
                    particleGroup.Particles.Remove(particle);
                    particleGroup.Particles.Add(particle + resultingForce);
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

            // redraw particlemap after updating particles.
            UpdateParticleMap();
        }

        private static Vector2 SedimentaryForce(Vector2 particle, ParticleGroup particleGroup)
        {
            int maxLengthAway = (int)Math.Ceiling(Math.Tan(particleGroup.AngleOfReposeRad)); //Calculates the length blocks can move to the side when going down
            
            // Midlertidig
            if (!(particleGroup is Particles.Block))
            {

            }

            int dir;
            // Chooses the direction it should check first
            if (randomNumber.Next(2) == 1)
            {
                dir = 1;
            }
            else
            {
                dir = -1;
            }

            for (int n = 0; n < maxLengthAway * 2; n++) // Checking one spot further out each time for the first direction
            {
                // Two vectors so it can check both in one loop instead of going through a whole loop once more
                Vector2 checkVector = new Vector2(dir * n, 1);

                // Checks that this particle is on a particle and can actually move other places than straight down
                if (!IsColliding(particle + new Vector2(0, 1), particle, particleGroup))
                {
                    break;
                }

                // Checks if the spot have another particle ontop of it that blocks the particle and stops checking the direction if it does, if it checks directly underneath then it skips
                if (IsColliding(new Vector2(particle.X + checkVector.X, particle.Y), particle, particleGroup) && n != 0)
                {
                    break;
                }

                // Checks if the new position have a particle
                if (!IsColliding(particle + checkVector, particle, particleGroup))
                {
                    // Moves particle to empty space since its empty
                    return checkVector;
                    
                }

                // Checks if the particle is lighter than the one that checks
                foreach (ParticleGroup checkParticleGroup in ParticleGroups)
                {
                    //If this particular particle group does not have a particle placed where it is checking
                    if (!checkParticleGroup.Particles.Contains(particle + checkVector))
                    {
                        continue;
                    }

                    // If this type of particle is lighter
                    if (checkParticleGroup.Mass < particleGroup.Mass && checkParticleGroup.Mass != 0)
                    {
                        // Swaps the particles
                        //SwapParticles(particle, particle + checkVector, particleGroup, checkParticleGroup);
                    }
                }
                continue;
            }
            
            //Switches the direction
            dir *= -1;

            // skal fikses så der ikke er så meget kodedublikation. Kan laves til en metode.
            for (int n = 0; n < maxLengthAway * 2; n++) // Checking one spot further out each time for the first direction
            {
                // Two vectors so it can check both in one loop instead of going through a whole loop once more
                Vector2 checkVector = new Vector2(dir * n, 1);

                // Checks that this particle is on a particle and can actually move other places than straight down
                if (!IsColliding(particle + new Vector2(0, 1), particle, particleGroup))
                {
                    break;
                }

                // Checks if the spot have another particle ontop of it that blocks the particle and stops checking the direction if it does, if it checks directly underneath then it skips
                if (IsColliding(new Vector2(particle.X + checkVector.X, particle.Y), particle, particleGroup) && n != 0)
                {
                    break;
                }

                // Checks if the new position have a particle
                if (!IsColliding(particle + checkVector, particle, particleGroup))
                {
                    // Moves particle to empty space since its empty
                    return checkVector;

                }

                // Checks if the particle is lighter than the one that checks
                foreach (ParticleGroup checkParticleGroup in ParticleGroups)
                {
                    //If this particular particle group does not have a particle placed where it is checking
                    if (!checkParticleGroup.Particles.Contains(particle + checkVector))
                    {
                        continue;
                    }

                    // If this type of particle is lighter
                    if (checkParticleGroup.Mass < particleGroup.Mass && checkParticleGroup.Mass != 0)
                    {
                        // Swaps the particles
                        //SwapParticles(particle, particle + checkVector, particleGroup, checkParticleGroup);
                    }
                }
                continue;
            }
            // Returns nothing if there is nothing to move
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
            if (particleMap.TryGetValue(checkPosition, out ParticleGroup otherGroup))
            {
                // Queue if a collision has occured.
                collisions.Enqueue((originalParticlePosition, originalParticleGroup, checkPosition, otherGroup));

                return true;
            }

            return false;
        }

        private static void UpdateParticleMap()
        {
            particleMap.Clear();

            // Generates collection of all particle positions to check collision and handle reactions.
            foreach (var particleGroup in ParticleGroups)
            {
                foreach (var particle in particleGroup.Particles)
                {
                    particleMap[particle] = particleGroup;
                }
            }
        }

        /// <summary>
        /// Swaps two particles.
        /// </summary>
        /// <param name="particle1"></param>
        /// <param name="particle2"></param>
        /// <param name="group1"></param>
        /// <param name="group2"></param>
        private static void SwapParticles(Vector2 particle1, Vector2 particle2, ParticleGroup group1, ParticleGroup group2)
        {
            group1.Particles.Remove(particle1);
            group2.Particles.Remove(particle2);
            group1.Particles.Add(particle2);
            group2.Particles.Add(particle1);
        }
    }
}
