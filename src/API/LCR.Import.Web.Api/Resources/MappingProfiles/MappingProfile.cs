using AutoMapper;
using LCR.Import.Web.Api.ViewModels;
using LCR.TPM.Model;
using System.Linq;

namespace LCR.Import.Web.Api.Resources
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      CreateMap<UploadHistoryModel, ImportSummaryViewModel>()
        .ForMember(d => d.SwitchName, opt => opt.Ignore())
        .ForMember(d => d.RowsCount, opt => opt.MapFrom(s => s.ImportRawData.Count))
        .ForMember(d => d.DiffRowsCount, opt => opt.MapFrom(s => s.ImportMappedData.Count(md => (md.Flags & 2) == 0)))
        .ForMember(d => d.ErrorRowsCount, opt => opt.MapFrom(s => s.ImportRawData.Count(rd => rd.ImportFormatErrors != null)))
        ;
    }
  }
}
