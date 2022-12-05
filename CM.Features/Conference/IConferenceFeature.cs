using CM.Model.Dto;
using CM.Model.General;

namespace CM.Features;

public interface IConferenceFeature
{
    Task<PagedResponse<InscriptionDto>> GetInscriptionsByConference(long conferenceId);
    Task<IEnumerable<ProductDto>> GetProductsByConference(long conferenceId);
    Task<IEnumerable<ProductDto>> SetProductsByConference(long conferenceId, List<ProductDto> products);
    Task<IEnumerable<ProgramDto>> GetProgramsByConference(long conferenceId);
    Task<IEnumerable<ProgramDto>> SetProgramsByConference(long conferenceId, List<ProgramDto> programs);
    Task<IEnumerable<ProgramDto>> GetPaymentMethodsByConference(long conferenceId);
    Task<IEnumerable<ProgramDto>> SetPaymentMethodsByConference(long conferenceId, List<PaymentMethodDto> paymentMethods);
    Task<ConferenceDto> CreateAsync(ConferenceDto conference);
}