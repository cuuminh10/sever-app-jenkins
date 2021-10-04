using gmc_api.Base;
using gmc_api.Base.InterFace;
using gmc_api.Entities;

namespace gmc_api.Repositories
{
    public interface IADInboxItemReponsitory : IRepositoriesBase<ADInboxItems, ADInboxItemPaging>
    {
    }

    public class ADInboxItemReponsitory : RepositoriesBaseImpl<ADInboxItems, ADInboxItemPaging>, IADInboxItemReponsitory
    {
        private readonly GMCContext _context;
        public ADInboxItemReponsitory(GMCContext context) : base(context, "ADInboxItems", "ADInboxItemID")
        {
            _context = context;
        }
    }
}
