using System.Reflection;
using Microsoft.EntityFrameworkCore;

using Zenit.Share.Data.Interfaces;

namespace Zenit.Share.Data.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void RegisterAllEntities(this ModelBuilder builder)
        {
            var assemblyTypes = Assembly.GetCallingAssembly().GetExportedTypes()
                .Where(t => t.IsPublic && !t.IsInterface && !t.IsAbstract);

            foreach (var t in assemblyTypes)
            {
                Console.WriteLine($"  {t.FullName}");
            }

            var entityTypes = assemblyTypes
                .Where(t => t.IsAssignableTo(typeof(IDataModel)) || t.IsAssignableTo(typeof(IDataModel<>)));

            foreach (var entityType in entityTypes)
            {
                builder.Entity(entityType);
            }
        }
    }
}