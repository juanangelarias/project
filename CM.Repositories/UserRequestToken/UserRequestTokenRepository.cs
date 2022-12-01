using AutoMapper;
using CM.Database;
using CM.Entities;
using CM.Model.Dto;
using CM.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Repositories;

public class UserRequestTokenRepository: BaseRepository<UserRequestToken, UserRequestTokenDto>, IUserRequestTokenRepository
{
    private readonly IMapper _mapper;

    public UserRequestTokenRepository(IMapper mapper, CmDbContext db) : base(mapper, db)
    {
        _mapper = mapper;
    }

    public async Task<UserRequestTokenDto> GetTokenDefinition(string token)
    {
        var tokenDef = await GetQuery()
            .Include(i=>i.User)
            .FirstOrDefaultAsync(f =>
                f.Expires >= DateTime.UtcNow &&
                f.Token == token);

        return _mapper.Map<UserRequestTokenDto>(tokenDef);
    }
}