using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            // Setup
            // Fordi det er en statisk klasse vil ændringer blive gemt gennem tests. Derfor kan der være rester fra en anden test.
            ParticleGroup sandGroup = Physics.GetParticleGroup<Sand>();

            int sandID = 0;
            if (!(sandGroup is null))
            {
                sandID = sandGroup.Particles.Count;
            }
            Physics.Instantiate<Sand>(new Vector2(0, 0));

            sandGroup = Physics.GetParticleGroup<Sand>();

            // Logic
            Physics.Update();
            Assert.AreEqual(new Vector2(0, 1), sandGroup.Particles[sandID]);

            Physics.Update();
            Assert.AreEqual(new Vector2(0, 2), sandGroup.Particles[sandID]);
        }

        [TestMethod]
        public void TestSandFallingOnBlock()
        {
            // Setup
            // Fordi det er en statisk klasse vil ændringer blive gemt gennem tests. Derfor kan der være rester fra en anden test.
            ParticleGroup blockGroup = Physics.GetParticleGroup<Block>();
            ParticleGroup sandGroup = Physics.GetParticleGroup<Sand>();

            int blockID = 0, sandID = 0;
            if (!(blockGroup is null))
            {
                blockID = blockGroup.Particles.Count;
            }
            if (!(sandGroup is null))
            {
                sandID = sandGroup.Particles.Count;
            }

            Physics.Instantiate<Block>(new Vector2(0, 2));
            Physics.Instantiate<Sand>(new Vector2(0, 0));

            blockGroup = Physics.GetParticleGroup<Block>();
            sandGroup = Physics.GetParticleGroup<Sand>();

            // Logic
            Physics.Update();
            // Antag at blokken ikke har rykket sig.
            Assert.AreEqual(new Vector2(0, 2), blockGroup.Particles[blockID]);

            // Antag at sand er flyttet.
            Assert.AreEqual(new Vector2(0, 1), sandGroup.Particles[sandID]);

            Physics.Update();
            // Antag at sand bliver stående, siden blokken er i vejen.
            Assert.AreEqual(new Vector2(0, 1), sandGroup.Particles[sandID]);
        }
    }
}
