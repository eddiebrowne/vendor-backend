using System.IO;
using System.Net.Mime;
using Domain;

namespace Infrastructure
{
  public class Picture : IPicture
  {
    public string Path { get; set; }
    public string Extension { get; set; }
  }
}