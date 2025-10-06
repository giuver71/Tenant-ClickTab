using EQP.EFRepository.Core.Repository;
using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Core.DAL.Context;

namespace ClickTab.Core.DAL.Repository.Generics
{
    public class MenuRepository : IdentityRepository<Menu>
    {
        public MenuRepository(DatabaseContext context) : base(context)
        {

        }
    }
}
