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

            qry = qry.Where(r => r.Name!.ToLower().Contains(filter) ||
                                 r.Notes!.ToLower().Contains(filter));
        }

        qry = sort switch
        {
            InscriptionSort.Name
                => parameters.Descending
                    ? qry.OrderByDescending(o => o.Name)
                    : qry.OrderBy(o => o.Name),
            _
                => parameters.Descending
                    ? qry.OrderByDescending(o => o.Date)
                    : qry.OrderBy(o => o.Date)
        };

        var queryable = GetPaginatedQueryable(parameters, qry);

        var result = queryable.query != null
            ? await queryable.query.ToListAsync()
            : null;

        var mappedResult = _mapper.Map<List<InscriptionDto>>(result);

        var response = new PagedResponse<InscriptionDto>(mappedResult, queryable.rowCount);

        return response;
    }
}