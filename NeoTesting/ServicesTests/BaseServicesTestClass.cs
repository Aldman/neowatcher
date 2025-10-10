using NeoTesting.Auxiliary;

namespace NeoTesting.ServicesTests;

[Collection(WellKnownConstants.ServiceTemplate)]
public abstract class BaseServicesTestClass
{
    protected readonly ServiceProviderTemplate ServicesTemplate;

    protected BaseServicesTestClass(ServiceProviderTemplate servicesTemplate)
    {
        ServicesTemplate = servicesTemplate;
    }
}