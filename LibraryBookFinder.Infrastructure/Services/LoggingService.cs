namespace LibraryBookFinder.Infrastructure.Services
{
    using Infrastructure.Interfaces;
    using System;
    using System.IO;
    using System.Reflection;

    public class LoggingService : ILoggingService
    {
        private string rootFolder;
        private string fileNamePrefix;

        public LoggingService()
        {
            this.fileNamePrefix = Assembly.GetExecutingAssembly().GetName().Name;
            this.rootFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                this.fileNamePrefix);
        }

        public async void LogException(Exception ex)
        {
            if (!Directory.Exists(this.rootFolder))
            {
                Directory.CreateDirectory(this.rootFolder);
            }

            string filePath = Path.Combine(this.rootFolder, $"{this.fileNamePrefix}-{DateTime.Now.ToString("yyyyMMddhhmmssfff")}.log");

            using (StreamWriter writer = new StreamWriter(new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write)))
            {
                await writer.WriteAsync(ex.ToString());
            }
        }
    }
}
