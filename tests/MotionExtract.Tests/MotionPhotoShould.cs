namespace MotionExtract.Tests;

public class MotionPhotoShould
{
    [Fact]
    public void Should_Produce_One_JpgData()
    {
        // Arrange
        var baseFile = new FileInfo("SyntheticFiles/OnePOneV.MP.jpg");
        var photoVideoBase = new MotionPhoto(baseFile);

        // Act
        photoVideoBase.Extract();

        // Assert
        Assert.Single(photoVideoBase.JpgData);
    }

    [Fact]
    public void Should_Load_Correct_Jpg_Sequence_Data()
    {
        // Arrange
        var baseFile = new FileInfo("SyntheticFiles/OnePOneV.MP.jpg");
        var photoVideoBase = new MotionPhoto(baseFile);

        // Act
        photoVideoBase.Extract();

        // Assert
        Assert.Equal(8, photoVideoBase.JpgData[0].Length);
    }

    [Fact]
    public void Should_Produce_One_MpgData()
    {
        // Arrange
        var baseFile = new FileInfo("SyntheticFiles/OnePOneV.MP.jpg");
        var photoVideoBase = new MotionPhoto(baseFile);

        // Act
        photoVideoBase.Extract();

        // Assert
        Assert.Single(photoVideoBase.Mp4Data);
    }

    [Fact]
    public void Should_Load_Correct_Mp4_Sequence_Data()
    {
        // Arrange
        var baseFile = new FileInfo("SyntheticFiles/OnePOneV.MP.jpg");
        var photoVideoBase = new MotionPhoto(baseFile);

        // Act
        photoVideoBase.Extract();

        // Assert
        Assert.Equal(24, photoVideoBase.Mp4Data[0].Length);
    }

    [Fact]
    public void Should_Have_Zero_Data_Blocks()
    {
        // Arrange
        var baseFile = new FileInfo("SyntheticFiles/OneV.jpg");
        var photoVideoBase = new MotionPhoto(baseFile);

        // Act
        photoVideoBase.Extract();

        // Assert
        Assert.Empty(photoVideoBase.JpgData);
        Assert.Empty(photoVideoBase.Mp4Data);
    }

    [Fact]
    public void Should_Have_Zero_Data_Blocks_For_Non_Motion_Photo()
    {
        // Arrange
        var baseFile = new FileInfo("SyntheticFiles/EmptyFile.jpg");
        var photoVideoBase = new MotionPhoto(baseFile);

        // Act
        photoVideoBase.Extract();

        // Assert
        Assert.Empty(photoVideoBase.JpgData);
        Assert.Empty(photoVideoBase.Mp4Data);
    }
}
