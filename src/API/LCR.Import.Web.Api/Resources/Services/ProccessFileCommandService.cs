using ExcelDataReader;
using LCR.TPM.Context;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LCR.Import.Web.Api.Resources
{
  public class ProccessFileCommandService : IProccessFileCommandService<ProccessFileCommand>
  {
    public ProccessFileCommandService(
      TPMContext tpmCtx
      )
    {
      this.TPMContext = tpmCtx;
    }

    public TPMContext TPMContext { get; }

    public async Task OnNextAsync(ProccessFileCommand command)
    {
      var userId = command.UserId;
      var filePath = command.FilePath;

      using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
      {
        using (var reader = ExcelReaderFactory.CreateReader(stream))
        {
          while (!reader.IsClosed && reader.Name != "Направления")
          {
            reader.NextResult();
          }
          do
          {
            while (reader.Read())
            {
              var firstValue = reader.GetValue(0);
              if (firstValue == null || !Int32.TryParse(firstValue.ToString(), out var number))
              {
                continue;
              }

              var rawData = reader.ToImportRawDataModel();
              rawData.UploadHistoryId = command.ImportHistoryId;

              this.TPMContext.ImportRawData.Add(rawData);
            }
          } while (reader.NextResult());
        }
      }

      var historyEntry = await this.TPMContext.ImportHistory.FindAsync(command.ImportHistoryId);
      historyEntry.Step = TPM.Model.ImportStep.Applied;

      await this.TPMContext.SaveChangesAsync();
    }
  }
}
