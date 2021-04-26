using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RUtils.Logging
{
    public delegate void LogListener(Level level, string tag, string content);

    public class Logger
    {
        public static Logger Default { get; set; } = new Logger();
        public TextWriter Out { get; set; }
        public Level Level { get; set; } = Level.Default;
        public string DateTimeFormat { get; set; } = "dd/MM/yyyy HH:mm:ss";

        public event LogListener Logged;

        public Logger() : this(Console.Out)
        {

        }

        public Logger(TextWriter output)
        {
            Out = output;
        }

        public void Write(Level level, string tag, string content)
        {
            if (Level.HasFlag(level))
            {
                Logged?.Invoke(level, tag, content);
                Out?.WriteLine($"[{DateTime.Now.ToString(DateTimeFormat)}][{level}][{tag}]: {content}");
            }
        }

        public void Info(string tag, string content)
        {
            Write(Level.Info, tag, content);
        }

        public void Error(string tag, string content)
        {
            Write(Level.Error, tag, content);
        }

        public void Error(string tag, string content, Exception e)
        {
            Write(Level.Error, tag, $"{content}\r\n{e}");
        }

        public void Warn(string tag, string content)
        {
            Write(Level.Warn, tag, content);
        }

        public void Debug(string tag, string content)
        {
            Write(Level.Debug, tag, content);
        }
    }
}
