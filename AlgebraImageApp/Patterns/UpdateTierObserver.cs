using System.Data;
using AlgebraImageApp.Models;
using AlgebraImageApp.Models.Database;
using MsSqlSimpleClient.Converters;
using MsSqlSimpleClient.SqlClient.Direct;

namespace AlgebraImageApp.Patterns;

public interface IUserTierObserver
{
    void UpdateUserTier(string tier);
}

public class UserTierSubject
{
    private readonly List<IUserTierObserver> _observers = new List<IUserTierObserver>();

    public void Attach(IUserTierObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IUserTierObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify(string tier)
    {
        foreach (var observer in _observers)
        {
            observer.UpdateUserTier(tier);
        }
    }
}


public class UserTierObserver : IUserTierObserver
{
    
    public async void UpdateUserTier(string tier)
    {
        // Update "last package change" for the user
        var LastPackageChange = DateTime.UtcNow;
        
        // Perform any additional operations or actions related to the update
        //await SomeOtherService.DoSomething(user);
        //DataSet consumption = (await _directClient.ExecuteQueryAsync("select * from photos where author_id="+id));
        //IEnumerable<DbUser> cons = consumption.ConvertTo<DbUser>();
    }
}
