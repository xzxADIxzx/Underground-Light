namespace Light.IO;

using Godot;
using System;
using System.Runtime.InteropServices;
using System.Text;

/// <summary> Wrapper over Marshal for convenience and the ability to read floating point numbers. </summary>
public class Reader
{
    /// <summary> Current cursor position. </summary>
    public int Position;
    /// <summary> Allocated memory length. </summary>
    public readonly int Length;
    /// <summary> Pointer to the allocated memory. </summary>
    public readonly IntPtr mem;

    /// <summary> Creates a reader with the given memory. </summary>
    public Reader(IntPtr memory, int length) { mem = memory; Length = length; }

    /// <summary> Wraps the given memory into a reader. </summary>
    public static void Read(IntPtr memory, int length, Cons<Reader> cons) => cons(new(memory, length));

    /// <summary> Converts integer to float. </summary>
    public static unsafe float Int2Float(int value) => *(float*)&value;
    /// <summary> Converts int to uint. </summary>
    public static unsafe uint Int2Uint(int value) => *(uint*)&value;

    /// <summary> Moves the cursor by the given amount of bytes and returns the old cursor position. </summary>
    public int Inc(int amount)
    {
        if (Position < 0) throw new IndexOutOfRangeException("Attempt to read data at a negative index.");
        Position += amount;

        if (Position > Length) throw new IndexOutOfRangeException("Attempt to read more bytes than were allocated in the memory.");
        return Position - amount;
    }

    #region types

    public bool Bool() => Marshal.ReadByte(mem, Inc(1)) == 0xFF;

    public void Bools(out bool b0, out bool b1, out bool b2, out bool b3, out bool b4, out bool b5, out bool b6, out bool b7)
    {
        byte value = Byte();
        b0 = (value & 1 << 0) != 0; b1 = (value & 1 << 1) != 0; b2 = (value & 1 << 2) != 0; b3 = (value & 1 << 3) != 0;
        b4 = (value & 1 << 4) != 0; b5 = (value & 1 << 5) != 0; b6 = (value & 1 << 6) != 0; b7 = (value & 1 << 7) != 0;
    }

    public byte Byte() => Marshal.ReadByte(mem, Inc(1));

    public byte[] Bytes(int amount) => Bytes(0, amount);

    public byte[] Bytes(int start, int amount)
    {
        var bytes = new byte[amount];
        Marshal.Copy(mem + Inc(amount), bytes, start, amount);
        return bytes;
    }

    public int Int() => Marshal.ReadInt32(mem, Inc(4));

    public float Float() => Int2Float(Marshal.ReadInt32(mem, Inc(4)));

    public uint Id() => Int2Uint(Marshal.ReadInt32(mem, Inc(4)));

    public string String() => Encoding.Unicode.GetString(Bytes(Int()));

    public Vector3 Vector() => new(Float(), Float(), Float());

    public Color Color() => new(Int2Uint(Int()));

    public T Enum<T>() where T : Enum => (T)System.Enum.ToObject(typeof(T), Byte());

    #endregion
}
