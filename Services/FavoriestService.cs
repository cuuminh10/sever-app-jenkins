using AutoMapper;
using gmc_api.Base;
using gmc_api.Base.InterFace;
using gmc_api.DTO.CommonData;
using gmc_api.Entities;
using gmc_api.Repositories;

namespace gmc_api.Services
{
    public interface IFavoriestService : IServiceGMCBase<FavoriestReponse, FavoriestCreateRequest, Favorites, Favorites>
    {
        int checkExistModule(string aDUserShortCutModule, int fK_ADUserID);
    }

    public class FavoriestService : ServiceBaseImpl<FavoriestReponse, FavoriestCreateRequest, Favorites, Favorites>, IFavoriestService
    {
        private readonly IFavoriestReponsitory _repository;
        private readonly IMapper _mapper;

        public FavoriestService(IFavoriestReponsitory repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public int checkExistModule(string aDUserShortCutModule, int fK_ADUserID)
        {
            var data = _repository.checkExistModule(aDUserShortCutModule, fK_ADUserID);
            if (data != null)
            {
                return data.counts;
            }
            return 0;
        }
    }
}
