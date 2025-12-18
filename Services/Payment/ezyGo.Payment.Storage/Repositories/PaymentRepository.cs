using ezyGo.EntityFrameworkCore.Repository;
using ezyGo.Payment.Storage.Entities;
using ezyGo.Payment.Storage.Repositories.Interfaces;
using ezyGo.Payment.Storage.Sql;
using Microsoft.EntityFrameworkCore;

namespace ezyGo.Payment.Storage.Repositories;

public class PaymentRepository : GenericRepository<PaymentInfo>, IPaymentRepository
{
    private readonly PaymentDbContext _db;
    public PaymentRepository(PaymentDbContext db) : base(db)
    {
        _db = db;
    }

    public Task<PaymentInfo> GetPaymentInfoByTransactionId(string tran_id)
    {
        return _db.PaymentInfos
            .FirstAsync(p => p.TransactionId == tran_id);
    }

    public async Task<string?> GetSeatsAsync(string tranId)
    {
        return await _db.PaymentInfos
            .Where(p => p.TransactionId == tranId)
            .Select(p => p.SeatNumbers)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> SavePaymentStatus(PaymentInfo paymentInfo)
    {
        var isExist = await _db.PaymentInfos.FirstOrDefaultAsync(p=>p.TransactionId == paymentInfo.TransactionId);

        if(isExist == null)
        {
            // Save
            await _db.PaymentInfos.AddAsync(paymentInfo);
            await _db.SaveChangesAsync();
            return true;
        }
        else
        {
            // Update
            isExist.Status = paymentInfo.Status;
            isExist.Fare = paymentInfo.Fare;
            _db.PaymentInfos.Update(isExist);
            await _db.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task UpdatePaymentStatus(string tran_id, string status)
    {
        var currentStatus = await _db.PaymentInfos.FirstOrDefaultAsync(p => p.TransactionId == tran_id);

        if (currentStatus == null)
        {
            throw new Exception("Transaction not found");

        }
        else
        {
            currentStatus.Status = status;
            _db.PaymentInfos.Update(currentStatus);
            await _db.SaveChangesAsync();
        }
    }
}
