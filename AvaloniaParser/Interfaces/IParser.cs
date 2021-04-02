using System.Collections.Generic;
using System.Threading.Tasks;
using AvaloniaParser.Models;

namespace AvaloniaParser.Interfaces
{
    public interface IParser
    {
        List<SongModel> Parse(string url);
    }
}