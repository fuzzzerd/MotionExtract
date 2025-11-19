# MotionExtract

A command-line tool to split Google Motion Photos into separate photo and video files.

## What are Motion Photos?

Motion Photos (on Google Pixel phones) store a short video clip inside a JPG file. This tool extracts them into separate files.

## Usage

### Process a folder

```bash
# Windows
MotionExtract "C:\Users\YourName\Pictures\MotionPhotos"

# Linux/macOS
./MotionExtract "/home/user/Pictures/MotionPhotos"
```

### Specify output directory

```bash
# Use --output or -o to specify where to save extracted files
MotionExtract "C:\Photos" --output "D:\Extracted"

# Short form
MotionExtract "C:\Photos" -o "D:\Extracted"

# Without --output, files are saved to <source-directory>/output (default)
```

### Interactive mode

```bash
# Run without arguments to be prompted for a directory
MotionExtract
```

### Get help

```bash
MotionExtract --help
# or
MotionExtract -h
```

### Check version

```bash
MotionExtract --version
# or
MotionExtract -v
```

### Output

Files are saved to an `output` subdirectory:

- `<filename>_photo.jpg` - The still photo
- `<filename>_video.mp4` - The video clip

**Example:**

```text
Input:  C:\Photos\PXL_20220613_003727701.MP.jpg
Output: C:\Photos\output\PXL_20220613_003727701.MP_photo.jpg
        C:\Photos\output\PXL_20220613_003727701.MP_video.mp4
```

### Sample Output

```text
Scanning for files in: C:\Photos
Found 15 file(s)
Output directory: C:\Photos\output

[1/15] IMG_1234.bmp... ⚠ Skipped (not JPG)
[2/15] PXL_20220613_003727701.MP.jpg... ✓
[3/15] PXL_20220614_120000123.MP.jpg... ✓
...

Summary:
  Total processed: 15
  Extracted: 12
  Skipped: 3
```

### Exit Codes

- `0` - Success (all files processed without errors)
- `1` - Errors occurred during processing

## Requirements

- .NET 8.0 SDK or runtime

## Installation

```bash
# Build
dotnet build -c Release

# Executable will be in: src/MotionExtract/bin/Release/net8.0/
```

## Developer Commands

```bash
# Run tests
dotnet test

# Run application
dotnet run --project src/MotionExtract -- "C:\path\to\photos"

# Format code
dotnet format

# Clean
dotnet clean
```

## How It Works

Searches for the MP4 `ftyp` signature and JPG end marker (`FF D9`) to split the embedded files.

Based on [this solution](https://android.stackexchange.com/a/203898).
