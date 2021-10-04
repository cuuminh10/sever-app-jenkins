using gmc_api.Base;
using gmc_api.Base.InterFace;
using gmc_api.Entities;

namespace gmc_api.Repositories
{
    public interface IADOutboxItemReponsitory : IRepositoriesBase<ADOutboxItems, ADOutboxItemPaging>
    {
    }

    public class ADOutboxItemReponsitory : RepositoriesBaseImpl<ADOutboxItems, ADOutboxItemPaging>, IADOutboxItemReponsitory
    {
        private readonly GMCContext _context;
        public ADOutboxItemReponsitory(GMCContext context) : base(context, "ADOutboxItems", "ADOutboxItemID")
        {
            _context = context;
        }
    }
}
