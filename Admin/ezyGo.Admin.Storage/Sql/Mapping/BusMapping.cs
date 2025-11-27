using ezyGo.Admin.Storage.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ezyGo.Admin.Storage.Sql.Mapping;

public class BusMapping
{
    public static void Configure(EntityTypeBuilder<BusEntity> builder)
    {
        builder.ToTable("Buses");

        builder.HasOne(b => b.Company)
            .WithMany(c=>c.Buses)
            .HasForeignKey(b=>b.BusCompanyEntityId)
            .OnDelete(DeleteBehavior.SetNull);

    }
}
