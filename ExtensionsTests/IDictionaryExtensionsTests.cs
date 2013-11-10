namespace ExtensionsTests
{
  using System.Collections.Generic;
  using Extensions;
  using FluentAssertions;
  using NUnit.Framework;

  [TestFixture]
  public class IDictionaryExtensionsTests
  {
    [Test]
    public void IsNullOrEmpty_IsNull_ReturnsTrue()
    {
      SortedDictionary<string, int> sortedDictionary = null;
      bool result = sortedDictionary.IsNullOrEmpty();
      result.Should().BeTrue();
    }

    [Test]
    public void IsNullOrEmpty_IsEmpty_ReturnsTrue()
    {
      SortedDictionary<string, int> sortedDictionary = new SortedDictionary<string, int>();
      bool result = sortedDictionary.IsNullOrEmpty();
      result.Should().BeTrue();
    }

    [Test]
    public void IsNullOrEmpty_HasDate_ReturnsFalse()
    {
      SortedDictionary<string, int> sortedDictionary = new SortedDictionary<string, int> { { "something", 1 } };
      bool result = sortedDictionary.IsNullOrEmpty();
      result.Should().BeFalse();
    }
  }
}
