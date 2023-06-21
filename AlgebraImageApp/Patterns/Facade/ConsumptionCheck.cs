namespace AlgebraImageApp.Patterns.Facade;

public class ConsumptionCheck
{
    public bool inLimit(int consumption, string tier)
    {
        if (tier == "GOLD" && consumption < 100)
            return true;
        if (tier == "PRO" && consumption < 50)
            return true;
        if (tier == "FREE" && consumption < 10)
            return true;

        return false;
    }

}