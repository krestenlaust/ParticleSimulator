using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ParticleEngine
{
    public static class Physics
    {
        public static readonly List<ParticleGroup> ParticleGroups = new List<ParticleGroup>();
        private static readonly Dictionary<Vector2, ParticleGroup> particleMap = new Dictionary<Vector2, ParticleGroup>();
        private static readonly Queue<(Vector2 original, ParticleGroup originalGroup, Vector2 other, ParticleGroup otherGroup)> collisions =
            new Queue<(Vector2 original, ParticleGroup originalGroup, Vector2 other, ParticleGroup otherGroup)>();

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

            particleMap[position] = group;
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
                List<Vector2> particleList = particleGroup.Particles.ToList();
                foreach (Vector2 particle in particleList)
                {
                    // The vector that will take all the physics in and move the particle each frame the resulting force at what ever direction it should go
                    Vector2 resultingForce = new Vector2();

                    // Sedimentary force being calculated which also checks repose
                    (Vector2? primaryParticle, Vector2? swapParticle, ParticleGroup swapParticleGroup) = SedimentaryForce(particle, particleGroup);


                    if (primaryParticle.HasValue)
                    {
                        if (swapParticle.HasValue)
                        {
                            // Particles are swapped instead of moved by force.
                            SwapParticles(primaryParticle.Value, swapParticle.Value, particleGroup, swapParticleGroup);
                            continue;
                        }
                        else
                        {
                            // Convert primaryParticle to relative force.
                            resultingForce = primaryParticle.Value - particle;
                        }
                    }

                    Vector2 newParticle = particle + resultingForce;

                    // Moves the particle the amount of the resulting force
                    particleGroup.Particles.Remove(particle);
                    particleGroup.Particles.Add(newParticle);
                    particleMap.Remove(particle);
                    particleMap.Add(newParticle, particleGroup);
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

        private static (Vector2? primaryParticle, Vector2? swapParticle, ParticleGroup swapParticleGroup) SedimentaryForce(Vector2 particle, ParticleGroup particleGroup)
        {
            //Calculates the length blocks can move to either side when going down
            int maxLengthAway = (int)Math.Ceiling(Math.Tan(particleGroup.ReposeAngle));

            // Chooses the direction it should check first
            int direction = randomNumber.Next(2) == 1 ? 1 : -1;

            // For each direction
            for (int j = 0; j < 2; j++)
            {
                direction = -direction;

                // Checking one spot further out each time for both directions alternately
                for (int i = 0; i < maxLengthAway * 2; i++)
                {
                    // Undgå at tjekke i = 0 to gange.
                    if (i == 0 && j == 0)
                    {
                        break;
                    }

                    Vector2 checkVector = new Vector2(direction * i, 1);

                    // If the check position doesn't have a particle. If the statement is false then we have found a particle that we can try and swap with
                    if (!particleMap.ContainsKey(particle + checkVector))
                    {
                        // Moves particle to empty space since its empty by only returning our main particle
                        return (particle + checkVector, null, null);
                    }

                    RegisterCollision(particle + checkVector, (particle, particleGroup));

                    // Wheter it have found the particle above the checkvector
                    bool found = particleMap.TryGetValue(particle + new Vector2(checkVector.X, 0), out ParticleGroup sideCheckGroup);

                    // Makes sure that there isn't a particle above where it wants to go that is same or higher density, and ignores if first iteration
                    if (found)
                    {
                        // If there is a particle above checkposition which is denser or a block
                        if (sideCheckGroup.Density > particleGroup.Density && i != 0 || sideCheckGroup.Density == 0)
                        {
                            // Break so it stops checking this direction
                            break;
                        }
                    }

                    // Gets the particle group of the checkVector particel that we need to swap out with the primary particle
                    particleMap.TryGetValue(particle + checkVector, out ParticleGroup checkParticleGroup);

                    //If this particular particle group does not have a particle placed where it is checking
                    if (!checkParticleGroup.Particles.Contains(particle + checkVector))
                    {
                        break;
                    }

                    // If this type of particle is lighter then swap and density is not 0 which means that it's a block
                    if (checkParticleGroup.Density < particleGroup.Density && checkParticleGroup.Density != 0)
                    {
                        // Swaps the particles by returning our particle and the particle and group of the one to swap with
                        return (particle, particle + checkVector, checkParticleGroup);
                    }
                }
            }

            //Returns empty so nothing gets moved
            return (null, null, null);
        }

        /// <summary>
        /// Returns whether a collision has occured, and adds to collision queue if it has.
        /// </summary>
        /// <param name="checkPosition"></param>
        /// <param name="originalParticlePosition"></param>
        /// <param name="originalParticleGroup"></param>
        /// <returns></returns>
        private static void RegisterCollision(Vector2 checkPosition, (Vector2 originalParticlePosition, ParticleGroup originalParticleGroup) collisionParameters)
        {
            if (particleMap.TryGetValue(checkPosition, out ParticleGroup otherGroup))
            {
                // Queue if a collision has occured.
                collisions.Enqueue((collisionParameters.originalParticlePosition, collisionParameters.originalParticleGroup, checkPosition, otherGroup));
            }
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
