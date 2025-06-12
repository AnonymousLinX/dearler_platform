using DearlerPlatform.Domain;
using DearlerPlatform.Service.CustomerApp.DTO;

namespace DearlerPlatform.Service.CustomerApp;

public interface ICustomerService
{
    public Task<bool> CheckPassworld(CustomerLoginDTO dto);
    public Task<Customer> GetCustomerAsync(string customerNo);
}
