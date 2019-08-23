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


        /* Autoimplemented by R# */
        protected bool Equals(Stage other)
        {
            return StageId == other.StageId &&
                   StartPositions == other.StartPositions &&
                   EndPositions == other.EndPositions &&
                   BragPositions == other.BragPositions &&
                   Splines == other.Splines;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Stage)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)StageId;
                hashCode = (hashCode * 397) ^ unchecked((int)(long)StartPositions);
                hashCode = (hashCode * 397) ^ unchecked((int)(long)EndPositions);
                hashCode = (hashCode * 397) ^ unchecked((int)(long)BragPositions);
                hashCode = (hashCode * 397) ^ unchecked((int)(long)Splines);
                return hashCode;
            }
        }
    }
}
