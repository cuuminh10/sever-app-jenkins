namespace gmc_api.Base.dto
{
    /*
     * Object contains information of login user
     * UserLogin info = (UserLogin)HttpContext.Items["User"];
     */
    public class UserLoginInfo
    {
        public UserLoginInfo(int userID, int groupId, string userName)
        {
            UserID = userID;
            UserName = userName;
            GroupID = groupId;
        }

        public int UserID { get; set; }
        public string UserName { get; set; }
        public int GroupID { get; set; }
    }
}
