using AlgebraImageApp.Repositories;

namespace AlgebraImageApp.Patterns;

public class PhotoBuilder
{
    private int _authorId;
    private string _description;
    private string _format;
    private string _hashtags;
    private string _url;
    private string _authorUsername;

    public PhotoBuilder SetAuthorId(int authorId)
    {
        _authorId = authorId;
        return this;
    }

    public PhotoBuilder SetDescription(string description)
    {
        _description = description;
        return this;
    }

    public PhotoBuilder SetFormat(string format)
    {
        _format = format;
        return this;
    }

    public PhotoBuilder SetHashtags(string hashtags)
    {
        _hashtags = hashtags;
        return this;
    }

    public PhotoBuilder SetUrl(string url)
    {
        _url = url;
        return this;
    }

    public PhotoBuilder SetAuthorUsername(string authorUsername)
    {
        _authorUsername = authorUsername;
        return this;
    }

    public async Task<int> AddPhotoAsync(IPhotosRepository repository)
    {
        return await repository.AddPhotoAsync(_authorId, _description, _format, _hashtags, _url, _authorUsername);
    }
}
