using AlgebraImageApp.Models;

namespace AlgebraImageApp.Patterns.Facade;

public class UploadCheck
{
    private ConsumptionCheck _consumptionCheck = new ConsumptionCheck();
    private DescriptionCheck _descriptionCheck = new DescriptionCheck();
    private TypeCheck _typeCheck = new TypeCheck();
    private HashtagsCheck _hashtagsCheck = new HashtagsCheck();

    public bool CanUpload(User user, string description, string hashtags)
    {
        bool upload = true;
        if (!_consumptionCheck.inLimit(user.Consumption, user.Tier))
        {
            upload = false;
        }else if (!_descriptionCheck.isDescriptionSafe(description))
        {
            upload = false;
        }else if (!_typeCheck.isRegistered(user.Type))
        { 
            upload = false;
        }else if (!_hashtagsCheck.areHashtagsSafe(hashtags))
        {
            upload = false;
        }


        return upload;
    }
    
}