using ExcelDataReader;
using LCR.TPM.Model;
using System;
using System.Globalization;

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
      result.DateOpen = reader.GetAsLocaleDateString(8);
      result.DateClose = reader.GetAsLocaleDateString(9);
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
      result.FileDirection = rawData.Direction[0] == 'I' ? '1' : '2';
      result.FileDateOpen = DateTime.Parse(rawData.DateOpen);

      if (!String.IsNullOrEmpty(rawData.DateClose))
      {
        result.FileDateClose = DateTime.Parse(rawData.DateClose);
      }

      rawData.ImportMappedData = result;
      return result;
    }

    public static string GetAsString(this IExcelDataReader reader, int index)
    {
      return reader.GetValue(index)?.ToString().Trim();
    }

    public static string GetAsLocaleDateString(this IExcelDataReader reader, int index)
    {
      var value = reader.GetValue(index);
      if (value != null)
      {
        switch (value)
        {
          case DateTime date:
            return date.ToString();
          default:
            var dateTemp = DateTime.Parse(value.ToString().Trim(), new CultureInfo("ru-RU"));
            return dateTemp.ToString();
        }
      }

      return value as string;
    }
  }
}
