namespace MotionExtract;

public class MotionPhoto(FileInfo baseFile) : PhotoVideoBase(baseFile)
{
    public override void Extract()
    {
        Console.WriteLine($"Processing: {BaseFile.FullName}, size: {BaseFile.Length} bytes");

        var data = File.ReadAllBytes(BaseFile.FullName);

        // Look for the position of the "ftyp" in the data to detect MP4 start
        var mp4StartPos = IndexOfFtyp(data);

        if (mp4StartPos != -1)
        {
            mp4StartPos -= 4; // the real beginning of the mp4 starts 4 bytes before "ftyp"

            // Look for the JPG end (FF D9)
            int jpgEndPos = IndexOfJpgEnd(data, mp4StartPos);

            if (jpgEndPos != -1)
            {
                jpgEndPos += 2; // account for the length of the search string

                JpgData.Add(data.Take(jpgEndPos).ToArray());
                Mp4Data.Add(data.Skip(mp4StartPos).ToArray());
            }
            else
            {
                Console.WriteLine("SKIPPING - File appears to contain an MP4 but no valid JPG EOI segment could be found.");
            }
        }
        else
        {
            Console.WriteLine("SKIPPING - File does not appear to be a Google motion photo.");
        }
    }

    /// <summary>
    /// Find the position of the "ftyp" pattern in the byte array
    /// </summary>
    static int IndexOfFtyp(byte[] data)
    {
        // C#11 style for: new byte[4] { 0x66, 0x74, 0x79, 0x70 };
        var patternBytes = "ftyp"u8.ToArray();
        for (var i = 0; i < data.Length - patternBytes.Length; i++)
        {
            if (data.Skip(i).Take(patternBytes.Length).SequenceEqual(patternBytes))
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Find the position of the JPG end (FF D9) in the byte array
    /// </summary>
    static int IndexOfJpgEnd(byte[] data, int mp4StartPos)
    {
        var jpgEnd = new byte[] { 0xFF, 0xD9 };
        for (var i = mp4StartPos; i >= 0; i--)
        {
            if (data.Skip(i).Take(jpgEnd.Length).SequenceEqual(jpgEnd))
            {
                return i;
            }
        }
        return -1;
    }
}
