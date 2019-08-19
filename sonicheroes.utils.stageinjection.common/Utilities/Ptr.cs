namespace SonicHeroes.Utils.StageInjector.Common.Utilities
{
    /// <summary>
    /// Wraps an unmanaged pointer so .NET sees it as blittable.
    /// </summary>
    public unsafe struct Ptr<T> where T : unmanaged
    {
        public T* Pointer { get; set; }
        public Ptr(T* pointer)
        {
            Pointer = pointer;
        }

        public static implicit operator Ptr<T>(T* operand)
        {
            return new Ptr<T>(operand);
        }
    }
}
