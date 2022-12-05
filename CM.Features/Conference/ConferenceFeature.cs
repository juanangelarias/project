using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories;

namespace CM.Features;

public class ConferenceFeature: IConferenceFeature
{
    private readonly IConferenceRepository _conferenceRepository;
    private readonly IInscriptionRepository _inscriptionRepository;
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly IConferencePaymentMethodRepository _conferencePaymentMethodRepository;

    public ConferenceFeature(IConferenceRepository conferenceRepository,
        IInscriptionRepository inscriptionRepository,
        IPaymentMethodRepository paymentMethodRepository,
        IConferencePaymentMethodRepository conferencePaymentMethodRepository)
    {
        _conferenceRepository = conferenceRepository;
        _inscriptionRepository = inscriptionRepository;
        _paymentMethodRepository = paymentMethodRepository;
        _conferencePaymentMethodRepository = conferencePaymentMethodRepository;
    }

    public async Task<PagedResponse<InscriptionDto>> GetInscriptionsByConference(long conferenceId)
    {
        throw new NotImplementedException();
        //return await _inscriptionRepository.GetInscriptionsByConference(conferenceId);
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

    public Task<IEnumerable<ProgramDto>> GetPaymentMethodsByConference(long conferenceId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ProgramDto>> SetPaymentMethodsByConference(long conferenceId, List<PaymentMethodDto> paymentMethods)
    {
        throw new NotImplementedException();
    }

    public async Task<ConferenceDto> CreateAsync(ConferenceDto conference)
    {
        var response = await _conferenceRepository.CreateAsync(conference);
        var paymentMethods = (await _paymentMethodRepository.GetAsync()).ToList();
        var conferencePaymentMethods = paymentMethods
            .Select(s => new ConferencePaymentMethodDto
            {
                ConferenceId = response.Id,
                PaymentMethodId = s.Id,
                IsAvailable = false
            });
        await _conferencePaymentMethodRepository.CreateManyAsync(conferencePaymentMethods);

        return response;
    }
}