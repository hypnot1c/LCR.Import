using LCR.TPM.Model.Abstractions;

namespace LCR.TPM.Model
{
  public class ImportFormatErrorsModel : BaseModel
  {
    public decimal ImportRawDataId { get; set; }
    public long ErrorFlags { get; set; }
    public ImportRawDataModel ImportRawData { get; set; }
  }
}
