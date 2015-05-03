using System;

public class FoundsFlow
{
    public FoundsFlow()
	{
	}

    public DateTime Date { get; set; }
    public string IdentityCode { get; set; }
    public string DealerCode { get; set; }
    public string Description { get; set; }
    public Decimal Quantity { get; set; }
    public Decimal Price { get; set; }
    public Decimal Fee { get; set; }
    public Decimal VAT { get; set; }
    public Decimal Total { get; set; }
}
