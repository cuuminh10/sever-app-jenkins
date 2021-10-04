using gmc_api.Base;
using gmc_api.Base.dto;
using gmc_api.Base.InterFace;
using gmc_api.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace gmc_api.Repositories
{
    public interface IFavoriestReponsitory : IRepositoriesBase<Favorites, Favorites>
    {
        OnePropIntReturn checkExistModule(string aDUserShortCutModule, int fK_ADUserID);
    }

    public class FavoriestReponsitory : RepositoriesBaseImpl<Favorites, Favorites>, IFavoriestReponsitory
    {
        private readonly GMCContext _context;
        public FavoriestReponsitory(GMCContext context) : base(context, "ADUserShortCuts", "ADUserShortCutID")
        {
            _context = context;
        }

        public OnePropIntReturn checkExistModule(string aDUserShortCutModule, int fK_ADUserID)
        {
            string sql = string.Format(@"select count(ADUserShortCutID) counts from ADUserShortCuts where ADUserShortCutModule = N'{0}'
AND FK_ADUserID = {1} AND AAStatus = 'Alive'", aDUserShortCutModule, fK_ADUserID);
            return _context.OnePropIntReturn.FromSqlRaw(sql).FirstOrDefault();
        }
    }
}
