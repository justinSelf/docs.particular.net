using log4net;

public class MyService
{
    static ILog logger = LogManager.GetLogger(typeof(MyService));

    public void WriteHello()
    {
        logger.Info("Hello from MyService.");
    }
}