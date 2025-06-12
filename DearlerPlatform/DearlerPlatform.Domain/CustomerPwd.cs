using System;
using System.Collections.Generic;

namespace DearlerPlatform.Domain;

public partial class CustomerPwd: BaseEntity
{
    public new int Id { get; set; }

    public string CustomerNo { get; set; } = null!;

    public string CustomerPwd1 { get; set; } = null!;
}
