using Pars.Core.Monitoring.OpenTelemetry.Metrics;
using Pars.Core.Monitoring.OpenTelemetry.Metrics.Options;
namespace GB.SQLChatBot.Business;

public static class MetricsRegistry
{
    public static CounterOptions GetItemCounter => new()
    {
        Name = "get_item",
        MeasurementUnit = Unit.Calls
    };

    public static CounterOptions ListOfItemCounter => new()
    {
        Name = "list_of_item",
        MeasurementUnit = Unit.Calls
    };
    public static CounterOptions StartUpCounter => new ()
    {
        Name = "Startup_Counter",
        MeasurementUnit = Unit.Calls
    };
}
