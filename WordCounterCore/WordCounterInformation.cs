
namespace WordCounterCore
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;

  using Extensions;

  using Newtonsoft.Json;

  [JsonObject(MemberSerialization.OptIn)]
  public class WordCounterInformation : IComparable, IComparable<WordCounterInformation>, IEnumerable
  {
    #region Member Variables
    [JsonProperty]
    private string fileName;

    [JsonProperty]
    private SortedDictionary<string, int> countedWords;
    #endregion

    #region Constructor
    public WordCounterInformation()
    {
      countedWords = new SortedDictionary<string, int>();
    }
    #endregion

    #region properties
    public string FileName
    {
      get
      {
        return this.fileName;
      }
      set
      {
        this.fileName = value;
      }
    }

    public int TotalWords
    {
      get
      {
        return this.countedWords.IsNullOrEmpty()
          ? 0
          : this.countedWords.Values.ToList().Aggregate((total, value) => total += value);
      }
    }
    #endregion

    #region Methods
    public static WordCounterInformation DeSerializeFromReader(StreamReader reader)
    {
      StringBuilder serializedObject = new StringBuilder();
      String line;
      while ((line = reader.ReadLine()) != null)
      {
        serializedObject.Append(line);
      }

      return WordCounterInformation.DeSerializeFromString(serializedObject.ToString());
    }

    public static WordCounterInformation DeSerializeFromString(string serializedObject)
    {
      return JsonConvert.DeserializeObject<WordCounterInformation>(serializedObject);
    }

    /// <summary>
    /// Converts the object into Json format
    /// </summary>
    /// <returns>The text that represents the object in Json format.</returns>
    public string SerializeToString()
    {
      return JsonConvert.SerializeObject(this);
    }

    /// <summary>
    /// Serializes the object into a file in Json format.
    /// </summary>
    /// <param name="filePathAndName">The full path to the file that where we want to serialize the object.</param>
    public void SerializeToFile(string filePathAndName)
    {
      using (TextWriter writer = new StreamWriter(filePathAndName))
      {
        writer.WriteLine(this.SerializeToString());
      }
    }

    /// <summary>
    /// Adds a word to the collection
    /// </summary>
    /// <param name="wordToAdd">The word that we want to add.</param>
    public void Add(string wordToAdd)
    {
      if (String.IsNullOrWhiteSpace(wordToAdd))
      {
        return;
      }

      if (this.countedWords.ContainsKey(wordToAdd))
      {
        this.countedWords[wordToAdd]++;
      }
      else
      {
        this.countedWords.Add(wordToAdd, 1);
      }
    }

    public int CompareTo(object obj)
    {
      if (obj is WordCounterInformation)
      {
        return this.CompareTo(obj as WordCounterInformation);
      }
      else
      {
        throw new InvalidCastException("The object passed is not of type " + this.GetType().ToString());
      }
    }

    public int CompareTo(WordCounterInformation other)
    {
      if (this == other)
      {
        return 0;
      }

      if (this.countedWords == null && other.countedWords == null)
      {
        return 0;
      }

      if (this.countedWords == null)
      {
        return -1;
      }

      if (other.countedWords == null)
      {
        return 1;
      }

      if (this.countedWords.Count != other.countedWords.Count)
      {
        return this.countedWords.Count.CompareTo(other.countedWords.Count);
      }

      for (int currentWordIndex = 0; currentWordIndex < this.countedWords.Keys.Count; currentWordIndex++)
      {
        if (!this.countedWords.Keys.ToList()[currentWordIndex].Equals(other.countedWords.Keys.ToList()[currentWordIndex]))
        {
          return this.countedWords.Keys.ToList()[currentWordIndex].CompareTo(other.countedWords.Keys.ToList()[currentWordIndex]);
        }
      }

      for (int currentWordIndex = 0; currentWordIndex < this.countedWords.Keys.Count; currentWordIndex++)
      {
        string firstKey = this.countedWords.Keys.ToList()[currentWordIndex];
        string secondKey = other.countedWords.Keys.ToList()[currentWordIndex];

        int comparisonResult = this.countedWords[firstKey].CompareTo(other.countedWords[secondKey]);

        if (comparisonResult != 0)
        {
          return comparisonResult;
        }
      }

      return 0;
    }

    public int this[string index]
    {
      get
      {
        return this.countedWords[index];
      }
    }

    public int this[int index]
    {
      get
      {
        string key = this.countedWords.Keys.ToList()[index];
        return this.countedWords[key];
      }
    }

    public IEnumerator GetEnumerator()
    {
      return this.countedWords.GetEnumerator();
    }

    public bool ContainsWord(String wordToCheck)
    {
      if (this.countedWords == null || this.countedWords.Count == 0)
      {
        return false;
      }

      return this.countedWords.Keys.Contains(wordToCheck);
    }

    public override string ToString()
    {
      StringBuilder builder = new StringBuilder();
      builder.AppendLine(string.Format("Filename: {0}", this.FileName));
      builder.AppendLine(string.Format("Unique Words: {0}", this.Count));
      builder.AppendLine(string.Format("Total Words: {0}", this.TotalWords));
      foreach (var countedWord in this.countedWords)
      {
        builder.AppendLine(string.Format("{0} : {1}", countedWord.Key, countedWord.Value));
      }

      return builder.ToString();
    }

    public int Count
    {
      get
      {
        return this.countedWords.Count;
      }
    }
    #endregion
  }
}
