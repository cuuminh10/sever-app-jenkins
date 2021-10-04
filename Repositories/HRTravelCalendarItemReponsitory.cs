using gmc_api.Base;
using gmc_api.Base.InterFace;
using gmc_api.Entities;

namespace gmc_api.Repositories
{
    public interface IHRTravelCalendarItemReponsitory : IRepositoriesBase<HRTravelCalendarItems, HRTravelCalendarItems>
    {
    }

    public class HRTravelCalendarItemReponsitory : RepositoriesBaseImpl<HRTravelCalendarItems, HRTravelCalendarItems>, IHRTravelCalendarItemReponsitory
    {
        private readonly GMCContext _context;
        public HRTravelCalendarItemReponsitory(GMCContext context) : base(context, "HRTravelCalendarItems", "HRTravelCalendarItemID")
        {
            _context = context;
        }
    }
}
