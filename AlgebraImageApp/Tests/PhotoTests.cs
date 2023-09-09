using AlgebraImageApp.Models;
using AlgebraImageApp.Patterns.Facade;
using NUnit.Framework;

namespace AlgebraImageApp.Tests;

[TestFixture]
public class PhotoTests
{
    private UploadCheck _uploadManager;

    [SetUp]
    public void SetUp()
    {
        _uploadManager = new UploadCheck();
    }

    [Test]
    public void CanUpload_WithValidUserAndContent_ReturnsTrue()
    {
        User user = new User
        {
            Consumption = 33,
            Tier = "PRO",
            Type = "Registered"
        };

        string description = "This is a safe description.";
        string hashtags = "#safetags";
        
        bool canUpload = _uploadManager.CanUpload(user, description, hashtags);
        
        Assert.IsTrue(canUpload);
    }

    [Test]
    public void CanUpload_WithExceededConsumption_ReturnsFalse()
    {
        User user = new User
        {
            Consumption = 1000,
            Tier = "Basic",
            Type = "Registered"
        };

        string description = "This is a safe description.";
        string hashtags = "#safetags";
        
        bool canUpload = _uploadManager.CanUpload(user, description, hashtags);
        
        Assert.IsFalse(canUpload);
    }

    [Test]
    public void CanUpload_WithUnsafeDescription_ReturnsFalse()
    {
        User user = new User
        {
            Consumption = 100,
            Tier = "Premium",
            Type = "Registered"
        };

        string description = "This description contains unsafe content: explicit words.";
        string hashtags = "#safetags";
        
        bool canUpload = _uploadManager.CanUpload(user, description, hashtags);
        
        Assert.IsFalse(canUpload);
    }

    [Test]
    public void CanUpload_WithUnregisteredUserType_ReturnsFalse()
    {
        User user = new User
        {
            Consumption = 100,
            Tier = "Premium",
            Type = "Unregistered"
        };

        string description = "This is a safe description.";
        string hashtags = "#safetags";
        
        bool canUpload = _uploadManager.CanUpload(user, description, hashtags);
        
        Assert.IsFalse(canUpload);
    }

    [Test]
    public void CanUpload_WithUnsafeHashtags_ReturnsFalse()
    {
        User user = new User
        {
            Consumption = 100,
            Tier = "Premium",
            Type = "Registered"
        };

        string description = "This is a safe description.";
        string hashtags = "#unsafe #inappropriate";
        
        bool canUpload = _uploadManager.CanUpload(user, description, hashtags);
        
        Assert.IsFalse(canUpload);
    }
}