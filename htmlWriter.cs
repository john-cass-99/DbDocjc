using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDocjc
{
    public class htmlWriter : StreamWriter
    {
        public htmlWriter(string filename)
            : base(filename)
        {

        }

        public override void Close()
        {
            Write("</body>\r\n</html>");
            Flush();
            base.Close();
        }
    }
}
