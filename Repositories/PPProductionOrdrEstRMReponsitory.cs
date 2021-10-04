using gmc_api.Base;
using gmc_api.Base.InterFace;
using gmc_api.Entities;

namespace gmc_api.Repositories
{
    public interface IPPProductionOrdrEstRMReponsitory : IRepositoriesBase<PPProductionOrdrEstRMs, PPProductionOrdrEstRMs>
    {
    }

    public class PPProductionOrdrEstRMReponsitory : RepositoriesBaseImpl<PPProductionOrdrEstRMs, PPProductionOrdrEstRMs>, IPPProductionOrdrEstRMReponsitory
    {
        private readonly GMCContext _context;
        public PPProductionOrdrEstRMReponsitory(GMCContext context) : base(context, "PPProductionOrdrEstRMs", "PPProductionOrdrEstRMID")
        {
            _context = context;
        }
    }
}
