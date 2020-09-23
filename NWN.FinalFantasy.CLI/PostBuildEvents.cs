using System;
using System.IO;
using Newtonsoft.Json;
using NWN.FinalFantasy.CLI.Model;

namespace NWN.FinalFantasy.CLI
{
    public class PostBuildEvents
    {
        public void Process(string buildOutDirectory)
        {
            // Make sure the build output directory exists.
            if (!Directory.Exists(buildOutDirectory))
            {
                Console.WriteLine("Build out directory could not be found.");
                return;
            }

            var directory = GetNWNDirectory();
            if (string.IsNullOrWhiteSpace(directory)) return;

            var path = directory + "dotnet/";

            // Wipe the dotnet folder
            CleanDotnetFolder(path);

            // Copy all directories in the build out directory to the dotnet folder.
            foreach (var dir in Directory.GetDirectories(buildOutDirectory, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dir.Replace(buildOutDirectory, path));
            }

            // Copy all files in the build out directory to the dotnet folder.
            foreach (var file in Directory.GetFiles(buildOutDirectory, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(file, file.Replace(buildOutDirectory, path), true);
            }

            Console.WriteLine("Post build event finished successfully.");
        }

        /// <summary>
        /// Attempts to locate NWN at the default location. If it can't be found, the PostBuildEvents.json config file
        /// will be read and that directory will be used instead, if available.
        /// If NWN can't be found at all, a warning will be displayed to the console and null will be returned.
        /// </summary>
        /// <returns>The directory to use or null if not found.</returns>
        private string GetNWNDirectory()
        {
            var defaultNWNDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\Neverwinter Nights\\";
            const string ConfigFilePath = "./PostBuildEvents.json";
            var directory = defaultNWNDirectory;

            const string UnableToFindNWNError = "Warning: NWN directory could not be determined. Skipping post build events.";

            // Check for config file first.
            if (File.Exists(ConfigFilePath))
            {
                var content = File.ReadAllText(ConfigFilePath);
                var config = JsonConvert.DeserializeObject<PostBuildEventsConfig>(content);

                directory = config.NWNPath;
            }

            // If directory value isn't configured in the config file, set it to the default.
            if(string.IsNullOrWhiteSpace(directory))
            {
                directory = defaultNWNDirectory;
            }

            // Default NWN directory doesn't exist. Check for a config file
            if ( !Directory.Exists(directory))
            {
                Console.WriteLine(UnableToFindNWNError);
                return null;
            }

            return directory;
        }

        private void CleanDotnetFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return;
            }

            var directoryInfo = new DirectoryInfo(path);

            foreach (var file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            foreach (var directory in directoryInfo.GetDirectories())
            {
                directory.Delete(true);
            }
        }
    }
}
