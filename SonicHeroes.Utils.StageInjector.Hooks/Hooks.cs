using System;
using System.Runtime.InteropServices;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X86;
using Reloaded.Hooks.ReloadedII.Interfaces;
using SonicHeroes.Utils.StageInjector.Common.Shared.Enums;
using SonicHeroes.Utils.StageInjector.Common.Shared.Splines;
using SonicHeroes.Utils.StageInjector.Common.Structs.Positions.Substructures;
using static Reloaded.Hooks.Definitions.X86.FunctionAttribute;

namespace SonicHeroes.Utils.StageInjector.Hooks
{
    public unsafe class Hooks
    {
        /* This class exists because the EndPositionHook is giving problems calling the original function when being
           optimized if optimizations are enabled. This is a compiler bug, however given the lack of way to repro it,
           I can't report it.
        */

        public IHook<InitPath>         InitPathHook         { get; private set; }
        public IHook<GetStartPosition> GetStartPositionHook { get; private set; }
        public IHook<GetEndPosition>   GetEndPositionHook   { get; private set; }
        public IHook<GetBragPosition>  GetBragPositionHook   { get; private set; }

        public Hooks(IReloadedHooks hooks, InitPath initPath, GetStartPosition getStartPosition, GetEndPosition getEndPosition, GetBragPosition getBragPosition)
        {
            InitPathHook            = hooks.CreateHook(initPath, 0x00439020).Activate();
            GetStartPositionHook    = hooks.CreateHook(getStartPosition, 0x00426F10).Activate();
            GetEndPositionHook      = hooks.CreateHook(getEndPosition, 0x00426FD0).Activate();
            GetBragPositionHook     = hooks.CreateHook(getBragPosition, 0x00427010).Activate();
        }

        /// <summary>
        /// Retrieves the end position for a given team.
        /// </summary>
        /// <param name="team">The team to get the position for.</param>
        [Function(Register.esi, Register.eax, StackCleanup.Caller)]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate PositionEnd* GetEndPosition(Teams team); // 00426FD0

        /// <summary>
        /// Retrieves the start position for a given team.
        /// </summary>
        /// <param name="team">The team to get the position for.</param>
        [Function(Register.eax, Register.eax, StackCleanup.Caller)]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate PositionStart* GetStartPosition(Teams team); // 00426F10

        /// <summary>
        /// Retrieves the start position for a given team.
        /// </summary>
        /// <param name="team">The team to get the position for.</param>
        [Function(Register.eax, Register.eax, StackCleanup.Caller)]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate PositionEnd* GetBragPosition(Teams team); // 00427010

        /// <summary>
        /// Loads a spline given a pointer to an array of pointers to splines.
        /// </summary>
        /// <param name="splinePointerArray">Address of array of pointers to splines.</param>
        /// <returns>A value of 1 or 0 for success/failure.</returns>
        [Function(CallingConventions.Cdecl)]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool InitPath(Spline** splinePointerArray);
    }
}
