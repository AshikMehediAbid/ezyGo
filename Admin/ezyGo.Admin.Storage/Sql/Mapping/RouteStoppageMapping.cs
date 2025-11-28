using ezyGo.Admin.Storage.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ezyGo.Admin.Storage.Sql.Mapping;

public class RouteStoppageMapping
{
    public static void Configure(EntityTypeBuilder<RouteStoppageEntity> builder)
    {
        builder.ToTable("RouteStoppages");

        builder.HasOne(rs => rs.RouteEntity)
            .WithMany(r => r.Stoppages)
            .HasForeignKey(rs => rs.RouteEntityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rs => rs.BusStationEntity)
            .WithMany(r => r.RouteStoppages)
            .HasForeignKey(rs => rs.BusStationEntityId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
