using CM.Model.Dto;

namespace CM.Features;

public interface IConferenceFeature
{
    Task<IEnumerable<ProductDto>> GetInscriptionsByConference(long conferenceId);
    Task<IEnumerable<ProductDto>> SetInscriptionsByConference(long conferenceId, List<ProductDto> products);
    Task<IEnumerable<ProductDto>> GetProductsByConference(long conferenceId);
    Task<IEnumerable<ProductDto>> SetProductsByConference(long conferenceId, List<ProductDto> products);
    Task<IEnumerable<ProgramDto>> GetProgramsByConference(long conferenceId);
    Task<IEnumerable<ProgramDto>> SetProgramsByConference(long conferenceId, List<ProgramDto> programs);
}