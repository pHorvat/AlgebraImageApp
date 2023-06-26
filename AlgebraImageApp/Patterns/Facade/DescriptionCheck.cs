namespace AlgebraImageApp.Patterns.Facade;

public class DescriptionCheck
{
    public bool isDescriptionSafe(string description)
    {
        List<string> badWords = new List<string> { "fake", "spam", "rude" };

        foreach (string word in badWords)
        {
            if (description.Contains(word))
            {
                return false;
            }
        }

        return true;
    }
}