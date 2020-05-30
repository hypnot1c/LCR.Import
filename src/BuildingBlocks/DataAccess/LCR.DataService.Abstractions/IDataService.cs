using LCR.TPM.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LCR.DataService.Abstractions
{
  public interface IDataService
  {
    Task<IEnumerable<SwitchModel>> Switch_GetListAsync();
    Task RawData_MapDataAsync(decimal uploadHistoryId);
    Task MappedData_SetFlagsAsync(decimal uploadHistoryId);
    Task MappedData_SetRowFlagsAsync(decimal uploadHistoryId, decimal rawDataId);
    Task MappedData_DetectUnchangedRowsAsync(decimal uploadHistoryId, decimal previousHploadHistoryId);
    Task Import_SaveAsync(decimal uploadHistoryId);
    Task<IEnumerable<OperatorModel>> Operator_GetListAsync();
    Task<IEnumerable<UserQueryModel>> User_GetListAsync();
  }
}
