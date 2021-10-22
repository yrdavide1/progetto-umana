using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyModel.BuisnessLogic
{
    public class StudentSearchInfo
    {
        public string? Fullname;

        public StudentSearchInfo() { }
        public StudentSearchInfo(string? fullname)
        {
            Fullname = fullname;
        }
    }
}
