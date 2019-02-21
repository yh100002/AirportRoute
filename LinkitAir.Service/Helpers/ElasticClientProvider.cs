using LinkitAir.Service.Configuration;
using LinkitAir.Service.Model;
using Microsoft.Extensions.Options;
using Nest;

public class ElasticClientProvider
{
    public ElasticClientProvider(IOptions<ElasticConnectionSettings> settings)
    {        
        // Create the connection settings
        ConnectionSettings connectionSettings = new ConnectionSettings(new System.Uri(settings.Value.ClusterUrl));
        // This is going to enable us to see the raw queries sent to elastic when debugging (really useful)
        connectionSettings.EnableDebugMode();

        if (!string.IsNullOrEmpty(settings.Value.AirportIndex))
        {   
            connectionSettings.DefaultMappingFor<Airport>(m => m.IndexName(settings.Value.AirportIndex));
        }

        if (!string.IsNullOrEmpty(settings.Value.AirlineIndex))
        {         
            connectionSettings.DefaultMappingFor<Airline>(m => m.IndexName(settings.Value.AirlineIndex));
        }

        if (!string.IsNullOrEmpty(settings.Value.RouteIndex))
        {         
            connectionSettings.DefaultMappingFor<Route>(m => m.IndexName(settings.Value.RouteIndex));
        }

        if (!string.IsNullOrEmpty(settings.Value.RouteDetailedIndex))
        {  
            connectionSettings.DefaultIndex(settings.Value.RouteDetailedIndex);  
        }        
        this.Client = new ElasticClient(connectionSettings);       
    }
    public ElasticClient Client { get; }
}