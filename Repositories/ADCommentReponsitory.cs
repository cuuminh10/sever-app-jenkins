using gmc_api.Base;
using gmc_api.Base.InterFace;
using gmc_api.DTO.FC;
using gmc_api.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace gmc_api.Repositories
{
    public interface IADCommentReponsitory : IRepositoriesBase<ADComments, ADComments>
    {
        List<ADDocumentReponse> GetConvData(string table, int id);
    }

    public class ADCommentReponsitory : RepositoriesBaseImpl<ADComments, ADComments>, IADCommentReponsitory
    {
        private readonly GMCContext _context;
        public ADCommentReponsitory(GMCContext context) : base(context, "ADComments", "ADCommentID")
        {
            _context = context;
        }

        public List<ADDocumentReponse> GetConvData(string table, int id)
        {
            var sqlBuilding = string.Format(@"SELECT id, realName, saveName,comment, createUser, createDate, types
from (
        SELECT ADAttachmentID id ,ADAttachmentName realName ,ADAttachmentPath saveName , '' comment, AACreatedUser createUser , AACreatedDate createDate, 'attach' types
        from ADAttachments WHERE AAStatus = 'Alive' AND ADAttachmentRefID = {0} AND ADAttachmentTable = '{1}' 
        UNION ALL
        SELECT ADCommentID id, '' realName, '' saveName , ADCommentName comment , AACreatedUser createUser, AACreatedDate createDate, 'comment' types
        from ADComments  WHERE AAStatus = 'Alive' AND ADCommentRefID = {0} AND ADCommentTable = '{1}' ) tmp
order by tmp.createDate desc", id, table);
            return _context.ADDocumentReponse.FromSqlRaw(sqlBuilding).ToList<ADDocumentReponse>();
        }
    }
}
