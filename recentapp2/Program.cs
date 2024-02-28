using System;
using System.IO;
using System.Linq;

class RecentlyOpenedExecutables
{
    static void Main()
    {

        string searchDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

        try
        {
            var executables = Directory.GetFiles(searchDirectory, "*.exe", SearchOption.AllDirectories)
                .Where(file => !file.StartsWith(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "System32"), StringComparison.OrdinalIgnoreCase))
                .Select(file => new FileInfo(file))
                .OrderByDescending(fileInfo => fileInfo.LastAccessTime)
                .Take(20)
                .ToList();

            string outputFile = "RecentlyOpenedExecutables.txt";
            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                writer.WriteLine("Recently Opened Executables:");
                foreach (var executable in executables)
                {
                    writer.WriteLine($"{executable.Name} - Full Path: {executable.FullName} - Last Access Time: {executable.LastAccessTime}");
                }
            }

            Console.WriteLine($"List of recently opened executables has been saved to {outputFile}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine("Access to some directories was denied. Check permissions and run the program with elevated privileges if necessary.");
        }
    }
}
