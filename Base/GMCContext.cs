using gmc_api.Base.dto;
using gmc_api.Base.dto.Product;
using gmc_api.DTO;
using gmc_api.DTO.AR;
using gmc_api.DTO.CommonData;
using gmc_api.DTO.dto;
using gmc_api.DTO.FC;
using gmc_api.DTO.HR;
using gmc_api.DTO.Payment;
using gmc_api.DTO.PO;
using gmc_api.DTO.PP;
using gmc_api.DTO.PR;
using gmc_api.DTO.User;
using gmc_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace gmc_api.Base
{
    public class GMCContext : DbContext
    {
        public GMCContext(DbContextOptions options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleOfUser>().HasNoKey();
            modelBuilder.Entity<PPProductionOrdrGroupCount>().HasNoKey();
            modelBuilder.Entity<ProductBasicInfoList>().HasNoKey();
            modelBuilder.Entity<CommonInfo>().HasNoKey();
            modelBuilder.Entity<OnePropIntReturn>().HasNoKey();
            modelBuilder.Entity<OnePropStringReturn>().HasNoKey();
            modelBuilder.Entity<TwoPropReturn>().HasNoKey();
            modelBuilder.Entity<ApproveDataInfo>().HasNoKey();
            modelBuilder.Entity<EmployeeBasicInfo>().HasNoKey();

            modelBuilder.Entity<HREmployeeOffWorkReponse>().HasNoKey();
            modelBuilder.Entity<HREmployeeOvertimeReponse>().HasNoKey();
            modelBuilder.Entity<HRConfigInfos>().HasNoKey();
            modelBuilder.Entity<HRTravelCalendarReponse>().HasNoKey(); 

            modelBuilder.Entity<ThreePropIntReturn>().HasNoKey();
            modelBuilder.Entity<ADInboxItemPaging>().HasNoKey();
            modelBuilder.Entity<ADOutboxItemPaging>().HasNoKey();

            modelBuilder.Entity<JobTicketDetailBasic>().HasNoKey();
            modelBuilder.Entity<JobTicketItemsInfo>().HasNoKey();
            modelBuilder.Entity<JobTicketItemsKQSXInfo>().HasNoKey();
            modelBuilder.Entity<ProdRstDetailBasic>().HasNoKey();
            modelBuilder.Entity<ProdRstItemsInfo>().HasNoKey();
            modelBuilder.Entity<PPMSLayoutItemsInfo>().HasNoKey();
            modelBuilder.Entity<ICProductBasicInfo>().HasNoKey();

            modelBuilder.Entity<GeneralNoItemsInfo>().HasNoKey();
            modelBuilder.Entity<NumberingGenNo>().HasNoKey();

            modelBuilder.Entity<APPRInfo>().HasNoKey();
            modelBuilder.Entity<APPRItemInfo>().HasNoKey();
            modelBuilder.Entity<APPOInfo>().HasNoKey();
            modelBuilder.Entity<APPOItemInfo>().HasNoKey();
            modelBuilder.Entity<GLVoucherPaymentInfo>().HasNoKey();
            modelBuilder.Entity<GLVoucherPaymentItemInfo>().HasNoKey();
            modelBuilder.Entity<ARSOInfo>().HasNoKey();
            modelBuilder.Entity<ARSOItemInfo>().HasNoKey();

            modelBuilder.Entity<GLTranCfgBasicInfo>().HasNoKey();

            modelBuilder.Entity<AproveChart>().HasNoKey();
            modelBuilder.Entity<ApproveHistory>().HasNoKey();
            modelBuilder.Entity<CommentHistory>().HasNoKey();

            modelBuilder.Entity<PPProductionOrdrGroup>().HasNoKey();
            modelBuilder.Entity<ADDocumentReponse>().HasNoKey();
        }
        public DbSet<User> Users { get; set; }

        public DbSet<PPProductionOrdrs> PPProductionOrdrss { get; set; }
        public DbSet<PPProductionOrdrEstFGs> PPProductionOrdrEstFGs { get; set; }
        public DbSet<PPProductionOrdrEstRMs> PPProductionOrdrEstRMs { get; set; }
        public DbSet<Favorites> Favorites { get; set; }
        public DbSet<ADDocHistorys> ADDocHistorys { get; set; }
        public DbSet<HREmployeeOffWorks> HREmployeeOffWorks { get; set; }
        public DbSet<HREmployeeOvertimes> HREmployeeOvertimes { get; set; }
        public DbSet<HRTravelCalendars> HRTravelCalendars { get; set; }
        public DbSet<HRTravelCalendarItems> HRTravelCalendarItems { get; set; }
        public DbSet<ADInboxItems> ADInboxItems { get; set; }
        public DbSet<ADInboxItemPaging> ADInboxItemPaging { get; set; }
        public DbSet<ADOutboxItems> ADOutboxItems { get; set; }
        public DbSet<ADOutboxItemPaging> ADOutboxItemPaging { get; set; }
        public DbSet<ADAttachments> ADAttachments { get; set; }
        public DbSet<ADComments> ADComments { get; set; }

        public DbSet<RoleOfUser> RoleOfUser { get; set; }
        public DbSet<PPProductionOrdrGroupCount> PPProductionOrdrGroupCount { get; set; }
        public DbSet<PPProductionOrdrGroup> PPProductionOrdrGroup { get; set; }
        
        public DbSet<ProductBasicInfoList> ProductBasicInfoList { get; set; }
        public DbSet<CommonInfo> CommonInfo { get; set; }
        public DbSet<OnePropIntReturn> OnePropIntReturn { get; set; }
        public DbSet<OnePropStringReturn> OnePropStringReturn { get; set; }

        public DbSet<TwoPropReturn> TwoPropReturn { get; set; }
        public DbSet<ApproveDataInfo> ApproveDataInfo { get; set; }
        public DbSet<EmployeeBasicInfo> EmployeeBasicInfo { get; set; }
        public DbSet<HREmployeeOffWorkReponse> HREmployeeOffWorkReponse { get; set; }
        public DbSet<HRPeriods> HRPeriods { get; set; }
        public DbSet<HREmployeeLRegs> HREmployeeLRegs { get; set; }
        public DbSet<HRConfigInfos> HRConfigInfos { get; set; }
        
        public DbSet<HREmployeeOvertimeReponse> HREmployeeOvertimeReponse { get; set; }
        public DbSet<HRTravelCalendarReponse> HRTravelCalendarReponse { get; set; }

        public DbSet<ThreePropIntReturn> TwoPropIntReturn { get; set; }

        public DbSet<JobTicketDetailBasic> JobTicketDetailBasic { get; set; }
        public DbSet<JobTicketItemsInfo> JobTicketItemsInfo { get; set; }
        public DbSet<JobTicketItemsKQSXInfo> JobTicketItemsKQSXInfo { get; set; }

        public DbSet<ProdRstDetailBasic> ProdRstDetailBasic { get; set; }
        public DbSet<ProdRstItemsInfo> ProdRstItemsInfo { get; set; }
        public DbSet<PPMSLayoutItemsInfo> PPMSLayoutItemsInfo { get; set; }
        public DbSet<ICProductBasicInfo> ICProductBasicInfo { get; set; }

        public DbSet<GeneralNoItemsInfo> GeneralNoItemsInfo { get; set; }
        public DbSet<NumberingGenNo> NumberingGenNo { get; set; }

        public DbSet<ApproveHistory> ApproveHistory { get; set; }
        public DbSet<CommentHistory> CommentHistory { get; set; }
        public DbSet<ADDocumentReponse> ADDocumentReponse { get; set; }
        

        public DbSet<ICUOMFactors> ICUOMFactors { get; set; }
        public DbSet<ICProductUOMs> ICProductUOMs { get; set; }
        public DbSet<APPRInfo> APPRInfo { get; set; }
        public DbSet<APPRItemInfo> APPRItemInfo { get; set; }
        public DbSet<APPOInfo> APPOInfo { get; set; }
        public DbSet<APPOItemInfo> APPOItemInfo { get; set; }
        public DbSet<GLVoucherPaymentInfo> GLVoucherPaymentInfo { get; set; }
        public DbSet<GLVoucherPaymentItemInfo> GLVoucherPaymentItemInfo { get; set; }
        public DbSet<ARSOInfo> ARSOInfo { get; set; }
        public DbSet<ARSOItemInfo> ARSOItemInfo { get; set; }
        public DbSet<AproveChart> AproveChart { get; set; }
        public DbSet<GLTranCfgBasicInfo> GLTranCfgBasicInfo { get; set; }

    }
}
