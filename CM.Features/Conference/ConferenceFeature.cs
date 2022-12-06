using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories;

namespace CM.Features;

public class ConferenceFeature: IConferenceFeature
{
    private readonly IConferenceRepository _conferenceRepository;
    private readonly IInscriptionRepository _inscriptionRepository;
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly IProductRepository _productRepository;
    private readonly IProgramRepository _programRepository;
    private readonly IConferencePaymentMethodRepository _conferencePaymentMethodRepository;

    public ConferenceFeature(IConferenceRepository conferenceRepository,
        IInscriptionRepository inscriptionRepository,
        IPaymentMethodRepository paymentMethodRepository,
        IProductRepository productRepository,
        IProgramRepository programRepository,
        IConferencePaymentMethodRepository conferencePaymentMethodRepository)
    {
        _conferenceRepository = conferenceRepository;
        _inscriptionRepository = inscriptionRepository;
        _paymentMethodRepository = paymentMethodRepository;
        _productRepository = productRepository;
        _programRepository = programRepository;
        _conferencePaymentMethodRepository = conferencePaymentMethodRepository;
    }

    public async Task<PagedResponse<InscriptionDto>> GetInscriptionsByConference(InscriptionQueryParams parameters)
    {
        return await _inscriptionRepository.GetInscriptionsByConference(parameters);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByConference(long conferenceId)
    {
        return await _productRepository.GetByConference(conferenceId);
    }

    public async Task<IEnumerable<ProgramDto>> GetProgramsByConference(long conferenceId)
    {
        return await _programRepository.GetByConference(conferenceId);
    }

    public async Task<ProgramDto> CreateProgramAsync(ProgramDto program)
    {
        return await _programRepository.CreateAsync(program);
    }

    public async Task<ProgramDto?> UpdateProgramAsync(ProgramDto program)
    {
        return await _programRepository.UpdateAsync(program);
    }

    public async Task DeleteProgramAsync(long programId)
    {
        await _programRepository.DeleteAsync(programId);
    }

    public async Task<IEnumerable<ConferencePaymentMethodDto>> GetPaymentMethodsByConference(long conferenceId)
    {
        return await _conferencePaymentMethodRepository.GetPaymentMethodByConference(conferenceId);
    }

    public async Task<IEnumerable<ConferencePaymentMethodDto>> SetPaymentMethodsByConference(List<ConferencePaymentMethodDto> conferencePaymentMethods)
    {
        var paymentMethods = (await _paymentMethodRepository.GetAsync()).ToList();

        var conferenceId = conferencePaymentMethods.First().Id;
        
        foreach (var paymentMethod in paymentMethods)
        {
            if (conferencePaymentMethods.All(a => a.PaymentMethodId != paymentMethod.Id))
            {
                conferencePaymentMethods.Add(new ConferencePaymentMethodDto
                {
                    ConferenceId = conferenceId,
                    PaymentMethodId = paymentMethod.Id,
                    IsAvailable = false
                });
            }
        }

        return await _conferencePaymentMethodRepository.SetPaymentMethodByConference(conferencePaymentMethods);
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