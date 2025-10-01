using SyncService.EfComponents.Repository;

namespace SyncService.Services.NeoReporting;

public class NeoReportingService : INeoReportingService
{
    private readonly INeoRepository _neoRepository;

    public NeoReportingService(INeoRepository neoRepository)
    {
        _neoRepository = neoRepository;
    }
}