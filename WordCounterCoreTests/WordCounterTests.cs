

namespace WordCounterCoreTests
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Reflection;
  using System.Text;
  using FluentAssertions;
  using NSubstitute;
  using NUnit.Framework;
  using WordCounterCore;

  [TestFixture]
  class WordCounterTests
  {
    [Test]
    public void CountWords_ValidStringNoPunctuationBasicStrategy_CorrectCount()
    {
      WordCounter wordCounter = new WordCounter(new BasicSplit());
      IDictionary<string, int> results = wordCounter.CountWordsOnText("Go do that thing that you do so well");
      CheckBasicGoLine(results);
    }

    [Test]
    public void CountWords_ValidStringNoPunctuationComplexStrategy_CorrectCount()
    {
      WordCounter wordCounter = new WordCounter(new ComplexSplit());
      IDictionary<string, int> results = wordCounter.CountWordsOnText("Go do that thing that you do so well");
      CheckBasicGoLine(results);
    }

    [Test]
    public void CountWords_ValidStringWithPunctuationBasicStrategy_CountsWordWithPunctuationAsDifferent()
    {
      WordCounter wordCounter = new WordCounter(new BasicSplit());
      IDictionary<string, int> results = wordCounter.CountWordsOnText("Go do, that thing that you do so well");
      results.Count.Should().Be(8);
      results.Keys.Contains("Go").Should().BeTrue();
      results.Keys.Contains("do").Should().BeTrue();
      results.Keys.Contains("do,").Should().BeTrue();
      results.Keys.Contains("that").Should().BeTrue();
      results.Keys.Contains("thing").Should().BeTrue();
      results.Keys.Contains("you").Should().BeTrue();
      results.Keys.Contains("so").Should().BeTrue();
      results.Keys.Contains("well").Should().BeTrue();
      results["Go"].Should().Be(1);
      results["do"].Should().Be(1);
      results["do,"].Should().Be(1);
      results["that"].Should().Be(2);
      results["thing"].Should().Be(1);
      results["you"].Should().Be(1);
      results["so"].Should().Be(1);
      results["well"].Should().Be(1);
    }

    [Test]
    public void CountWords_ValidStringWithPunctuationComplexStrategy_IgnoresPunctuationCorrectNumberOfWordsReturned()
    {
      WordCounter wordCounter = new WordCounter(new ComplexSplit());
      IDictionary<string, int> results = wordCounter.CountWordsOnText("Go do, that thing that. you do so well?");
      CheckBasicGoLine(results);
    }

    private void CheckBasicGoLine(IDictionary<string, int> results)
    {
      results.Count.Should().Be(7);
      results.Keys.Contains("Go").Should().BeTrue();
      results.Keys.Contains("do").Should().BeTrue();
      results.Keys.Contains("that").Should().BeTrue();
      results.Keys.Contains("thing").Should().BeTrue();
      results.Keys.Contains("you").Should().BeTrue();
      results.Keys.Contains("so").Should().BeTrue();
      results.Keys.Contains("well").Should().BeTrue();
      results["Go"].Should().Be(1);
      results["do"].Should().Be(2);
      results["that"].Should().Be(2);
      results["thing"].Should().Be(1);
      results["you"].Should().Be(1);
      results["so"].Should().Be(1);
      results["well"].Should().Be(1);
    }

    [Test]
    public void CountWords_NullString_ShouldReturnEmptyDictionary()
    {
      ISplitStrategy strategy = Substitute.For<ISplitStrategy>();
      WordCounter wordCounter = new WordCounter(strategy);
      IDictionary<string, int> results = wordCounter.CountWordsOnText(null);
      results.Count.Should().Be(0);
    }

    [Test]
    public void CountWords_EmptyString_ShouldReturnEmptyDictionary()
    {
      ISplitStrategy strategy = Substitute.For<ISplitStrategy>();
      WordCounter wordCounter = new WordCounter(strategy);
      IDictionary<string, int> results = wordCounter.CountWordsOnText(String.Empty);
      results.Count.Should().Be(0);
    }

    [Test]
    [Category("Slow")]
    public void CountWordsOnStreamText_NormalTextFile_ReturnsCorrectCounts()
    {
      WordCounter wordCounter = new WordCounter(new ComplexSplit());
      Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WordCounterCoreTests.InputFiles.TextMultipleLines.txt");
      IDictionary<string, int> results = wordCounter.CountWordsOnStreamText(new StreamReader(stream));

      results.Count.Should().Be(17);
      results.Keys.Contains("Some").Should().BeTrue();
      results.Keys.Contains("text").Should().BeTrue();
      results.Keys.Contains("With").Should().BeTrue();
      results.Keys.Contains("a").Should().BeTrue();
      results.Keys.Contains("few").Should().BeTrue();
      results.Keys.Contains("lines").Should().BeTrue();
      results.Keys.Contains("or").Should().BeTrue();
      results.Keys.Contains("more").Should().BeTrue();
      results.Keys.Contains("And").Should().BeTrue();
      results.Keys.Contains("punctuactions").Should().BeTrue();
      results.Keys.Contains("signs").Should().BeTrue();
      results.Keys.Contains("Yes").Should().BeTrue();
      results.Keys.Contains("as").Should().BeTrue();
      results.Keys.Contains("well").Should().BeTrue();
      results.Keys.Contains("Just").Should().BeTrue();
      results.Keys.Contains("to").Should().BeTrue();
      results.Keys.Contains("test").Should().BeTrue();
      results["Some"].Should().Be(1);
      results["text"].Should().Be(1);
      results["With"].Should().Be(1);
      results["a"].Should().Be(2);
      results["few"].Should().Be(2);
      results["lines"].Should().Be(1);
      results["or"].Should().Be(1);
      results["more"].Should().Be(1);
      results["And"].Should().Be(1); 
      results["punctuactions"].Should().Be(1);
      results["signs"].Should().Be(1);
      results["Yes"].Should().Be(1);
      results["as"].Should().Be(1);
      results["well"].Should().Be(1);
      results["Just"].Should().Be(1);
      results["to"].Should().Be(1);
      results["test"].Should().Be(1);
    }
  }
}
