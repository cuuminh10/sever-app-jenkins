using gmc_api.Base;
using gmc_api.Base.InterFace;
using gmc_api.DTO.PP;
using gmc_api.DTO.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gmc_api.Repositories
{
    public interface IADUsersRepository : IRepositoriesBase<User, User>
    {
        DTO.PP.User GetUserByNameAndPass(string UserName, string Password);
        List<RoleOfUser> roleInSytems(int userId);
    }

    public class UserRepository : RepositoriesBaseImpl<User, User>, IADUsersRepository
    {
        private readonly GMCContext _context;
        public UserRepository(GMCContext context) : base(context, "ADUsers", "ADUserID")
        {
            _context = context;
        }

        public User GetUserByNameAndPass(string UserName, string Password)
        {
            GMCSecurity.Cryptography.Crypto cry = new GMCSecurity.Cryptography.Crypto();
            return _context.Users.AsQueryable().Where(s => s.ADUserName == UserName && s.ADPassword == cry.Encrypt(Password)).FirstOrDefault();
        }

        public List<RoleOfUser> roleInSytems(int userId)
        {
            var sqlBuilding = String.Format(@"select st.STModuleName as moduleName, st.STModuleID as moduleId from  ADUsers u 
inner join ADUserGroups ug on u.ADUserGroupID = ug.ADUserGroupID and ug.AAStatus = 'Alive'
inner join ADUserGroupSections ugs on ug.ADUserGroupID = ugs.ADUserGroupID 
inner join STModuleToUserGroupSections stugs on stugs.STUserGroupSectionID = ugs.ADUserGroupSectionID 
inner join STModules st on stugs.STModuleID = st.STModuleID 
where u.ADUserID = ${0}", userId);
            return _context.RoleOfUser.FromSqlRaw(sqlBuilding).ToList<RoleOfUser>();
        }
    }
}
