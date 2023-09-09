namespace AlgebraImageApp.Tools;

[Serializable]
public class BadClass
{
    public string Field1 { get; set; }
    
    public string Field2 { get; set; }

    public BadClass()
    {
        Field1 = "one";
        Field2 = "two";
    }
}