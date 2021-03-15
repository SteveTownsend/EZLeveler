using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace EZLeveler
{
    public class Config
    {
        public double MinLevelScaleFactor { get; set; }
        public double MaxLevelScaleFactor { get; set; }
        public bool DisableCombatBoundary { get; set; }

        public Config(string configFilePath)
        {
            // default values
            double minLevelScaleFactor = 1.0;
            double maxLevelScaleFactor = 0.0;
            bool disableCombatBoundary = false;

            // override if config is well-formed
            if (!File.Exists(configFilePath))
            {
                Console.WriteLine("\"config.json\" cannot be found in the users Data folder, using default values.");
            }
            else
            {
                JObject configJson = JObject.Parse(File.ReadAllText(configFilePath));
                minLevelScaleFactor = (float)configJson["minLevelScaleFactor"]!;
                maxLevelScaleFactor = (float)configJson["maxLevelScaleFactor"]!;
                disableCombatBoundary = (bool)configJson["disableCombatBoundary"]!;
            }
            MinLevelScaleFactor = Math.Max(minLevelScaleFactor, 0.0);
            MaxLevelScaleFactor = Math.Max(maxLevelScaleFactor, 0.0);
            if (MaxLevelScaleFactor > 0.0)
                MaxLevelScaleFactor = Math.Max(MinLevelScaleFactor, MaxLevelScaleFactor);
            DisableCombatBoundary = disableCombatBoundary;
            Console.WriteLine(String.Format("Relevel Encounter Zones: minLevelScaleFactor={0} maxLevelScaleFactor={1} disableCombatBoundary={2}",
                MinLevelScaleFactor, MaxLevelScaleFactor, DisableCombatBoundary));
        }
    }
}
