using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using SonicHeroes.Utils.StageInjector.Common.Shared.Splines;

namespace SonicHeroes.Utils.StageInjector.Common.Shared
{
    /// <summary>
    /// Wraps a native disposable unmanaged struct around a managed object, exposing its pointer.
    /// Provides garbage collection support
    /// </summary>
    public unsafe class PinnedDisposableNativeStruct<TStruct> : IDisposable where TStruct : unmanaged, IDisposable
    {
        public TStruct  Struct      { get; set; }
        public TStruct* StructPtr   { get; private set; }
        public GCHandle Handle      { get; private set; }

        public PinnedDisposableNativeStruct(TStruct @struct)
        {
            Struct = @struct;
            Handle = GCHandle.Alloc(Struct, GCHandleType.Pinned);
            StructPtr = (TStruct*) Handle.AddrOfPinnedObject();
        }

        ~PinnedDisposableNativeStruct()
        {
            Dispose();
        }

        public void Dispose()
        {
            Handle.Free();
            Struct.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
