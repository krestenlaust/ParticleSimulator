﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;
using ParticleEngine.Particles;

namespace ParticleEngine.Tests
{
    [TestClass]
    public class PhysicsLogicTests
    {
        /// <summary>
        /// Vi forventer at sandkornets y-koordinat vil stige for hver opdatering, da det falder lige uden noget i vejen.
        /// </summary>
        [TestMethod]
        public void TestFreeFallingSand2TimesInARow()
        {
            // Fordi det er en statisk klasse vil ændringer blive gemt gennem tests. Derfor kan der være rester fra en anden test.
            Physics.ParticleGroups.Clear();

            // Setup
            Physics.Instantiate<Sand>(new Vector2(0, 0));

            ParticleGroup sandGroup = Physics.GetParticleGroup<Sand>();

            // Logic
            Physics.Update();
            Assert.AreEqual(new Vector2(0, Physics.GRAVITATIONAL_CONSTANT), sandGroup.Particles[0]);

            Physics.Update();
            Assert.AreEqual(new Vector2(0, Physics.GRAVITATIONAL_CONSTANT * 2), sandGroup.Particles[0]);
        }

        [TestMethod]
        public void TestSandFallingOnBlock()
        {
            // Fordi det er en statisk klasse vil ændringer blive gemt gennem tests. Derfor kan der være rester fra en anden test.
            Physics.ParticleGroups.Clear();

            // Setup
            Physics.Instantiate<Block>(new Vector2(0, Physics.GRAVITATIONAL_CONSTANT * 2));
            Physics.Instantiate<Sand>(new Vector2(0, 0));

            ParticleGroup blockGroup = Physics.GetParticleGroup<Block>();
            ParticleGroup sandGroup = Physics.GetParticleGroup<Sand>();

            // Logic
            Physics.Update();
            // Antag at blokken ikke har rykket sig.
            Assert.AreEqual(new Vector2(0, Physics.GRAVITATIONAL_CONSTANT * 2), blockGroup.Particles[0]);

            // Antag at sand er flyttet.
            Assert.AreEqual(new Vector2(0, Physics.GRAVITATIONAL_CONSTANT), sandGroup.Particles[0]);

            Physics.Update();
            // Antag at sand bliver stående, siden blokken er i vejen.
            Assert.AreEqual(new Vector2(0, Physics.GRAVITATIONAL_CONSTANT), sandGroup.Particles[0]);
        }
    }
}
