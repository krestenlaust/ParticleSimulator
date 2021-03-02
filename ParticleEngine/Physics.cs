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
        
        private static Dictionary<Vector2, ParticleGroup> collisionMap = new Dictionary<Vector2, ParticleGroup>();
        private static Queue<(Vector2 original, ParticleGroup originalGroup, Vector2 other, ParticleGroup otherGroup)> collisions = 
            new Queue<(Vector2 original, ParticleGroup originalGroup, Vector2 other, ParticleGroup otherGroup)>();

        private static Vector2 updraftVector;
        private static ParticleGroup particleGroup;

        private static Random randomNumber = new Random(42352352);

        public static void Update()
        {
            // Draw new collision map.
            collisionMap.Clear();

            // Generates collection of all particle positions to check collision and handle reactions.
            foreach (var particleGroup in ParticleTypes)
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
            foreach (var particleGroup in ParticleTypes)
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
                    resultingForce += CheckRepose(i, particleGroup);
                    
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

        public static Vector2 CheckRepose(int i, ParticleGroup particleGroup)
        {
            //Angle of repose
            if (updraftVector.Y <= 0) //If the particle is going downward
            {
                int maxLengthAway = (int)Math.Ceiling(Math.Tan(particleGroup.AngleOfReposeRad)); //Calculates the length from
                for (int n = 0; n < maxLengthAway * 2; n++)
                {
                    Vector2 checkVector; 
                    //Chooses the direction it should check first
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

                    // Checks if the checking spot is empty and that the particle actually have another particle underneath
                    if (!collisionMap.TryGetValue(particleGroup.Particles[i] + checkVector, out _) &&
                        collisionMap.TryGetValue(particleGroup.Particles[i] + new Vector2(0, 1), out _))
                    {
                        return checkVector;
                    }
                }
            }
            return Vector2.Zero;
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
