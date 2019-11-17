using LCR.TPM.Model.Abstractions;

namespace LCR.TPM.Model
{
  public class UserQueryModel : BaseModel
  {
    public string Firstname { get; set; }
    public string Lastname { get; set; }
  }
}
