using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.Generics;
using EQP.EFRepository.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.DAL.Repository.Generics
{
    public class UrlTokenRepository : IdentityRepository<UrlToken>
    {
        public UrlTokenRepository(DatabaseContext context) : base(context) { }
    }
}
