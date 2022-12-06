using AutoMapper;
using CM.Database;
using CM.Entities;
using CM.Model.Dto;
using CM.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Repositories;

public class ConferencePaymentMethodRepository : BaseRepository<ConferencePaymentMethod, ConferencePaymentMethodDto>,
    IConferencePaymentMethodRepository
{
    private readonly IMapper _mapper;

    public ConferencePaymentMethodRepository(IMapper mapper, CmDbContext db) : base(mapper, db)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<ConferencePaymentMethodDto>> GetPaymentMethodByConference(long conferenceId)
    {
        var list = await GetQuery()
            .Include(i => i.PaymentMethod)
            .Where(r => r.ConferenceId == conferenceId)
            .OrderBy(o => o.PaymentMethod!.Name)
            .ToListAsync();

        return _mapper.Map<List<ConferencePaymentMethodDto>>(list);
    }

    public async Task<IEnumerable<ConferencePaymentMethodDto>> SetPaymentMethodByConference(List<ConferencePaymentMethodDto> methods)
    {
        var conferenceId = methods.First().ConferenceId ?? 0;
        
        var toBeAdded = new List<ConferencePaymentMethodDto>();
        var toBeUpdated = new List<ConferencePaymentMethodDto>();
        foreach (var method in methods)
        {
            if (method.Id == 0)
            {
                toBeAdded.Add(method);
            }
            else
            {
                toBeUpdated.Add(method);
            }
        }

        await CreateManyAsync(toBeAdded);

        await UpdateManyAsync(toBeUpdated);

        return await GetPaymentMethodByConference(conferenceId);
    }
}