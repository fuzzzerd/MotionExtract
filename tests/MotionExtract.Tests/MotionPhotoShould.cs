namespace MotionExtract.Tests;

public class MotionPhotoShould
{
    [Fact]
    public void Should_Produce_One_JpgData()
    {
        // Arrange
        var baseFile = new FileInfo("SyntheticFiles/OnePOneV.MP.jpg");
        var motionPhoto = new MotionPhoto(baseFile);

        // Act
        motionPhoto.Extract();

        // Assert
        Assert.NotEmpty(motionPhoto.JpgData);
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
    public void Should_Produce_One_MpgData()
    {
        // Arrange
        var baseFile = new FileInfo("SyntheticFiles/OnePOneV.MP.jpg");
        var motionPhoto = new MotionPhoto(baseFile);

        // Act
        motionPhoto.Extract();

        // Assert
        Assert.NotEmpty(motionPhoto.Mp4Data);
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
}
