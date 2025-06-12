using DearlerPlatform.Core.Repository;
using DearlerPlatform.Domain;

namespace DearlerPlatform.Service.CustomerApp;

public partial class CustomerService: ICustomerService
{
    public IRepository<Customer> CustomerRepo { get; set; }
    public IRepository<CustomerInvoice> CustomerInvoiceRepo { get; set; }
    public IRepository<CustomerPwd> CustomerRepoPassworldRepo { get; set; }

    public CustomerService(
        IRepository<Customer> customerRepo,
        IRepository<CustomerInvoice> customerInvoiceRepo,
        IRepository<CustomerPwd> customerRepoPassworldRepo
    )
    {
        CustomerRepo = customerRepo;
        CustomerInvoiceRepo = customerInvoiceRepo;
        CustomerRepoPassworldRepo = customerRepoPassworldRepo;
    }
    public async Task<Customer> GetCustomerAsync(string customerNo)
    {
        return await CustomerRepo.GetAsync(m => m.CustomerNo == customerNo);
    }
}
