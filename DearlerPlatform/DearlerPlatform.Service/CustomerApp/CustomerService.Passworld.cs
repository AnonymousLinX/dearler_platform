using DearlerPlatform.Service.CustomerApp.DTO;
using DearlerPlatform.Common.SHA256Module;

namespace DearlerPlatform.Service.CustomerApp;

public partial class CustomerService
{
    public async Task<bool> CheckPassworld(CustomerLoginDTO dto)
    {
        var res = await CustomerRepoPassworldRepo.GetAsync(m => m.CustomerNo == dto.CustomerNo && m.CustomerPwd1 == dto.Password.ToSHA256());
        return res != null;
    }
}
