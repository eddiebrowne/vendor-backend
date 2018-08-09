namespace Infrastructure
{
  public class DatabaseSettings
  {
    public string ConnectionString { get; set; }
    public string VendorName { get; set; }
    public Scripts Scripts { get; set; }
    public bool Admin { get; set; }
  }

  public class Scripts
  {
    public string Main { get; set; }
    public string Vendor { get; set; }
  }
}