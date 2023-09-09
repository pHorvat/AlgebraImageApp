using PostSharp.Aspects;

namespace AlgebraImageApp.Aspect;

[Serializable]
public class LoggingAspect : OnMethodBoundaryAspect
{
    public override void OnEntry(MethodExecutionArgs args)
    {
        Console.WriteLine($"Entering method {args.Method.Name}");
        Console.WriteLine("Parameters:");
        foreach (var argument in args.Arguments)
        {
            Console.WriteLine(argument);
        }
        Console.WriteLine("----------------------");
    }

    public override void OnExit(MethodExecutionArgs args)
    {
        Console.WriteLine($"Exiting method {args.Method.Name}");
        Console.WriteLine("----------------------");
    }
}

