
namespace WordCounterCLITests
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  using FluentAssertions;

  using NUnit.Framework;
  using WordCounterCLI;

  [TestFixture]
  public class LineArgumentsTests
  {
    [Test]
    public void Parse_NullArray_FalseIsReturned()
    {
      LineArguments lineArguments = new LineArguments();
      bool result = lineArguments.Parse(null);
      result.Should().BeFalse();
    }

    [Test]
    public void Parse_EmptyArray_FalseIsReturned()
    {
      LineArguments lineArguments = new LineArguments();
      bool result = lineArguments.Parse(new string[0]);
      result.Should().BeFalse();
    }

    [Test]
    public void Parse_FirstArgumentIsOption_FalseIsReturned()
    {
      LineArguments lineArguments = new LineArguments();
      bool result = lineArguments.Parse(new string[] { "/compareto" });
      result.Should().BeFalse();
    }

    [Test]
    public void Parse_FirstArgumentIsNotOption_IsParsedCorrectly()
    {
      string fileName = "something.txt";
      LineArguments lineArguments = new LineArguments();
      bool result = lineArguments.Parse(new string[] { fileName });
      result.Should().BeTrue();
      lineArguments.SourceFile.Should().Be(fileName);
      lineArguments.ThereIsFileOutput.Should().BeFalse();
      lineArguments.DestinationPath.Should().BeNull();
      lineArguments.CompareTo.Should().BeFalse();
      lineArguments.SourceFileToCompare.Should().BeNull();
    }

    [Test]
    [TestCaseSource("EvenNumberArguments")]
    public void Parse_EvenNumberArguments_FalseIsReturned(string[] arguments)
    {
      LineArguments lineArguments = new LineArguments();
      bool result = lineArguments.Parse(arguments);
      result.Should().BeFalse();
    }

    private static object[] EvenNumberArguments =
      {
        new object[] { new string[] { "filename.txt", "/compareto" } },
        new object[] { new string[] { "filename.txt", "/compareto", "filename2.txt", "/destinationpath" } },
      };

    [Test]
    public void Parse_TwoConsecutiveOptions_FalseIsReturned()
    {
      LineArguments lineArguments = new LineArguments();
      bool result = lineArguments.Parse(new string[] { "filename.txt", "/compareto", "/destinationpath" });
      result.Should().BeFalse();
    }

    [Test]
    public void Parse_UnrecognizedOption_FalseIsReturned()
    {
      LineArguments lineArguments = new LineArguments();
      bool result = lineArguments.Parse(new string[] { "filename.txt", "/wrongoption", "somevalue" });
      result.Should().BeFalse();
    }

    [Test]
    public void Parse_CorrectOptions_AllDataFilledCorrectly()
    {
      string filename = "filename.txt";
      string compareTo = "otherfilename.txt";
      string destinationPath = "filename.json";
      LineArguments lineArguments = new LineArguments();
      bool result = lineArguments.Parse(new string[] { filename, "/destinationpath", destinationPath, "/compareto", compareTo });
      result.Should().BeTrue();
      lineArguments.SourceFile.Should().Be(filename);
      lineArguments.ThereIsFileOutput.Should().BeTrue();
      lineArguments.DestinationPath.Should().Be(destinationPath);
      lineArguments.CompareTo.Should().BeTrue();
      lineArguments.SourceFileToCompare.Should().Be(compareTo);
    }
  }
}
