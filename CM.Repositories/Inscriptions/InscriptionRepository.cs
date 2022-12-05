using AutoMapper;
using CM.Database;
using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Model.Sorts;
using CM.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Repositories;

public class InscriptionRepository: BaseRepository<Inscription, InscriptionDto>, IInscriptionRepository
{
    private readonly IMapper _mapper;

    public InscriptionRepository(IMapper mapper, CmDbContext db) : base(mapper, db)
    {
        _mapper = mapper;
    }

    public async Task<PagedResponse<InscriptionDto>> GetInscriptionsByConference(InscriptionQueryParams parameters)
    {
        var qry = GetQuery();
        var sort = parameters.Sort ?? InscriptionSort.Date;

        if (parameters.Expand)
        {
            qry = qry
                .Include(i => i.Currency)
                .Include(i => i.PaymentMethod);
        }

        if (!string.IsNullOrEmpty(parameters.Filter))
        {
            var filter = parameters.Filter.ToLower();

            qry = qry.Where(r => r.Name.ToLower().Contains(filter));
        }

        throw new NotImplementedException();

        /*var list = await GetQuery()
            .Where(r => r.ConferenceId == conferenceId)
            .OrderBy(o => o.ConferenceId)
            .ThenBy(t => t.Date)
            .ThenBy(t => t.Name)
            .ToListAsync();

        return _mapper.Map<List<InscriptionDto>>(list);*/
    }
}