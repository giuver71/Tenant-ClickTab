using EQP.EFRepository.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.DAL.Models.Tables
{
    public class Group : IBaseEntity, IAuditEntity<int>
    {
        public int ID { get ; set ; }
        public string Description { get; set; }
        public string Code { get; set; }

        public int FK_InsertUser { get ; set ; }
        public DateTime InsertDate { get ; set ; }
        public int FK_UpdateUser { get ; set ; }
        public DateTime? UpdateDate { get ; set ; }
    }
}
