using SIS.MvcFramework.DependencyContainer;
using SIS.MvcFramework.Routing;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework
{
    public interface IMvcApplication
    {
        void Configure(IServerRoutingTable serverRoutingTable);

        void ConfigureServices(IServiceProvider serviceProvider);
    }
}
