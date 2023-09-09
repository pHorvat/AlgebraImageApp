using PostSharp.Aspects;

namespace AlgebraImageApp.Aspect;

[Serializable]

public class ExceptionHandlingAspect : OnExceptionAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            Console.WriteLine("Exception caught in aspect: " + args.Exception.Message); 
            base.OnException(args);
        }
    }