using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using AlgebraImageApp.Patterns;

namespace AlgebraImageApp.Tools;

public class Serialization
{
    private static readonly List<Type> WhitelistedTypes = new List<Type>
    {
        typeof(PhotoBuilder) 
    };

    public static byte[] SerializeObject(object obj)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, obj);
            return memoryStream.ToArray();
        }
    }

    public static T DeserializeObject<T>(byte[] data)
    {
        using (MemoryStream memoryStream = new MemoryStream(data))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            object obj = binaryFormatter.Deserialize(memoryStream);

            if (obj is T result && WhitelistedTypes.Contains(result.GetType()))
            {
                return result;
            }
            else
            {
                throw new InvalidCastException("Error: class "+obj.GetType()+" is not whitelisted and cannot be deserialized");
            }
        }
    }
}