using ezyGo.Payment.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace ezyGo.Payment.Storage.Sql;

public class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
    {
    }

    public DbSet<PaymentInfo> PaymentInfos { get; set; }
}
