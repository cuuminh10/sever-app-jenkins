using gmc_api.Base;
using gmc_api.Base.InterFace;
using gmc_api.Entities;

namespace gmc_api.Repositories
{
    public interface IADDocHistoryReponsitory : IRepositoriesBase<ADDocHistorys, ADDocHistorys>
    {
    }

    public class ADDocHistoryReponsitory : RepositoriesBaseImpl<ADDocHistorys, ADDocHistorys>, IADDocHistoryReponsitory
    {
        private readonly GMCContext _context;
        public ADDocHistoryReponsitory(GMCContext context) : base(context, "ADDocHistorys", "ADDocHistoryID")
        {
            _context = context;
        }
    }
}
