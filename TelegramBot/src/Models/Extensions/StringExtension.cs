namespace DesktopApp.Models.Extensions;

public static class StringExtension
{
    public static string[] SplitBySemicolon(this string str)
    {
        return str.Split();
    }
}