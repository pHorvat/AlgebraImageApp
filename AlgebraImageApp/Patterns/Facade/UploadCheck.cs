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
            CustomLogger.Instance.Log("Consumption check: "+upload.ToString());

            
        }else if (!_descriptionCheck.isDescriptionSafe(description))
        {
            upload = false;
            CustomLogger.Instance.Log("description check: "+upload.ToString());

            
        }else if (!_typeCheck.isRegistered(user.Type))
        { 
            upload = false;
            CustomLogger.Instance.Log("Type check: "+upload.ToString());

           
        }else if (!_hashtagsCheck.areHashtagsSafe(hashtags))
        {
            upload = false;
            CustomLogger.Instance.Log("hashtags check: "+upload.ToString());

            
        }
        CustomLogger.Instance.Log("Upload check: "+upload.ToString());


        return upload;
    }
    
}