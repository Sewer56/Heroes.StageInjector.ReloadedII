using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using SonicHeroes.Utils.StageInjector.Common.Shared.Enums;

namespace SonicHeroes.Utils.StageInjector.Structures
{
    [StructLayout(LayoutKind.Sequential, Size = 0x38)]
    public unsafe struct TeamTop
    {
        private fixed byte gap0[0x34];
        public Teams CurrentTeam;
    }
}
