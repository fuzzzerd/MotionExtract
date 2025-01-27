namespace MotionExtract;

public abstract class PhotoVideoBase(FileInfo baseFile)
{
    public FileInfo BaseFile { get; init; } = baseFile;

    public List<byte[]> JpgData { get; set; } = [];

    public List<byte[]> Mp4Data { get; set; } = [];

    public abstract void Extract();

    public virtual void Save(string outputDir)
    {
        var baseFileName = Path.GetFileNameWithoutExtension(BaseFile.FullName);

        for (var i = 0; i < JpgData.Count; i++)
        {
            var jpgFileName = $"{baseFileName}_photo_{i + 1}.jpg";
            File.WriteAllBytes(Path.Combine(outputDir, jpgFileName), JpgData[i]);
        }

        for (var i = 0; i < Mp4Data.Count; i++)
        {
            var mp4FileName = $"{baseFileName}__video_{i + 1}.mp4";
            File.WriteAllBytes(Path.Combine(outputDir, mp4FileName), Mp4Data[i]);
        }
    }

    public static bool TryGetPv(string filePath, out PhotoVideoBase file)
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
