namespace AlgebraImageApp.Patterns.Facade;

public class HashtagsCheck
{
    public bool areHashtagsSafe(string hashtags)
    {
        List<string> badWords = new List<string> { "download", "prize", "viral", "blog", "website" };

        foreach (string word in badWords)
        {
            if (hashtags.Contains(word))
            {
                return false;
            }
        }

        return true;
    }
}