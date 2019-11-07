using ExcelDataReader;
using LCR.TPM.Model;
using System;

namespace LCR.Import.Web.Api.Resources
{
  static class ExcelDataReaderExtensions
  {
    public static ImportRawDataModel ToImportRawDataModel(this IExcelDataReader reader)
    {
      var result = new ImportRawDataModel();

      result.DataRowId = Decimal.Parse(reader.GetAsString(0));
      result.ChannelBundleName = reader.GetAsString(1);
      result.SwitchOperatorName = reader.GetAsString(2);
      result.ATES = reader.GetAsString(3);
      result.ChannelBundleOperatorName = reader.GetAsString(4);
      result.Direction = reader.GetAsString(5);
      result.DirectionType = reader.GetAsString(6);
      result.AlarmType = reader.GetAsString(7);
      result.DateOpen = reader.GetAsString(8);
      result.DateClose = reader.GetAsString(9);
      result.PairedSwitchOperatorFullName = reader.GetAsString(10);
      result.PairedSwitchOperatorCoverage = reader.GetAsString(11);
      result.OperatorsNetworkConnectionLevel = reader.GetAsString(12);
      result.RTNetworkConnectionLevel = reader.GetAsString(13);
      result.BranchOffice = reader.GetAsString(14);

      return result;
    }

    public static ImportMappedDataModel ToMappedDataModel(this ImportRawDataModel rawData)
    {
      var result = new ImportMappedDataModel();

      result.UploadHistoryId = rawData.UploadHistoryId;
      result.ImportRawData = rawData;
      //result.ImportRawDataId = rawData.Id;
      result.FileDirection = rawData.Direction[0] == 'I' ? '1' : '2';
      result.FileDateOpen = DateTime.Parse(rawData.DateOpen);
      result.FileDateClose = String.IsNullOrEmpty(rawData.DateClose) ? null : (DateTime?)DateTime.Parse(rawData.DateClose);

      rawData.ImportMappedData = result;
      return result;
    }

    public static string GetAsString(this IExcelDataReader reader, int index)
    {
      return reader.GetValue(index)?.ToString().Trim();
    }
  }
}
