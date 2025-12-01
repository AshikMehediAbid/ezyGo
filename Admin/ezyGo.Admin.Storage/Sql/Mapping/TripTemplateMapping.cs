using ezyGo.Admin.Storage.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ezyGo.Admin.Storage.Sql.Mapping;

public class TripTemplateMapping
{
    public static void Configure(EntityTypeBuilder<TripTemplate> builder)
    {
        builder.ToTable("TripTemplates");

        // Relationship: Bus
        builder.HasOne(t => t.BusEntity)
               .WithMany()
               .HasForeignKey(t => t.BusId)
               .OnDelete(DeleteBehavior.SetNull);

        // Relationship: Route
        builder.HasOne(t => t.RouteEntity)
               .WithMany()
               .HasForeignKey(t => t.RouteId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
