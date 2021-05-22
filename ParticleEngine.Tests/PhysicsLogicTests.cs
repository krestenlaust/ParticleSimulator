using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

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
            Physics.Instantiate<Particles.Sand>(new Vector2(0, 0));
            Physics.Update();

            Assert.AreEqual(new Vector2(0, 1), Physics.GetParticleGroup<Particles.Sand>().Particles[0]);

            Physics.Update();
            Assert.AreEqual(new Vector2(0, 2), Physics.GetParticleGroup<Particles.Sand>().Particles[0]);
        }
    }
}
