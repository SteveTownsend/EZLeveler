using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;

namespace EZLeveler
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .Run(args, new RunPreferences()
                {
                    ActionsForEmptyArgs = new RunDefaultPatcher()
                    {
                        IdentifyingModKey = "LeveledEncounterZones.esp",
                        TargetRelease = GameRelease.SkyrimSE,
                    }
                });
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            string configFilePath = Path.Combine(state.ExtraSettingsDataPath, "config.json");
            Config configuration = new Config(configFilePath);
            foreach (var ez in state.LoadOrder.PriorityOrder.WinningOverrides<IEncounterZoneGetter>())
            {
                // Skip records that have not baseline to work from
                if (ez.MinLevel == 0)
                {
                    Console.WriteLine(String.Format("Skip ECZN {0} for LCTN {1}: it has no min level",
                        ez.FormKey.ToString(), ez.Location.FormKey.ToString()));
                    continue;
                }
                // Skip records that already match desired state
                sbyte newMin = (sbyte)((double)ez.MinLevel * configuration.MinLevelScaleFactor);
                sbyte newMax = (sbyte)((double)ez.MinLevel * configuration.MaxLevelScaleFactor);
                if ((ez.Flags.HasFlag(EncounterZone.Flag.DisableCombatBoundary) == configuration.DisableCombatBoundary) &&
                    (ez.MinLevel == newMin) && (ez.MaxLevel == newMax))
                    continue;

                var modifiedEZ = state.PatchMod.EncounterZones.GetOrAddAsOverride(ez);
                if (configuration.DisableCombatBoundary)
                {
                    modifiedEZ.Flags |= EncounterZone.Flag.DisableCombatBoundary;
                }
                else
                {
                    modifiedEZ.Flags &= ~EncounterZone.Flag.DisableCombatBoundary;
                }
                modifiedEZ.MinLevel = newMin;
                // underflow implies > max value: allow any scaling
                modifiedEZ.MaxLevel = (newMax < 0) ? (sbyte)0 : newMax;
                if (newMax > 0)
                {
                    Console.WriteLine(String.Format("Updated ECZN {0} @ LCTN {1} [{2} - {3}]",
                        modifiedEZ.FormKey.ToString(), modifiedEZ.Location.FormKey.ToString(), newMin, modifiedEZ.MaxLevel));
                }
                else
                {
                    Console.WriteLine(String.Format("Updated ECZN {0} @ LCTN {1} [{2}+]",
                        modifiedEZ.FormKey.ToString(), modifiedEZ.Location.FormKey.ToString(), newMin));
                }

            }
        }
    }
}
