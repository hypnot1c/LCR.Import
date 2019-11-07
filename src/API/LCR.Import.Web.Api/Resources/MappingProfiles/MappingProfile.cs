using AutoMapper;
using LCR.Import.Web.Api.ViewModels;
using LCR.TPM.Model;

namespace LCR.Import.Web.Api.Resources
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      CreateMap<ImportRawDataModel, ImportResultViewModel>()
        .ForMember(d => d.OperatorsNetworkConnectionLvl, opt => opt.MapFrom(s => s.OperatorsNetworkConnectionLevel))
        .ForMember(d => d.FileDirection, opt => opt.MapFrom(s => s.ImportMappedData != null ? (char?)s.ImportMappedData.FileDirection : null))
        .ForMember(d => d.FileDateOpen, opt => opt.MapFrom(s => s.ImportMappedData != null ? s.ImportMappedData.FileDateOpen : null))
        .ForMember(d => d.FileDateClose, opt => opt.MapFrom(s => s.ImportMappedData != null ? s.ImportMappedData.FileDateClose : null))
        .ForMember(d => d.LCRDirection, opt => opt.MapFrom(s => s.ImportMappedData != null ? (char?)s.ImportMappedData.LCRDirection : null))
        .ForMember(d => d.LCRDateOpen, opt => opt.MapFrom(s => s.ImportMappedData != null ? s.ImportMappedData.LCRDateOpen : null))
        .ForMember(d => d.LCRDateClose, opt => opt.MapFrom(s => s.ImportMappedData != null ? s.ImportMappedData.LCRDateClose : null))
        ;
    }
  }
}
