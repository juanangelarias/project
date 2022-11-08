using AutoMapper;
using CM.Common.Configuration.Models;
using CM.Database;
using CM.Entities;
using CM.Model.Dto;
using CM.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Repositories;

public class UserPasswordRepository: BaseRepository<UserPassword, UserPasswordDto>, IUserPasswordRepository
{
    private readonly IMapper _mapper;
    private readonly PasswordSettings _passwordSettings;

    public UserPasswordRepository(IMapper mapper, CmDbContext db, PasswordSettings passwordSettings) : base(mapper, db)
    {
        _mapper = mapper;
        _passwordSettings = passwordSettings;
    }

    public async Task<UserPasswordDto?> GetCurrentPassword(long userId)
    {
        var qry = await GetQuery()
            .Where(r => r.UserId == userId)
            .OrderBy(r => userId)
            .ThenByDescending(r => r.Date)
            .FirstOrDefaultAsync();

        return _mapper.Map<UserPasswordDto>(qry);
    }

    public async Task<bool> PasswordUsedBefore(long userId, string hashedPassword)
    {
        var qry = await GetQuery()
            .Where(r => r.UserId == userId)
            .OrderBy(r => userId)
            .ThenByDescending(r => r.Date)
            .Take(_passwordSettings.PasswordHistory)
            .ToListAsync();

        return qry.Any(a => a.PasswordHash == hashedPassword);
    }
}