﻿using SonicHeroes.Utils.StageInjector.Common.Utilities;

namespace SonicHeroes.Utils.StageInjector.Common
{
    public class SplineFile : JsonSerializable<SplineFile>
    {
        public ManagedSpline[] Splines { get; set; }
    }
}