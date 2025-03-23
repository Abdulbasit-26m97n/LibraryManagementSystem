using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public record BookDto(int Id, string Title, string Author, string ISBN, int PublicationYear, bool IsAvailable);
    
    
}
