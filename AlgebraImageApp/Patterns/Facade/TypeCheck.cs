namespace AlgebraImageApp.Patterns.Facade;

public class TypeCheck
{
    public bool isRegistered(string type)
    {
        if (type == "Registered" || type == "Admin")
        {
            return true;
        }

        return false;
    }
}