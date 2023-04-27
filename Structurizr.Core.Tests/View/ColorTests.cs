
using Xunit;

namespace Structurizr.Core.Tests
{


  public class ColorTests
  {

    [Fact]
    public void Test_IsHexColorCode_ReturnsFalse_WhenPassedNull()
    {
      Assert.False(Color.IsValidColor(null));
    }

    [Fact]
    public void Test_IsHexColorCode_ReturnsFalse_WhenPassedAnEmptyString()
    {
      Assert.False(Color.IsValidColor(""));
    }

    [Fact]
    public void Test_IsHexColorCode_ReturnsFalse_WhenPassedAnInvalidString()
    {
      Assert.False(Color.IsHexColorCode("ffffff"));
      Assert.False(Color.IsHexColorCode("#fffff"));
      Assert.False(Color.IsHexColorCode("#gggggg"));
    }

    [Fact]
    public void Test_IsHexColorCode_ReturnsTrue_WhenPassedAnValidString()
    {
      Assert.True(Color.IsValidColor("#abcdef"));
      Assert.True(Color.IsValidColor("#ABCDEF"));
      Assert.True(Color.IsValidColor("#123456"));
    }

  }

}
