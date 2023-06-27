using System.Data;
using AlgebraImageApp.Models;
using AlgebraImageApp.Models.Database;
using AlgebraImageApp.Repositories;
using AlgebraImageApp.Services;
using MsSqlSimpleClient.Converters;
using MsSqlSimpleClient.SqlClient.Direct;

namespace AlgebraImageApp.Patterns;

public interface IUserTierObserver
{
    void UpdateUserTier(int id);
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

    public void Notify(int id)
    {
        foreach (var observer in _observers)
        {
            observer.UpdateUserTier(id);
        }
    }
}


public class UserTierObserver : IUserTierObserver
{
    private IUserService _userService;

    public UserTierObserver(IUserService userService)
    {
        _userService = userService;
    }
    
    public async void UpdateUserTier(int  id)
    {
        
        await this._userService.UpdateLastPackageChangeAsync(id);

        
        
    }
}
