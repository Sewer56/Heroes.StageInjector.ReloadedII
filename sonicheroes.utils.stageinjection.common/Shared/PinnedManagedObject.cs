using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using SonicHeroes.Utils.StageInjector.Common.Shared.Splines;

namespace SonicHeroes.Utils.StageInjector.Common.Shared
{
    /// <summary>
    /// Pins a managed object for interop with unmanaged code, exposing the address of object.
    /// Primarily used for arrays.
    /// </summary>
    public class PinnedManagedObject<TObject> : IDisposable
    {
        public TObject  Object { get; set; }
        public IntPtr   ObjectPtr { get; private set; }
        public GCHandle Handle { get; private set; }

        public PinnedManagedObject(TObject @struct)
        {
            Object = @struct;
            Handle = GCHandle.Alloc(Object, GCHandleType.Pinned);
            ObjectPtr = Handle.AddrOfPinnedObject();
        }

        ~PinnedManagedObject()
        {
            Dispose();
        }

        public void Dispose()
        {
            Handle.Free();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Returns the pointer to managed object as a pointer to an unmanaged type.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe TPtr* AsPointer<TPtr>() where TPtr : unmanaged
        {
            return (TPtr*)ObjectPtr;
        }
    }
}
