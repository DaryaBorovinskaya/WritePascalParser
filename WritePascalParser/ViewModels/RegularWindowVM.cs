using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WritePascalParser.Models;

namespace WritePascalParser.ViewModels
{
    public class RegularWindowVM : BasicVM
    {

        public string RegularText { get; set; }


        public RegularWindowVM(int menuNumb)
        {
            RegularTextClass reg = new();

            switch (menuNumb)
            {
                case 1:
                    RegularText = reg.Formule;
                    break;
                case 2:
                    RegularText = reg.Language;
                    break;
                case 3:
                    RegularText = reg.Example;
                    break;
            }
        }

    }
}
