namespace Phoneden.Services
{
  using Interfaces;

  public class PaginationConfiguration : IPaginationConfiguration
  {
    public int RecordsPerPage { get; set; }
  }
}
