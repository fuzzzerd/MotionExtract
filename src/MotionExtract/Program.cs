
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MotionExtract;

/// <summary>
/// Runs through a directory and splits Google Motion Photos into separate photo and video files.
/// Adapted from https://android.stackexchange.com/a/203898
/// Converted to C#.
/// </summary>
[ExcludeFromCodeCoverage]
public static class Program
{
    static int Main(string[] args)
    {
        // Parse arguments
        var parsedArgs = ParseArguments(args);

        // Handle help and version flags
        if (parsedArgs.ShowHelp)
        {
            ShowHelp();
            return 0;
        }

        if (parsedArgs.ShowVersion)
        {
            ShowVersion();
            return 0;
        }

        // Get directory path
        string srcDir;
        if (string.IsNullOrEmpty(parsedArgs.SourceDirectory))
        {
            Console.WriteLine("Please enter a valid directory path:");
            var inputPath = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(inputPath) || !Directory.Exists(inputPath))
            {
                WriteError("Invalid directory. Please enter a valid directory path:");
                inputPath = Console.ReadLine();
            }
            srcDir = inputPath;
        }
        else if (!Directory.Exists(parsedArgs.SourceDirectory))
        {
            WriteError($"Directory not found: {parsedArgs.SourceDirectory}");
            return 1;
        }
        else
        {
            srcDir = parsedArgs.SourceDirectory;
        }

        // Determine output directory
        var outputDir = parsedArgs.OutputDirectory ?? Path.Combine(srcDir, "output");

        // Get all files matching the given pattern
        WriteInfo($"Scanning for files in: {srcDir}");
        var files = Directory.GetFiles(srcDir);

        if (files.Length == 0)
        {
            WriteWarning("No files found in directory.");
            return 0;
        }

        WriteInfo($"Found {files.Length} file(s)");
        WriteInfo($"Output directory: {outputDir}");
        Console.WriteLine();

        // Track statistics
        var processed = 0;
        var extracted = 0;
        var skipped = 0;
        var errors = 0;

        // Process each file
        foreach (var file in files)
        {
            processed++;
            var fileName = Path.GetFileName(file);

            if (TryGetPv(file, out var pvFile))
            {
                try
                {
                    Console.Write($"[{processed}/{files.Length}] {fileName}... ");

                    pvFile.Extract();

                    if (pvFile.HasValidData())
                    {
                        pvFile.Save(outputDir);
                        WriteSuccess("✓");
                        extracted++;
                    }
                    else
                    {
                        WriteWarning("⚠ Not a motion photo");
                        skipped++;
                    }
                }
                catch (Exception ex)
                {
                    WriteError($"✗ Error: {ex.Message}");
                    errors++;
                }
            }
            else
            {
                Console.Write($"[{processed}/{files.Length}] {fileName}... ");
                WriteWarning("⚠ Skipped (not JPG)");
                skipped++;
            }
        }

        // Summary
        Console.WriteLine();
        Console.WriteLine("Summary:");
        WriteInfo($"  Total processed: {processed}");
        WriteSuccess($"  Extracted: {extracted}");
        if (skipped > 0) WriteWarning($"  Skipped: {skipped}");
        if (errors > 0) WriteError($"  Errors: {errors}");

        return errors > 0 ? 1 : 0;
    }

    [ExcludeFromCodeCoverage]
    private static ParsedArguments ParseArguments(string[] args)
    {
        var result = new ParsedArguments();

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];

            if (arg == "--help" || arg == "-h")
            {
                result.ShowHelp = true;
                return result;
            }

            if (arg == "--version" || arg == "-v")
            {
                result.ShowVersion = true;
                return result;
            }

            if (arg == "--output" || arg == "-o")
            {
                if (i + 1 < args.Length)
                {
                    result.OutputDirectory = args[++i];
                }
                else
                {
                    WriteError("Error: --output requires a directory path");
                    result.ShowHelp = true;
                    return result;
                }
                continue;
            }

            // If it's not a flag, it's the source directory
            if (string.IsNullOrEmpty(result.SourceDirectory) && !arg.StartsWith("-"))
            {
                result.SourceDirectory = arg;
            }
        }

        return result;
    }

    private record ParsedArguments
    {
        public string? SourceDirectory { get; set; }
        public string? OutputDirectory { get; set; }
        public bool ShowHelp { get; set; }
        public bool ShowVersion { get; set; }
    }

    [ExcludeFromCodeCoverage]
    private static void ShowHelp()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "1.0.0";

        Console.WriteLine($"MotionExtract v{version}");
        Console.WriteLine();
        Console.WriteLine("Extract Google Motion Photos into separate photo and video files.");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  MotionExtract <directory> [options]");
        Console.WriteLine("  MotionExtract --help");
        Console.WriteLine("  MotionExtract --version");
        Console.WriteLine();
        Console.WriteLine("Arguments:");
        Console.WriteLine("  <directory>              Path to directory containing motion photos");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  -o, --output <directory> Output directory for extracted files");
        Console.WriteLine("                           (default: <source-directory>/output)");
        Console.WriteLine("  -h, --help               Show this help message");
        Console.WriteLine("  -v, --version            Show version information");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  MotionExtract \"C:\\Photos\"");
        Console.WriteLine("  MotionExtract \"C:\\Photos\" --output \"D:\\Extracted\"");
        Console.WriteLine("  MotionExtract \"C:\\Photos\" -o \"D:\\Extracted\"");
        Console.WriteLine();
        Console.WriteLine("Output:");
        Console.WriteLine("  Files are saved as:");
        Console.WriteLine("    <filename>_photo.jpg - The still photo");
        Console.WriteLine("    <filename>_video.mp4 - The video clip");
    }

    [ExcludeFromCodeCoverage]
    private static void ShowVersion()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "1.0.0";
        Console.WriteLine($"MotionExtract v{version}");
    }

    [ExcludeFromCodeCoverage]
    private static void WriteSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    [ExcludeFromCodeCoverage]
    private static void WriteWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    [ExcludeFromCodeCoverage]
    private static void WriteError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    [ExcludeFromCodeCoverage]
    private static void WriteInfo(string message)
    {
        Console.WriteLine(message);
    }

    [ExcludeFromCodeCoverage]
    public static bool TryGetPv(string filePath, out IPhotoVideo file)
    {
        var fullFilePath = Path.GetFullPath(filePath);

        if (!File.Exists(fullFilePath))
        {
            file = default!;
            return false;
        }

        if (new[] { "jpeg", "jpg" }.Contains(Path.GetExtension(filePath).TrimStart('.'), StringComparer.OrdinalIgnoreCase))
        {
            file = new MotionPhoto(new FileInfo(fullFilePath));
            return true;
        }

        file = default!;
        return false;
    }
}
