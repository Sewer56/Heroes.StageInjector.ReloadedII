using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SonicHeroes.Utils.StageInjector.Structures
{
    [StructLayout(LayoutKind.Sequential, Size = 0x2C)]
    public struct ModeSwitchFlags
    {
        public bool field_0;
        public bool field_1;
        public bool field_2;
        public bool field_3;
        public bool field_4;
        public bool field_5;
        public bool field_6;
        public bool field_7;
        public bool field_8;
        public bool field_9;
        public bool field_A;
        public bool field_B;
        public bool field_C;
        public bool field_D;
        public bool field_E;
        public bool field_F;
        public bool field_10;
        public bool maybe_video_mode;
        public bool maybe_audio;
        public bool lang_id;
        public bool maybe_speech;
        public bool field_15;
        public bool field_16;
        public bool field_17;
        public bool demoMode;
        public bool field_19;
        public bool field_1A;
        public bool field_1B;
        public bool field_1C;
        public bool field_1D;
        public bool twoPlayerMode;
        public bool field_1F;
        public bool field_20;
        public bool field_21;
        public bool field_22;
        public bool field_23;
        public bool field_24;
        public bool field_25;
        public bool field_26;
        public bool storyModeFlag;
        public bool missionID;
        public bool someResolutionRelated;
        public bool field_2A;
        public bool field_2B;
    }
}
