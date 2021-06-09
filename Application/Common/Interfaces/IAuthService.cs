using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<string> SignIn(string username, string password);
    }
}
