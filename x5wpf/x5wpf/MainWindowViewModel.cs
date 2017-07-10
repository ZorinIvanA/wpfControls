using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace x5wpf
{
    public class MainWindowViewModel
    {
        public DateTime NowDate { get { return DateTime.Now; } }

        public String TerminalNumber { get { return "№28"; } }
        public String PhoneNumber { get { return "8-800-200-95-55"; } } 
    }
}
