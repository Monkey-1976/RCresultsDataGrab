using Microsoft.Extensions.Logging;

namespace GrabDRCCData;

public sealed class WriteFile : IDisposable
{
    
     public WriteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting file - {ex.Message}");
            }

            sw = new StreamWriter(filePath, true);
        }

        public void Write(string line)
        {
            sw.WriteLine(line);
            sw.Flush();
        }
        public void Close()
        {
            sw.Close();
        }
        public void Dispose()
        {
            if (sw != null)
            {
                sw.Dispose();
            }
        }
        private StreamWriter sw;
        private static ILogger _logger = Log.Logger;
}