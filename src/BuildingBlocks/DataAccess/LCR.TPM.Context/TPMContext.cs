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
    }

    public DbSet<ImportHistoryModel> ImportHistory { get; set; }
    public DbSet<ImportRawDataModel> ImportRawData { get; set; }
  }
}
