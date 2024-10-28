namespace Light;

using Godot;
using System;

/// <summary> Custom logger used only by the game for convenience. </summary>
public class Log
{
    /// <summary> Time format used by the logger. </summary>
    public const string TIME_FORMAT = "yyyy.MM.dd HH:mm:ss";
    /// <summary> Formatted regional time. </summary>
    public static string Time => DateTime.Now.ToString(TIME_FORMAT);

    /// <summary> Formats and writes the message to the output point. </summary>
    public static void LogLevel(Level level, string msg)
    {
        msg = $"[{Time}] [{(char)level}] {msg}";

        GD.PrintRich($"[color={level switch
        {
            Level.Debug   => "#BBBBBB",
            Level.Info    => "#FFFFFF",
            Level.Warning => "#FFA500",
            Level.Error   => "#FF341C",
            _             => "#FF66CC"
        }}]{msg}[/color]");
    }

    public static void Debug(string msg) => LogLevel(Level.Debug, msg);

    public static void Info(string msg) => LogLevel(Level.Info, msg);

    public static void Warning(string msg) => LogLevel(Level.Warning, msg);

    public static void Error(string msg) => LogLevel(Level.Error, msg);

    /// <summary> Log importance levels. </summary>
    public enum Level
    {
        Debug   = 'D',
        Info    = 'I',
        Warning = 'W',
        Error   = 'E'
    }
}
