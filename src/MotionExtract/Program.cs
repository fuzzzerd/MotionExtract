
namespace MotionExtract;

/// <summary>
/// Runs through a directory and splits Google Motion Photos into separate photo and video files.
/// Adapted from https://android.stackexchange.com/a/203898
/// Converted to C#.
/// </summary>
public static class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0 || !Directory.Exists(args[0]))
        {
            Console.WriteLine("Please enter a valid directory path:");
            var inputPath = Console.ReadLine();
            while (!Directory.Exists(inputPath))
            {
                Console.WriteLine("Invalid directory. Please enter a valid directory path:");
                inputPath = Console.ReadLine();
            }
            args = [inputPath];
        }

        string srcDir = args[0];

        // string srcDir = "/home/nate/Downloads/Trial";

        Console.WriteLine("Scanning for files...");

        // Get all files matching the given pattern
        var files = Directory.GetFiles(srcDir);

        foreach (var file in files)
        {
            if (PhotoVideoBase.TryGetPv(file, out var pvFile))
            {
                pvFile.Extract();
                pvFile.Save(Path.Combine(srcDir, "output"));
            }
            else
            {
                Console.WriteLine("SKIPPING - File does not appear to be a Google motion photo.");
            }
        }

        Console.WriteLine("Done.");
    }
}