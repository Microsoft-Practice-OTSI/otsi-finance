using System.Threading.Tasks;
using Finance.Services.Models;

namespace Finance.Services;

public interface IAuthService
{
    Task<TokenModel?> AuthenticateAsync(LoginModel model);
}
