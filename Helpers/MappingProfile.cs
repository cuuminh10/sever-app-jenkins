using AutoMapper;
using gmc_api.DTO.CommonData;
using gmc_api.DTO.FC;
using gmc_api.DTO.HR;
using gmc_api.DTO.PP;
using gmc_api.DTO.User;
using gmc_api.Entities;

namespace gmc_api.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            /*
             * CreateMap<Source, Destination>());
             */
            // Đưa hết các cấu hình bạn muốn map giữa các object vào đây
            // Thuộc tính FullName trong xxxx được kết hợp từ FirstName và LastName trong User
            CreateMap<LoginRequest, User>();//.ForMember(d => d.FullName, o => o.MapFrom(s => $"{s.FirstName}   {s.LastName}"));
            CreateMap<UserCreateRequest, User>();
            CreateMap<UserUpdateRequest, User>();
            CreateMap<User, LoginResponse>();
            CreateMap<User, UserResponse>();
            CreateMap<PPProductionOrdrs, PPProductionOrdrResponse>();
            CreateMap<PPProductionOrdrs, PPProductionOrdrCreate>();
            CreateMap<PPProductionOrdrCreate, PPProductionOrdrs>();
            CreateMap<FavoriestCreateRequest, Favorites>();
            CreateMap<Favorites, FavoriestReponse>();
            CreateMap<ApproveDataInfo, ADInboxItemInfo>();
            CreateMap<ADInboxItemPaging, ADInboxItemResponse>();
            CreateMap<ADOutboxItemPaging, ADOutboxItemResponse>();
            CreateMap<ADInboxItemInfo, ADInboxItems>();
            CreateMap<ADInboxItems, ADInboxItemInfo>();
            CreateMap<ADInboxItems, ADInboxItemResponse>();
            CreateMap<ADOuboxItemInfo, ADOutboxItems>();
            CreateMap<ADOutboxItems, ADOutboxItemResponse>();

            CreateMap<ADDocHistoryInfo, ADDocHistorys>();
            CreateMap<ADDocHistorys, ADDocHistoryInfo>();

            CreateMap<HREmployeeOffWorkCreateRequest, HREmployeeOffWorks>();
            CreateMap<HREmployeeOffWorks, HREmployeeOffWorkReponse>();
            CreateMap<HREmployeeOffWorkReponse, HREmployeeOffWorkReponseCus>();
            CreateMap<HREmployeeOffWorkReponse, HREmployeeOffWorkReponseApproveCus>();


            CreateMap<HREmployeeOvertimeCreateRequest, HREmployeeOvertimes>();
            CreateMap<HREmployeeOvertimes, HREmployeeOvertimeReponse>();
            CreateMap<HREmployeeOvertimeReponse, HREmployeeOvertimeReponseCus>();
            CreateMap<HREmployeeOvertimeReponse, HREmployeeOvertimeReponseApproveCus>();

            CreateMap<HRTravelCalendarCreateRequest, HRTravelCalendars>();
            CreateMap<HRTravelCalendars, HRTravelCalendarReponse>();
            CreateMap<HRTravelCalendarReponse, HRTravelCalendarReponseCus>();
            CreateMap<HRTravelCalendarReponse, HRTravelCalendarReponseApproveCus>();

            CreateMap<HRTravelCalendarItemCreateRequest, HRTravelCalendarItems>();
            CreateMap<HRTravelCalendarItems, HRTravelCalendarItemReponse>();

            CreateMap<PPProductionOrdrEstFGCreate, PPProductionOrdrEstFGs>();
            CreateMap<PPProductionOrdrEstFGs, PPProductionOrdrEstFGCreate>();
            //  CreateMap<PPProductionOrdrEstFGs, PPProductionOrdr>();

            CreateMap<PPProductionOrdrEstRMCreate, PPProductionOrdrEstRMs>();
            CreateMap<PPProductionOrdrEstRMs, PPProductionOrdrEstRMCreate>();

            CreateMap<ADAttachmentCreateRequest, ADAttachments>();
            CreateMap<ADAttachments, ADAttachmentReponse>();
            CreateMap<ADCommentCreateRequest, ADComments>();
            CreateMap<ADComments, ADCommentReponse>();
            CreateMap<ADCommentReponse, ADCommentReponseCus>();

            CreateMap<JobTicketDetailBasic, JobTicketDetail>();
            CreateMap<ProdRstDetailBasic, ProdRstDetail>();
            CreateMap<JobTicketDetailBasic, ProdRstDetailBasic>()
                .ForMember(dest => dest.name, act => act.MapFrom(src => src.name))
                .ForMember(dest => dest.ordDate, act => act.MapFrom(src => src.ordDate))
                .ForMember(dest => dest.woNo, act => act.MapFrom(src => src.woNo))
                .ForMember(dest => dest.description, act => act.MapFrom(src => src.description));
            CreateMap<JobTicketItemsKQSXInfo, ProdRstItemsInfo>()
                .ForMember(dest => dest.itemID, act => act.MapFrom(src => src.itemID))
                .ForMember(dest => dest.qty, act => act.MapFrom(src => src.qty))
                .ForMember(dest => dest.phaseName, act => act.MapFrom(src => src.phaseName))
                .ForMember(dest => dest.remark, act => act.MapFrom(src => src.remark))
                .ForMember(dest => dest.unit, act => act.MapFrom(src => src.unit))
                .ForMember(dest => dest.productNo, act => act.MapFrom(src => src.productNo))
                .ForMember(dest => dest.productName, act => act.MapFrom(src => src.productName));
            CreateMap<ADInboxItemInfo, ADOuboxItemInfo>()
                .ForMember(dest => dest.ADOutboxItemAction, act => act.MapFrom(src => src.ADInboxItemAction))
                .ForMember(dest => dest.ADOutboxItemDocApprovalStatusCombo, act => act.MapFrom(src => src.ADInboxItemDocApprovalStatusCombo))
                .ForMember(dest => dest.ADOutboxItemDocNo, act => act.MapFrom(src => src.ADInboxItemDocNo))
                .ForMember(dest => dest.ADOutboxItemDocType, act => act.MapFrom(src => src.ADInboxItemDocType))
                .ForMember(dest => dest.ADOutboxItemMessage, act => act.MapFrom(src => src.ADInboxItemMessage))
                .ForMember(dest => dest.ADOutboxItemObjectID, act => act.MapFrom(src => src.ADInboxItemObjectID))
                .ForMember(dest => dest.ADOutboxItemPriorityCombo, act => act.MapFrom(src => src.ADInboxItemPriorityCombo))
                .ForMember(dest => dest.ADOutboxItemProtocol, act => act.MapFrom(src => src.ADInboxItemProtocol))
                .ForMember(dest => dest.ADOutboxItemSubject, act => act.MapFrom(src => src.ADInboxItemSubject))
                .ForMember(dest => dest.ADOutboxItemTableName, act => act.MapFrom(src => src.ADInboxItemTableName))
                .ForMember(dest => dest.ADOutboxItemTaskStatusCombo, act => act.MapFrom(src => src.ADInboxItemTaskStatusCombo));
        }
    }
}
