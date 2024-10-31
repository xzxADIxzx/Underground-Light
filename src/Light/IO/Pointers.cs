namespace Light.IO;

using System;
using System.Runtime.InteropServices;

/// <summary> Static class responsible for managing the memory allocated for writes. </summary>
public class Pointers
{
    /// <summary> Amount of bytes that will be allocated. </summary>
    public const int RESERVED = 64 * 1024;

    /// <summary> Pointer to the beginning of the allocated memory. </summary>
    private static IntPtr pointer;
    /// <summary> Amount of bytes reserved for writers. </summary>
    private static int offset;

    /// <summary> Allocates 64kb for future reservation. </summary>
    public static void Allocate() => pointer = Marshal.AllocHGlobal(RESERVED);

    /// <summary> Reserves the given amount of bytes of the allocated memory. </summary>
    public static IntPtr Reserve(int bytes)
    {
        var reserved = pointer + offset;

        if ((offset += bytes) >= RESERVED) throw new OutOfMemoryException("Attempt to reserve more bytes than were allocated in the memory.");

        return reserved;
    }

    /// <summary> Frees the memory reserved for writers. </summary>
    public static void Free() => offset = 0;
}
