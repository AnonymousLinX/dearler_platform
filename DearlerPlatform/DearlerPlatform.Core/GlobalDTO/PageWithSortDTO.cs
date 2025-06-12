namespace DearlerPlatform.Core.GlobalDTO;

public class PageWithSortDTO
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public string Sort { get; set; }
    public OrderType OrderType { get; set; } = OrderType.Asc;
}
public enum OrderType{
    Asc,
    Desc
}