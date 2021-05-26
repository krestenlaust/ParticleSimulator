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
            // Fordi det er en statisk klasse vil ændringer blive gemt gennem tests. Derfor kan der være rester fra en anden test.
            Physics.ParticleGroups.Clear();

            // Setup
            Physics.Instantiate<Sand>(new Vector2(0, 0));

            ParticleGroup sandGroup = Physics.GetParticleGroup<Sand>();

            // Logic
            Physics.Update();
            Assert.AreEqual(new Vector2(0, 1), sandGroup.Particles[0]);

            Physics.Update();
            Assert.AreEqual(new Vector2(0, 2), sandGroup.Particles[0]);
        }

        [TestMethod]
        public void TestSandFallingOnBlocks()
        {
            // Fordi det er en statisk klasse vil ændringer blive gemt gennem tests. Derfor kan der være rester fra en anden test.
            Physics.ParticleGroups.Clear();

            // Setup
            Physics.Instantiate<Block>(new Vector2(0, 2));
            Physics.Instantiate<Block>(new Vector2(1, 2));
            Physics.Instantiate<Block>(new Vector2(-1, 2));
            Physics.Instantiate<Sand>(new Vector2(0, 0));

            ParticleGroup blockGroup = Physics.GetParticleGroup<Block>();
            ParticleGroup sandGroup = Physics.GetParticleGroup<Sand>();

            // Logic
            Physics.Update();
            // Antag at blokken ikke har rykket sig.
            Assert.AreEqual(new Vector2(0, 2), blockGroup.Particles[0]);

            // Antag at sand er flyttet.
            Assert.AreEqual(new Vector2(0, 1), sandGroup.Particles[0]);

            Physics.Update();
            // Antag at sand bliver stående, siden blokken er i vejen.
            Assert.AreEqual(new Vector2(0, 1), sandGroup.Particles[0]);
        }

        [TestMethod]
        public void TestParticlesSwapsWhenHeavyParticleOverLighterParticle() //Patricks :)
        {
            // Fordi det er en statisk klasse vil ændringer blive gemt gennem tests. Derfor kan der være rester fra en anden test.
            Physics.ParticleGroups.Clear();

            // Water and sand holder of blocks
            Physics.Instantiate<Block>(new Vector2(-1, -1));
            Physics.Instantiate<Block>(new Vector2(1, -1));
            Physics.Instantiate<Block>(new Vector2(-1, 0));
            Physics.Instantiate<Block>(new Vector2(1, 0));
            Physics.Instantiate<Block>(new Vector2(-1, 1));
            Physics.Instantiate<Block>(new Vector2(0, 1));
            Physics.Instantiate<Block>(new Vector2(1, 1));


            // Sand and water particles
            Vector2 sandStartingPlace = new Vector2(0, -1);
            Vector2 waterStartingPlace = new Vector2(0, 0);
            Physics.Instantiate<Sand>(sandStartingPlace);
            Physics.Instantiate<Water>(waterStartingPlace);

            ParticleGroup sandGroup = Physics.GetParticleGroup<Sand>();
            ParticleGroup waterGroup = Physics.GetParticleGroup<Water>();

            // Logic
            Physics.Update();
            
            // Antag at sand og vand er byttet.
            // Test om sandet er hvor vandet startede:
            Assert.AreEqual(waterStartingPlace, sandGroup.Particles[0]);
            // Test om vandet er hvor sandet startede:
            Assert.AreEqual(sandStartingPlace, waterGroup.Particles[0]);
        }
    }
}