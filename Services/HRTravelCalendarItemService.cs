using AutoMapper;
using gmc_api.DTO.HR;
using gmc_api.Entities;
using gmc_api.Repositories;
using gmc_api.Base.InterFace;
using gmc_api.Base;

namespace gmc_api.Services
{
    public interface IHRTravelCalendarItemService : IServiceGMCBase<HRTravelCalendarItemReponse, HRTravelCalendarItemCreateRequest, HRTravelCalendarItems, HRTravelCalendarItems>
    {
    }

    public class HRTravelCalendarItemService : ServiceBaseImpl<HRTravelCalendarItemReponse, HRTravelCalendarItemCreateRequest, HRTravelCalendarItems, HRTravelCalendarItems>, IHRTravelCalendarItemService
    {
        private readonly IHRTravelCalendarItemReponsitory _repository;
        private readonly IMapper _mapper;

        public HRTravelCalendarItemService(IHRTravelCalendarItemReponsitory repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
    }
}
