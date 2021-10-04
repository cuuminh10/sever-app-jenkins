using AutoMapper;
using gmc_api.Base;
using gmc_api.Base.InterFace;
using gmc_api.DTO.FC;
using gmc_api.Entities;
using gmc_api.Repositories;
using System.Collections.Generic;
using static gmc_api.Base.Helpers.Constants;

namespace gmc_api.Services
{
    public interface IADCommentService : IServiceGMCBase<ADCommentReponse, ADCommentCreateRequest, ADComments, ADComments>
    {
        ADCommentReponseCus ConvertComment(ADCommentReponse rs);
        List<ADDocumentReponse> GetConvData(string type, int id);
    }

    public class ADCommentService : ServiceBaseImpl<ADCommentReponse, ADCommentCreateRequest, ADComments, ADComments>, IADCommentService
    {
        private readonly IADCommentReponsitory _repository;
        private readonly IMapper _mapper;

        public ADCommentService(IADCommentReponsitory repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ADCommentReponseCus ConvertComment(ADCommentReponse rs)
        {
            var tmp = _mapper.Map<ADCommentReponseCus>(rs);
            tmp.avatarUrl = "";
            return tmp;
        }

        public List<ADDocumentReponse> GetConvData(string type, int id)
        {
            return _repository.GetConvData(FileUploadType.UploadTable[type], id);
        }
    }
}
