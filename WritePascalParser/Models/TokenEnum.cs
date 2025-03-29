using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WritePascalParser.Models
{
    public enum TokenEnum
    {
        None,
        Write,
        OpenBracket,
        Arguments,
        Comma,     // символ ,
        CloseBracket,
        EndLine    // символ ;
    }
}
