using Stylophonix.Interfaces;
using Stylophonix.Interfaces.Controllers;

namespace Stylophonix.Services.Controllers;

public class DataControllerService : IDataControllerService
{
    private readonly IDataService _dataService;

    public DataControllerService(IDataService dataService)
    {
        _dataService = dataService;
    }

    public bool RefreshData()
    {
        return _dataService.LoadData();
    }
}