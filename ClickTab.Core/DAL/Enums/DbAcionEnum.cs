using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.DAL.Enums
{
    public enum DbActionEnum
    {
        ONLY_FIRST_START = 1,
        ONLY_INSERT = 2,
        INSERT_AND_UPDATE = 3
    }
}
