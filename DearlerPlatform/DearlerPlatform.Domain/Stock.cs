using System;
using System.Collections.Generic;

namespace DearlerPlatform.Domain;

public partial class Stock: BaseEntity
{
    public new int Id { get; set; }

    public string StockNo { get; set; } = null!;

    public string StockName { get; set; } = null!;

    public string StockLinkman { get; set; } = null!;

    public string StockTel { get; set; } = null!;

    public string StockPhone { get; set; } = null!;
}
