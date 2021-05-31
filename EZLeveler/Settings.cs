using System;
using Mutagen.Bethesda.Synthesis.Settings;

namespace EZLeveler
{
    public class Settings
    {
        private double _minLevelScaleFactor;
        [SynthesisSettingName("MinLevel Scale Factor")]
        [SynthesisTooltip("Multiplier for ECZN.MinLevel field. Use 0.0 for no minimum.")]
        public double MinLevelScaleFactor { 
            get
            {
                return _minLevelScaleFactor;
            }
            set
            {
                _minLevelScaleFactor = Math.Max(value, 0.0);
            }
        }


        private double _maxLevelScaleFactor;
        [SynthesisSettingName("MaxLevel Scale Factor")]
        [SynthesisTooltip("Multiplier for ECZN.MaxLevel field. Use 0.0 for no maximum.")]
        public double MaxLevelScaleFactor {
            get
            {
                return _maxLevelScaleFactor;
            }
            set
            {
                _maxLevelScaleFactor = Math.Max(value, 0.0);
                if (_maxLevelScaleFactor > 0.0)
                    _maxLevelScaleFactor = Math.Max(_minLevelScaleFactor, _maxLevelScaleFactor);
            }
        }

        [SynthesisSettingName("Disable Combat Boundary")]
        [SynthesisTooltip("Override setting for combat boundary behaviour.")]
        public bool DisableCombatBoundary { get; set; }
    }
}
