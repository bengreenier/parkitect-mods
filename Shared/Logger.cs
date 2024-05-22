using System;
using System.IO;
using System.Text;

namespace Shared
{
    public class Logger : IDisposable
    {
        private readonly FileStream _file;
        
        public Logger(string name)
        {
            this._file = File.OpenWrite($"{name}.log");
        }

        public void Log(string message)
        {
            using var writer = new StreamWriter(this._file, Encoding.Default, 1024, true);
            
            writer.WriteLine($"{DateTime.Now:yyyy-dd-M--HH-mm-ss}: {message}");
        }

        public void Dispose()
        {
            _file?.Dispose();
        }
    } 
}