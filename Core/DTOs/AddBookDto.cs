using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public record AddBookDto(string Title, string Author, string ISBN, int PublicationYear, bool IsAvailable, DateTime AddedDate, DateTime LastIssuedDate, string InternalNote, int TotalCopies, int IssuedCopies);
}
