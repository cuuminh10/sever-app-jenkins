using gmc_api.Base;
using gmc_api.Base.InterFace;
using gmc_api.Entities;

namespace gmc_api.Repositories
{
    public interface IPPProductionOrdrEstFGReponsitory : IRepositoriesBase<PPProductionOrdrEstFGs, PPProductionOrdrEstFGs>
    {
    }

    public class PPProductionOrdrEstFGReponsitory : RepositoriesBaseImpl<PPProductionOrdrEstFGs, PPProductionOrdrEstFGs>, IPPProductionOrdrEstFGReponsitory
    {
        private readonly GMCContext _context;
        public PPProductionOrdrEstFGReponsitory(GMCContext context) : base(context, "PPProductionOrdrEstFGs", "PPProductionOrdrEstFGID")
        {
            _context = context;
        }
    }
}
