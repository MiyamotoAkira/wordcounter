

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
      WordCounterInformation results = wordCounter.CountWordsOnText("Go do that thing that you do so well");
      CheckBasicGoLine(results);
    }

    [Test]
    public void CountWords_ValidStringNoPunctuationComplexStrategy_CorrectCount()
    {
      WordCounter wordCounter = new WordCounter(new ComplexSplit());
      WordCounterInformation results = wordCounter.CountWordsOnText("Go do that thing that you do so well");
      CheckBasicGoLine(results);
    }

    [Test]
    public void CountWords_ValidStringWithPunctuationBasicStrategy_CountsWordWithPunctuationAsDifferent()
    {
      WordCounter wordCounter = new WordCounter(new BasicSplit());
      WordCounterInformation results = wordCounter.CountWordsOnText("Go do, that thing that you do so well");
      results.Count.Should().Be(8);
      results.ContainsWord("Go").Should().BeTrue();
      results.ContainsWord("do").Should().BeTrue();
      results.ContainsWord("do,").Should().BeTrue();
      results.ContainsWord("that").Should().BeTrue();
      results.ContainsWord("thing").Should().BeTrue();
      results.ContainsWord("you").Should().BeTrue();
      results.ContainsWord("so").Should().BeTrue();
      results.ContainsWord("well").Should().BeTrue();
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
      WordCounterInformation results = wordCounter.CountWordsOnText("Go do, that thing that. you do so well?");
      CheckBasicGoLine(results);
    }

    private void CheckBasicGoLine(WordCounterInformation results)
    {
      results.Count.Should().Be(7);
      results.ContainsWord("Go").Should().BeTrue();
      results.ContainsWord("do").Should().BeTrue();
      results.ContainsWord("that").Should().BeTrue();
      results.ContainsWord("thing").Should().BeTrue();
      results.ContainsWord("you").Should().BeTrue();
      results.ContainsWord("so").Should().BeTrue();
      results.ContainsWord("well").Should().BeTrue();
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
      WordCounterInformation results = wordCounter.CountWordsOnText(null);
      results.TotalWords.Should().Be(0);
    }

    [Test]
    public void CountWords_EmptyString_ShouldReturnEmptyDictionary()
    {
      ISplitStrategy strategy = Substitute.For<ISplitStrategy>();
      WordCounter wordCounter = new WordCounter(strategy);
      WordCounterInformation results = wordCounter.CountWordsOnText(String.Empty);
      results.TotalWords.Should().Be(0);
    }

    [Test]
    [Category("Slow")]
    public void CountWordsOnStreamText_NormalTextFile_ReturnsCorrectCounts()
    {
      WordCounter wordCounter = new WordCounter(new ComplexSplit());
      Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WordCounterCoreTests.InputFiles.TextMultipleLines.txt");
      WordCounterInformation results = wordCounter.CountWordsOnStreamText(new StreamReader(stream));

      results.Count.Should().Be(17);
      results.ContainsWord("Some").Should().BeTrue();
      results.ContainsWord("text").Should().BeTrue();
      results.ContainsWord("With").Should().BeTrue();
      results.ContainsWord("a").Should().BeTrue();
      results.ContainsWord("few").Should().BeTrue();
      results.ContainsWord("lines").Should().BeTrue();
      results.ContainsWord("or").Should().BeTrue();
      results.ContainsWord("more").Should().BeTrue();
      results.ContainsWord("And").Should().BeTrue();
      results.ContainsWord("punctuactions").Should().BeTrue();
      results.ContainsWord("signs").Should().BeTrue();
      results.ContainsWord("Yes").Should().BeTrue();
      results.ContainsWord("as").Should().BeTrue();
      results.ContainsWord("well").Should().BeTrue();
      results.ContainsWord("Just").Should().BeTrue();
      results.ContainsWord("to").Should().BeTrue();
      results.ContainsWord("test").Should().BeTrue();
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
