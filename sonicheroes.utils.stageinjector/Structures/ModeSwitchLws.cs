using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SonicHeroes.Utils.StageInjector.Structures
{
    [StructLayout(LayoutKind.Sequential, Size = 0x18)]
    public struct ModeSwitchLws
    {
        public int field_0;
        public int some_counter;
        public int field_8;
        public int systemMode;
        public int field_10;
        public int field_14;
    }
}
