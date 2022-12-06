using AutoMapper;
using CM.Database;
using CM.Entities;
using CM.Model.Dto;
using CM.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Repositories;

public class ProgramRepository: BaseRepository<Program, ProgramDto>, IProgramRepository
{
    private readonly IMapper _mapper;

    public ProgramRepository(IMapper mapper, CmDbContext db) : base(mapper, db)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProgramDto>> GetByConference(long conferenceId)
    {
        var list = await GetQuery()
            .Where(r => r.ConferenceId == conferenceId)
            .OrderBy(o => o.Name)
            .ToListAsync();

        return _mapper.Map<List<ProgramDto>>(list);
    }
}