
namespace Extensions
{
  public static class ArrayExtensions
  {
    public static bool IsNullOrEmpty(this string[] array)
    {
      return array == null || array.Length == 0;
    }
  }
}
