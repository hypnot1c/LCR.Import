using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DataBaseContextExtensions
{
  public static class EntityTypeConfigurationExtensions
  {
    private static readonly ConcurrentDictionary<Assembly, IEnumerable<Type>> typesPerAssembly = new ConcurrentDictionary<Assembly, IEnumerable<Type>>();
    private static readonly ConcurrentDictionary<Assembly, IEnumerable<Type>> queryTypesPerAssembly = new ConcurrentDictionary<Assembly, IEnumerable<Type>>();

    public static ModelBuilder UseEntityTypeConfiguration(this ModelBuilder modelBuilder, Assembly extenstionsAssembly)
    {
      if (!typesPerAssembly.TryGetValue(extenstionsAssembly, out IEnumerable<Type> configurationTypes))
      {
        typesPerAssembly[extenstionsAssembly] = configurationTypes = extenstionsAssembly
        .GetExportedTypes()
        .Where(x =>
          (x.GetTypeInfo().IsClass)
          &&
          (!x.GetTypeInfo().IsAbstract)
          &&
          (
            x.GetInterfaces()
              .Any(y =>
                  (y.GetTypeInfo().IsGenericType)
                  &&
                  (
                    y.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)
                  )
              )
           )
         );
      }

      var configurations = configurationTypes.Select(x => Activator.CreateInstance(x));

      foreach (dynamic configuration in configurations)
      {
        modelBuilder.ApplyConfiguration(configuration);
      }

      return modelBuilder;
    }

    public static ModelBuilder UseQueryTypeConfiguration(this ModelBuilder modelBuilder, Assembly extenstionsAssembly)
    {
      if (!queryTypesPerAssembly.TryGetValue(extenstionsAssembly, out IEnumerable<Type> configurationTypes))
      {
        queryTypesPerAssembly[extenstionsAssembly] = configurationTypes = extenstionsAssembly
        .GetExportedTypes()
        .Where(x =>
          (x.GetTypeInfo().IsClass)
          &&
          (!x.GetTypeInfo().IsAbstract)
          &&
          (
            x.GetInterfaces()
              .Any(y =>
                  (y.GetTypeInfo().IsGenericType)
                  &&
                  (
                    y.GetGenericTypeDefinition() == typeof(IQueryTypeConfiguration<>)
                  )
              )
           )
         );
      }

      var configurations = configurationTypes.Select(x => Activator.CreateInstance(x));

      foreach (dynamic configuration in configurations)
      {
        modelBuilder.ApplyConfiguration(configuration);
      }

      return modelBuilder;
    }
  }
}
