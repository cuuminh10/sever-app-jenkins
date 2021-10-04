namespace gmc_api.Base.dto
{
    public class SQLBuilder
    {
        public string sqlCondition { get; set; }
        public string orderByCondition { get; set; }
        public Paging paging { get; set; }
    }
}
