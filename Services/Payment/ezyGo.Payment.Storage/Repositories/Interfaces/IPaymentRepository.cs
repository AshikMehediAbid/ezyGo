using ezyGo.EntityFrameworkCore.Repository;
using ezyGo.Payment.Storage.Entities;

namespace ezyGo.Payment.Storage.Repositories.Interfaces;

public interface IPaymentRepository : IGenericRepository<PaymentInfo>
{
    Task<bool> SavePaymentStatus(PaymentInfo paymentInfo);
    Task UpdatePaymentStatus(string tran_id, string status);
    Task<string> GetSeatsAsync(string tran_id);
}
