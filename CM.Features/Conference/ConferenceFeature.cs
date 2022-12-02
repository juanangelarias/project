using CM.Model.Dto;
using CM.Repositories;

namespace CM.Features;

public class ConferenceFeature: IConferenceFeature
{
    private readonly IConferenceRepository _conferenceRepository;
    private readonly IConferenceFeature _conferenceFeature;

    public ConferenceFeature(IConferenceRepository conferenceRepository, IConferenceFeature conferenceFeature)
    {
        _conferenceRepository = conferenceRepository;
        _conferenceFeature = conferenceFeature;
    }

    public async Task<IEnumerable<ProductDto>> GetInscriptionsByConference(long conferenceId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProductDto>> SetInscriptionsByConference(long conferenceId, List<ProductDto> products)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByConference(long conferenceId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProductDto>> SetProductsByConference(long conferenceId, List<ProductDto> products)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProgramDto>> GetProgramsByConference(long conferenceId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProgramDto>> SetProgramsByConference(long conferenceId, List<ProgramDto> programs)
    {
        throw new NotImplementedException();
    }
}