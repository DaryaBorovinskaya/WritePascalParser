using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WritePascalParser.Models
{
    public class RegularTextClass
    {
        private string _formule = "R = a|a(a|b)^nc(a(R1|R2|R3)),\r\nгде\r\na - буквы латинского алфавита и кириллица, цифры [0;9],\r\nb - символ \".\",\r\nс - символ \"@\",\r\nn [0;63],\r\nR1 = (a^sb)^kabd, s >= 0, k >= 0, s+k=m, m [5;255],\r\nR2 = (a^sb)^kab(e|f), s >= 0, k >= 0, s+k=m, m [6;255],\r\nd - ru\r\ne - com\r\nf - net";
        private string _language = "L(R) = {a|a(a|b)^nc(a(R1|R2|R3))},\r\nгде\r\na - буквы латинского алфавита и кириллица, цифры [0;9],\r\nb - символ \".\",\r\nс - символ \"@\",\r\nn [0;63],\r\nR1 = (a^sb)^kabd, s >= 0, k >= 0, s+k=m, m [5;255],\r\nR2 = (a^sb)^kab(e|f), s >= 0, k >= 0, s+k=m, m [6;255],\r\nd - ru\r\ne - com\r\nf - net";
        private string _example = "my.mail@corp.mail.com";

        public string Formule => _formule;
        public string Language => _language;
        public string Example => _example;
    }
}
