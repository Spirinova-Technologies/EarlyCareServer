using EarlyCare.Infrastructure;

namespace EarlyCare.Core.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(int userId);

        bool IsTokenValid(string authToken, out UserToken decodedToken);
    }
}