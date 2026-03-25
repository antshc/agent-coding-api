using LightBDD.Framework.Notification;
using Serilog;

namespace ApiTests.IntegrationTests.LightBDD;

public class SerilogLightBDDProgressNotifier : DefaultProgressNotifier
{
    public SerilogLightBDDProgressNotifier()
        : base(message => Log.Logger.Information(message))
    {
    }
}
