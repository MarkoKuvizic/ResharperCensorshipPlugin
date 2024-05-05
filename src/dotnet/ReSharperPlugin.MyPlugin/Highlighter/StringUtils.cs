
using System.Linq;

namespace ReSharperPlugin.MyPlugin.Highlighter;

public class StringUtils
{
    public static string CleanUpBadWords(string comment)
    {
        var words = comment.Split(' ');
        string cleanComment = "";
        foreach (var word in words)
        {
            if (!BadWordRepository.Words.Keys.Any(k=> word.ToLower().Contains(k)))
            {
                cleanComment += word + " ";
            }
            else
            {
                cleanComment += BadWordRepository.Words[word.ToLower()] + " ";
            }

        }

        return cleanComment;
    }
}