namespace Light.IO;

using Godot;
using System;
using System.Runtime.InteropServices;
using System.Text;

/// <summary> Wrapper over Marshal for convenience and the ability to write floating point numbers. </summary>
public class Writer
{
    /// <summary> Current cursor position. </summary>
    public int Position;
    /// <summary> Allocated memory length. </summary>
    public readonly int Length;
    /// <summary> Pointer to the allocated memory. </summary>
    public readonly IntPtr mem;

    /// <summary> Creates a writer with the given memory. </summary>
    public Writer(IntPtr memory, int length) { mem = memory; Length = length; }

    /// <summary> Reserves the given memory amount in bytes and wraps it into a writer. </summary>
    public static void Write(Cons<Writer> cons, Cons<IntPtr, int> result, int memoryAmount)
    {
        Writer instance = new(Pointers.Reserve(memoryAmount), memoryAmount);
        cons(instance);
        result(instance.mem, instance.Position);
    }

    /// <summary> Converts float to integer. </summary>
    public static unsafe int Float2Int(float value) => *(int*)&value;
    /// <summary> Converts uint to int. </summary>
    public static unsafe int Uint2int(uint value) => *(int*)&value;

    /// <summary> Moves the cursor by the given amount of bytes and returns the old cursor position. </summary>
    public int Inc(int amount)
    {
        if (Position < 0) throw new IndexOutOfRangeException("Attempt to write data at a negative index.");
        Position += amount;

        if (Position > Length) throw new IndexOutOfRangeException("Attempt to write more bytes than were allocated in the memory.");
        return Position - amount;
    }

    #region types

    public void Bool(bool value) => Marshal.WriteByte(mem, Inc(1), (byte)(value ? 0xFF : 0x00));

    public void Bools(bool b0 = false, bool b1 = false, bool b2 = false, bool b3 = false, bool b4 = false, bool b5 = false, bool b6 = false, bool b7 = false) =>
        Byte((byte)((b0 ? 1 : 0) | (b1 ? 1 << 1 : 0) | (b2 ? 1 << 2 : 0) | (b3 ? 1 << 3 : 0) | (b4 ? 1 << 4 : 0) | (b5 ? 1 << 5 : 0) | (b6 ? 1 << 6 : 0) | (b7 ? 1 << 7 : 0)));

    public void Byte(byte value) => Marshal.WriteByte(mem, Inc(1), value);

    public void Bytes(byte[] value) => Bytes(value, 0, value.Length);

    public void Bytes(byte[] value, int start, int amount) => Marshal.Copy(value, start, mem + Inc(amount), amount);

    public void Int(int value) => Marshal.WriteInt32(mem, Inc(4), value);

    public void Float(float value) => Marshal.WriteInt32(mem, Inc(4), Float2Int(value));

    public void Id(uint value) => Marshal.WriteInt32(mem, Inc(4), Uint2int(value));

    public void String(string value)
    {
        value ??= "";
        Int(value.Length * 2);
        Bytes(Encoding.Unicode.GetBytes(value));
    }

    public void Vector(Vector3 value)
    {
        Float(value.X);
        Float(value.Y);
        Float(value.Z);
    }

    public void Color(Color value) => Int(Uint2int(value.ToRgba32()));

    public void Enum<T>(T value) where T : Enum => Byte(Convert.ToByte(value));

    #endregion
}
