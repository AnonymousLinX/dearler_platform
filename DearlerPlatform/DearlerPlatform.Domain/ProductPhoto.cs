using System;
using System.Collections.Generic;

namespace DearlerPlatform.Domain;

public partial class ProductPhoto: BaseEntity
{
    public new int Id { get; set; }

    public string? SysNo { get; set; }

    public string ProductNo { get; set; } = null!;

    public string ProductPhotoUrl { get; set; } = null!;
}
