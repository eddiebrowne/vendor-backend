using System.IO;
using System.Net.Mime;

namespace Domain
{
  public interface IPicture
  {
    string Path { get; set; }
    string Extension { get; set; }
  }
}