using CM.Model.Dto;
using CM.Model.General;

namespace CM.App.Helper.Services;

public interface IConferenceService : IBaseService<ConferenceDto>
{
    Task<IEnumerable<ConferenceDto>> Autocomplete(AutoCompleteParams parameters);
    Task<PagedResponse<InscriptionDto>> GetInscriptions(InscriptionQueryParams parameters);
    Task<IEnumerable<ProductDto>> GetProducts(long conferenceId);
    Task<IEnumerable<ProgramDto>> GetPrograms(long conferenceId);
    Task<IEnumerable<ConferencePaymentMethodDto>> GetPaymentMethods(long conferenceId);
    Task<ProgramDto> CreateProgram(ProgramDto program);
    Task<IEnumerable<ConferencePaymentMethodDto>> SetPaymentMethods(List<ConferencePaymentMethodDto> paymentMethods);
    Task<ProgramDto> UpdateProgram(ProgramDto program);
    Task DeleteProgram(long programId);
}
