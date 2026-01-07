using System;
namespace ezyGo.QrCodeGenerator.Services;

public interface IQrCodeService
{
    byte[] GenerateQrCode(string text);
}
