using CM.Model.Dto;
using CM.Model.General;

namespace CM.Features;

public interface IConferenceFeature
{
    Task<PagedResponse<InscriptionDto>> GetInscriptionsByConference(InscriptionQueryParams parameters);
    Task<IEnumerable<ProductDto>> GetProductsByConference(long conferenceId);
    Task<IEnumerable<ProgramDto>> GetProgramsByConference(long conferenceId);
    Task<ProgramDto> CreateProgramAsync(ProgramDto program);
    Task<ProgramDto?> UpdateProgramAsync(ProgramDto program);
    Task DeleteProgramAsync(long programId);
    Task<IEnumerable<ConferencePaymentMethodDto>> GetPaymentMethodsByConference(long conferenceId);
    Task<IEnumerable<ConferencePaymentMethodDto>> SetPaymentMethodsByConference(List<ConferencePaymentMethodDto> conferencePaymentMethods);
    Task<ConferenceDto> CreateAsync(ConferenceDto conference);
}