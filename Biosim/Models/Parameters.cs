using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biosim.Models
{
    public static class Parameters
    {
        // Simulation settings
        public static int PopulationSize { get; set; } = 100;
        public static int GridWidth { get; set; } = 100;
        public static int GridHeight { get; set; } = 100;

        // Evolutionary settings
        public static double MutationRate { get; set; } = 0.01;
        public static int GenomeLength { get; set; } = 64;

        // Creature behavior settings
        public static double ReproductionThreshold { get; set; } = 0.5;
        public static int MaxAge { get; set; } = 100;

        // Add more parameters as needed
        // Define the maximum number of neurons
        public const ushort MaxNumberNeurons = 1000;  // Adjust this number based on your needs

        // You may have other parameters here, like:
        public const ushort MaxNumberSensors = 10;
        public const ushort MaxNumberActions = 10;
        // Other parameters can be defined here as well
        public static int GenomeInitialLengthMin = 10;
        public static int GenomeInitialLengthMax = 100;
        public static double GeneInsertionDeletionRate = 0.1; // example value
        public static double DeletionRatio = 0.5; // example value
        public static int GenomeMaxLength = 200; // example value
        public static double PointMutationRate = 0.01; // Example value

        // Optional: Load parameters from a file or another source
        public static void LoadFromConfig(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Configuration file not found.");
            }

            var json = File.ReadAllText(filePath);
            var config = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            if (config.ContainsKey("PopulationSize")) PopulationSize = Convert.ToInt32(config["PopulationSize"]);
            if (config.ContainsKey("GridWidth")) GridWidth = Convert.ToInt32(config["GridWidth"]);
            if (config.ContainsKey("GridHeight")) GridHeight = Convert.ToInt32(config["GridHeight"]);
            if (config.ContainsKey("MutationRate")) MutationRate = Convert.ToDouble(config["MutationRate"]);
            if (config.ContainsKey("GenomeLength")) GenomeLength = Convert.ToInt32(config["GenomeLength"]);
            // Load additional parameters as needed

        }
    }
}
