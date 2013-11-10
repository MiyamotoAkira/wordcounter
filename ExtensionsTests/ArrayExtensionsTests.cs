
namespace ExtensionsTests
{
  using System.Collections.Generic;
  using Extensions;
  using FluentAssertions;
  using NUnit.Framework;

  [TestFixture]
  public class ArrayExtensionsTests
  {
    [Test]
    public void IsNullOrEmpty_IsNull_ReturnsTrue()
    {
      string[] array = null;
      bool result = array.IsNullOrEmpty();
      result.Should().BeTrue();
    }

    [Test]
    public void IsNullOrEmpty_IsEmpty_ReturnsTrue()
    {
      string[] array = new string[0];
      bool result = array.IsNullOrEmpty();
      result.Should().BeTrue();
    }

    [Test]
    public void IsNullOrEmpty_HasDate_ReturnsFalse()
    {
      string[] array = new string[] { "something"} ;
      bool result = array.IsNullOrEmpty();
      result.Should().BeFalse();
    }
  }
}
