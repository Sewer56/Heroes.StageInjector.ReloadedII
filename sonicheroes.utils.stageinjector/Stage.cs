using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Reloaded.Memory;
using Reloaded.Universal.Redirector.Interfaces;
using SonicHeroes.Utils.StageInjector.Common;
using SonicHeroes.Utils.StageInjector.Common.Shared;
using SonicHeroes.Utils.StageInjector.Common.Shared.Enums;
using SonicHeroes.Utils.StageInjector.Common.Shared.Splines;
using SonicHeroes.Utils.StageInjector.Common.Structs.Positions.Substructures;
using SonicHeroes.Utils.StageInjector.Common.Structs.Splines;
using SonicHeroes.Utils.StageInjector.Common.Utilities;

namespace SonicHeroes.Utils.StageInjector
{
    public abstract unsafe class Stage
    {
        public StageId        StageId           { get; protected set; }
        public PositionStart* StartPositions    { get; protected set; }
        public PositionEnd*   EndPositions      { get; protected set; }
        public PositionEnd*   BragPositions     { get; protected set; }
        public Spline**       Splines           { get; protected set; }
    }
}
