using Biosim.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Biosim.Tests
{
    [TestClass]
    public class GenomeFunctionsTests
    {
        [TestMethod]
        public void MakeRandomGenome_CreatesGenomeOfValidLength()
        {
            // Arrange
            Parameters.GenomeInitialLengthMin = 10;
            Parameters.GenomeInitialLengthMax = 100;

            // Act
            var genome = GenomeFunctions.MakeRandomGenome();

            // Assert
            Assert.IsTrue(genome.Count >= Parameters.GenomeInitialLengthMin);
            Assert.IsTrue(genome.Count <= Parameters.GenomeInitialLengthMax);
        }

        [TestMethod]
        public void CropLength_ReducesGenomeLength()
        {
            // Arrange
            var genome = GenomeFunctions.MakeRandomGenome();
            int newLength = genome.Count / 2;

            // Act
            GenomeFunctions.CropLength(genome, newLength);

            // Assert
            Assert.AreEqual(newLength, genome.Count);
        }

        [TestMethod]
        public void ApplyPointMutations_MutatesGenome()
        {
            // Arrange
            var genome = GenomeFunctions.MakeRandomGenome();
            var originalGenome = new List<Gene>(genome);

            // Act
            GenomeFunctions.ApplyPointMutations(genome);

            // Assert
            CollectionAssert.AreNotEqual(originalGenome, genome);
        }

        // Additional tests for other methods in GenomeFunctions
    }
}