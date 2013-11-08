
namespace WordCounterCore
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;

  public class WordCounter
  {
    private ISplitStrategy strategy;
    private IDictionary<string, int> results;

    public WordCounter(ISplitStrategy strategy)
    {
      this.strategy = strategy;
    }

    public IDictionary<string, int> CountWordsOnText(string text)
    {
      this.results = new Dictionary<string, int>();

      this.CountWordsOnSingleLine(text);

      return results;
    }

    public IDictionary<string, int> CountWordsOnStreamText(StreamReader reader)
    {
      this.results = new Dictionary<string, int>();
      String line;
      while ((line = reader.ReadLine()) != null)
      {
        this.CountWordsOnSingleLine(line);
      }

      return results;
    }

    private void CountWordsOnSingleLine(string text)
    {
      if (String.IsNullOrWhiteSpace(text))
      {
        return;
      }

      string[] operands = this.strategy.SplitText(text);

      operands.ToList().ForEach(this.AddToDictionary);
    }

    private void AddToDictionary(string operand)
    {
      if (String.IsNullOrWhiteSpace(operand))
      {
        return;
      }

      if (results.ContainsKey(operand))
      {
        this.results[operand]++;
      }
      else
      {
        this.results.Add(operand, 1);
      }
    }
  }
}
