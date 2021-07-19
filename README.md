# EZLeveler
Encounter Zone Scaler and Unleveler Synthesis Patcher for Skyrim SE

# Configuration
The patcher updates *MinLevel* and *MaxLevel* for every ECZN (Encounter Zone) record by scaling the *MinLevel* of the winning override. The patcher also normalizes the *DisableCombatBoundary* flag across all ECZNs.
```
{
  "minLevelScaleFactor": 0.5,
  "maxLevelScaleFactor": 2.0,
  "disableCombatBoundary": false
}
```
Generally, each ECZN will be able to adjust to level with the player a certain amount - a record with *MinLevel* 20 would be patched to have *MinLevel* 10 and *MaxLevel* 40 with the config above, for example.

*maxLevelScaleFactor* 0 removes the upper limit on leveling with the player. If *maxLevelScaleFactor * ECZN.MinLevel* > 128 this has the same effect, since 128 is an upper bound on *MaxLevel* and *MinLevel* values.

*minLevelScaleFactor* set the same as *maxLevelScaleFactor* enforces a strict level on the ECZN.
