namespace MotionExtract.Tests;

public class MotionPhotoShould
{
    [Theory]
    [InlineData("SyntheticFiles/OnePOneV.MP.jpg")]
    [InlineData("CameraFiles/PXL_20220613_003727701.MP.jpg")]
    public void Should_Produce_One_JpgData(string filePath)
    {
        // Arrange
        var baseFile = new FileInfo(filePath);
        var motionPhoto = new MotionPhoto(baseFile);

        // Act
        motionPhoto.Extract();

        // Assert
        Assert.NotEmpty(motionPhoto.JpgData);
    }

    [Theory]
    [InlineData("SyntheticFiles/OnePOneV.MP.jpg")]
    [InlineData("CameraFiles/PXL_20220613_003727701.MP.jpg")]
    public void Should_Produce_One_MpgData(string filePath)
    {
        // Arrange
        var baseFile = new FileInfo(filePath);
        var motionPhoto = new MotionPhoto(baseFile);

        // Act
        motionPhoto.Extract();

        // Assert
        Assert.NotEmpty(motionPhoto.Mp4Data);
    }

    [Fact]
    public void Should_Load_Correct_Jpg_Sequence_Data()
    {
        // Arrange
        var baseFile = new FileInfo("SyntheticFiles/OnePOneV.MP.jpg");
        var motionPhoto = new MotionPhoto(baseFile);

        // Act
        motionPhoto.Extract();

        // Assert
        Assert.Equal(8, motionPhoto.JpgData.Length);
    }

    [Fact]
    public void Should_Have_Jpg_Segment_Matching_Output()
    {
        // Arrange
        var baseFile = new FileInfo("CameraFiles/PXL_20220613_003727701.MP.jpg");
        var expectedJpgSequence = File.ReadAllBytes("CameraFiles/ExpectedOutput/PXL_20220613_003727701.MP_photo.jpg");
        var motionPhoto = new MotionPhoto(baseFile);

        // Act
        motionPhoto.Extract();

        // Assert
        Assert.Equal(expectedJpgSequence, motionPhoto.JpgData);
    }

    [Fact]
    public void Should_Load_Correct_Mp4_Sequence_Data()
    {
        // Arrange
        var baseFile = new FileInfo("SyntheticFiles/OnePOneV.MP.jpg");
        var motionPhoto = new MotionPhoto(baseFile);

        // Act
        motionPhoto.Extract();

        // Assert
        Assert.Equal(24, motionPhoto.Mp4Data.Length);
    }

    [Fact]
    public void Should_Have_Mp4_Segment_Matching_Output()
    {
        // Arrange
        var baseFile = new FileInfo("CameraFiles/PXL_20220613_003727701.MP.jpg");
        var expectedMp4Sequence = File.ReadAllBytes("CameraFiles/ExpectedOutput/PXL_20220613_003727701.MP_video.mp4");
        var motionPhoto = new MotionPhoto(baseFile);

        // Act
        motionPhoto.Extract();

        // Assert
        Assert.Equal(expectedMp4Sequence, motionPhoto.Mp4Data);
    }

    [Fact]
    public void Should_Have_Zero_Data_Blocks()
    {
        // Arrange
        var baseFile = new FileInfo("SyntheticFiles/OneV.jpg");
        var motionPhoto = new MotionPhoto(baseFile);

        // Act
        motionPhoto.Extract();

        // Assert
        Assert.Empty(motionPhoto.JpgData);
        Assert.Empty(motionPhoto.Mp4Data);
    }

    [Fact]
    public void Should_Have_Zero_Data_Blocks_For_Non_Motion_Photo()
    {
        // Arrange
        var baseFile = new FileInfo("SyntheticFiles/EmptyFile.jpg");
        var motionPhoto = new MotionPhoto(baseFile);

        // Act
        motionPhoto.Extract();

        // Assert
        Assert.Empty(motionPhoto.JpgData);
        Assert.Empty(motionPhoto.Mp4Data);
    }

    [Fact]
    public void Should_Have_Valid_Data_When_Both_Populated()
    {
        // Arrange
        var baseFile = new FileInfo("CameraFiles/PXL_20220613_003727701.MP.jpg");
        var motionPhoto = new MotionPhoto(baseFile);
        motionPhoto.Extract();

        // Act
        var result = motionPhoto.HasValidData();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Should_Not_Have_Valid_Data_When_Both_Empty()
    {
        // Arrange
        var baseFile = new FileInfo("SyntheticFiles/EmptyFile.jpg");
        var motionPhoto = new MotionPhoto(baseFile);
        motionPhoto.Extract();

        // Act
        var result = motionPhoto.HasValidData();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Should_Not_Have_Valid_Data_When_Only_Jpg_Data()
    {
        // Arrange
        var baseFile = new FileInfo("SyntheticFiles/EmptyFile.jpg");
        var motionPhoto = new MotionPhoto(baseFile);
        motionPhoto.JpgData = [0xFF, 0xD8, 0xFF, 0xD9]; // Fake JPG data

        // Act
        var result = motionPhoto.HasValidData();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Should_Not_Have_Valid_Data_When_Only_Mp4_Data()
    {
        // Arrange
        var baseFile = new FileInfo("SyntheticFiles/EmptyFile.jpg");
        var motionPhoto = new MotionPhoto(baseFile);
        motionPhoto.Mp4Data = [0x66, 0x74, 0x79, 0x70]; // Fake MP4 data

        // Act
        var result = motionPhoto.HasValidData();

        // Assert
        Assert.False(result);
    }
}
