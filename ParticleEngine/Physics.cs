﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ParticleEngine
{
    public static class Physics
    {
        public static List<Particle> ParticleTypes = new List<Particle>();
        private const int GRAVITATIONAL_CONSTANT = 1;

        public static void Update()
        {
            HashSet<Vector2> collidingDots = new HashSet<Vector2>();

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
                    Vector2 original = particleGroup.Particles[i];
                    particleGroup.Particles[i] += new Vector2(0, particleGroup.Mass) * GRAVITATIONAL_CONSTANT;

                    if (collidingDots.Contains(particleGroup.Particles[i]))
                    {
                        particleGroup.Particles[i] = original;
                    }
                }
            }
        }

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
