
namespace WordCounterCoreTests
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  using FluentAssertions;

  using NUnit.Framework;
  using WordCounterCore;

  [TestFixture]
  class WordCounterInformationTests
  {
    [Test]
    public void SomeTest()
    {
      WordCounterInformation wordCounterInformation = new WordCounterInformation();

      wordCounterInformation.FileName = null;
      String serialization1 = wordCounterInformation.SerializeToString();



      wordCounterInformation.FileName = String.Empty;
      String serialization2 = wordCounterInformation.SerializeToString();

      wordCounterInformation.Add("One");
      wordCounterInformation.FileName = "Something";
      String serialization3 = wordCounterInformation.SerializeToString();

      wordCounterInformation.Add("One");
      wordCounterInformation.Add("Two");
      wordCounterInformation.Add("Three");

      wordCounterInformation.FileName = "Another";
      String serialization4 = wordCounterInformation.SerializeToString();

      wordCounterInformation = WordCounterInformation.DeSerializeFromString(serialization1);
      wordCounterInformation = WordCounterInformation.DeSerializeFromString(serialization2);
      wordCounterInformation = WordCounterInformation.DeSerializeFromString(serialization3);
      wordCounterInformation = WordCounterInformation.DeSerializeFromString(serialization4);
    }

    [Test]
    public void CompareTo_SameObject_Returns0()
    {
      WordCounterInformation wordCounter1 = new WordCounterInformation();
      Int32 results = wordCounter1.CompareTo(wordCounter1);
      results.Should().Be(0);
    }

    [Test]
    public void CompareTo_BothObjectsDoNotHaveWords_Returns0()
    {
      WordCounterInformation wordCounter1 = new WordCounterInformation();
      WordCounterInformation wordCounter2 = new WordCounterInformation();
      Int32 results = wordCounter1.CompareTo(wordCounter2);
      results.Should().Be(0);
    }

    [Test]
    public void CompareTo_SecondObjectHasNoWords_ReturnsMoreThan0()
    {
      WordCounterInformation wordCounter1 = new WordCounterInformation();
      wordCounter1.Add("one");
      WordCounterInformation wordCounter2 = new WordCounterInformation();
      Int32 results = wordCounter1.CompareTo(wordCounter2);
      results.Should().BeGreaterThan(0);
    }

    [Test]
    public void CompareTo_FirstObjectHasNoWords_ReturnsLessThan0()
    {
      WordCounterInformation wordCounter1 = new WordCounterInformation();
      WordCounterInformation wordCounter2 = new WordCounterInformation();
      wordCounter2.Add( "one");
      Int32 results = wordCounter1.CompareTo(wordCounter2);
      results.Should().BeLessThan(0);
    }

    [Test]
    public void CompareTo_FirstObjectHasMoreDifferentWords_ReturnsMoreThan0()
    {
      WordCounterInformation wordCounter1 = new WordCounterInformation();
      wordCounter1.Add("one");
      wordCounter1.Add("two");
      WordCounterInformation wordCounter2 = new WordCounterInformation();
      wordCounter2.Add("one");
      Int32 results = wordCounter1.CompareTo(wordCounter2);
      results.Should().BeGreaterThan(0);
    }

    [Test]
    public void CompareTo_FirstObjectHasLessDifferentWords_ReturnsLessThan0()
    {
      WordCounterInformation wordCounter1 = new WordCounterInformation();
      wordCounter1.Add("one");
      WordCounterInformation wordCounter2 = new WordCounterInformation();
      wordCounter2.Add("one");
      wordCounter2.Add("two");
      Int32 results = wordCounter1.CompareTo(wordCounter2);
      results.Should().BeLessThan(0);
    }

    [Test]
    public void CompareTo_SameWordsAndCountOnFirstIsHigher_ReturnsMoreThan0()
    {
      WordCounterInformation wordCounter1 = new WordCounterInformation();
      wordCounter1.Add("one");
      wordCounter1.Add("two");
      wordCounter1.Add("one");
      wordCounter1.Add("two");
      WordCounterInformation wordCounter2 = new WordCounterInformation();
      wordCounter2.Add("one");
      wordCounter2.Add("two");
      wordCounter2.Add("two");
      Int32 results = wordCounter1.CompareTo(wordCounter2);
      results.Should().BeGreaterThan(0);
    }

    [Test]
    public void CompareTo_SameWordsAndCountOnFirstIsLower_ReturnsLessThan0()
    {
      WordCounterInformation wordCounter1 = new WordCounterInformation();
      wordCounter1.Add("two");
      wordCounter1.Add("one");
      wordCounter1.Add("two");
      WordCounterInformation wordCounter2 = new WordCounterInformation();
      wordCounter2.Add("one");
      wordCounter2.Add("one");
      wordCounter2.Add("two");
      wordCounter2.Add("two");
      Int32 results = wordCounter1.CompareTo(wordCounter2);
      results.Should().BeLessThan(0);
    }

    [Test]
    public void CompareTo_SameWordsAndCountIsEqual_Returns0()
    {
      WordCounterInformation wordCounter1 = new WordCounterInformation();
      wordCounter1.Add("two");
      wordCounter1.Add("one");
      wordCounter1.Add("two");
      WordCounterInformation wordCounter2 = new WordCounterInformation();
      wordCounter2.Add("one");
      wordCounter2.Add("two");
      wordCounter2.Add("two");
      Int32 results = wordCounter1.CompareTo(wordCounter2);
      results.Should().Be(0);
    }

    [Test]
    public void CompareTo_SameCountDifferentWordsAndAlphabeticallyFirstIsHigher_ReturnsMoreThan0()
    {
      WordCounterInformation wordCounter1 = new WordCounterInformation();
      wordCounter1.Add("one");
      wordCounter1.Add("under");
      wordCounter1.Add("one");
      wordCounter1.Add("under");
      WordCounterInformation wordCounter2 = new WordCounterInformation();
      wordCounter2.Add("one");
      wordCounter2.Add("two");
      wordCounter2.Add("two");
      Int32 results = wordCounter1.CompareTo(wordCounter2);
      results.Should().BeGreaterThan(0);
    }

    [Test]
    public void CompareTo_SameCountDifferentWordsAndAlphabeticallyFirstIsLower_ReturnsLessThan0()
    {
      WordCounterInformation wordCounter1 = new WordCounterInformation();
      wordCounter1.Add("two");
      wordCounter1.Add("one");
      wordCounter1.Add("two");
      WordCounterInformation wordCounter2 = new WordCounterInformation();
      wordCounter2.Add("one");
      wordCounter2.Add("under");
      wordCounter2.Add("one");
      wordCounter2.Add("under");
      Int32 results = wordCounter1.CompareTo(wordCounter2);
      results.Should().BeLessThan(0);
    }

    [Test]
    public void CompareTo_AnotherType_ThrowsException()
    {
      WordCounterInformation wordCounter1 = new WordCounterInformation();
      Action action = () => wordCounter1.CompareTo(String.Empty);
      action.ShouldThrow<InvalidCastException>();
    }
  }
}
