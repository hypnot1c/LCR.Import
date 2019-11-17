using DataBaseContextExtensions;
using LCR.TPM.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LCR.TPM.Context
{
  public class TPMContext : DbContext
  {
    public TPMContext(DbContextOptions<TPMContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.UseEntityTypeConfiguration(typeof(TPMContext).GetTypeInfo().Assembly);
      modelBuilder.UseQueryTypeConfiguration(this, typeof(TPMContext).GetTypeInfo().Assembly);
    }

    public DbSet<UploadHistoryModel> UploadHistory { get; set; }
    public DbSet<ImportRawDataModel> ImportRawData { get; set; }
    public DbSet<ImportFormatErrorsModel> ImportFormatErrors { get; set; }
    public DbSet<ImportMappedDataModel> ImportMappedData { get; set; }
    public DbQuery<SwitchModel> Switches { get; set; }
    public DbQuery<OperatorModel> Operators { get; set; }
    public DbQuery<ImportResultQueryModel> ImportResults { get; set; }
  }
}
