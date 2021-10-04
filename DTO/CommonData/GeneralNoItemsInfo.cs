namespace gmc_api.DTO.CommonData
{
    public class GeneralNoItemsInfo
    {
        public string types { get; set; }
        public string cl_n_columnName { get; set; }
        public string cl_f_table { get; set; }
        public string cl_f_columnName { get; set; }
        public string st_date_format { get; set; }
        public string st_seper { get; set; }
        public string st_type { get; set; } //, --using for const
        public int orders { get; set; }
        public int st_auto_length { get; set; }
    }
}
