using ezyGo.Admin.Storage.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ezyGo.Admin.Storage.Sql.Mapping;

public class RouteMapping
{
    public static void Configure(EntityTypeBuilder<RouteEntity> builder)
    {
        builder.ToTable("Routes");

        builder.HasOne(r => r.StartingPoint)
            .WithMany()
            .HasForeignKey(r => r.StartingPointId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(r => r.EndingPoint)
            .WithMany()
            .HasForeignKey(r => r.EndingPointId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
