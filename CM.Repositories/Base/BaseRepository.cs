using AutoMapper;
using CM.Entities.Base;
using CM.Model.Dto.Base;
using CM.Model.General;
using Microsoft.EntityFrameworkCore;

namespace CM.Repositories.Base;

public abstract class BaseRepository<TEntity, TDto>: IBaseRepository<TEntity, TDto>
where TEntity: class, IBaseEntity
where TDto: class, IBaseDto
{
    private readonly IMapper _mapper;
    private readonly DbContext _db;

    protected BaseRepository(IMapper mapper, DbContext db)
    {
        _mapper = mapper;
        _db = db;
    }

    public virtual async Task<TDto> CreateAsync(TDto dto)
    {
        await using var t = await _db.Database.BeginTransactionAsync();

        try
        {
            var newRecord = _mapper.Map<TEntity>(dto);
            await _db.Set<TEntity>().AddAsync(newRecord);
            await _db.SaveChangesAsync();
            await t.CommitAsync();

            return _mapper.Map<TDto>(newRecord);
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<IEnumerable<TDto>> CreateManyAsync(IEnumerable<TDto> dtos)
    {
        await using var t = await _db.Database.BeginTransactionAsync();

        try
        {
            var newRecords = new List<TEntity>();

            foreach (var dto in dtos)
            {
                var entity = _mapper.Map<TEntity>(dto);
                newRecords.Add(entity);
            }

            await _db.Set<TEntity>().AddRangeAsync(newRecords);
            await _db.SaveChangesAsync();
            await t.CommitAsync();

            return _mapper.Map<IEnumerable<TDto>>(newRecords);
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<bool> DeleteAsync(long id)
    {
        await using var t = await _db.Database.BeginTransactionAsync();

        try
        {
            var existingRecord = await GetQuery().FirstOrDefaultAsync(x => x.Id == id);

            if (existingRecord == null)
            {
                throw new KeyNotFoundException();
            }

            existingRecord.IsDeleted = true;

            _db.Update(existingRecord);
            
            await _db.SaveChangesAsync();
            await t.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<bool> DeleteManyAsync(IEnumerable<long> ids)
    {
        await using var t = await _db.Database.BeginTransactionAsync();

        try
        {
            var existingRecords = await GetQuery()
                .Where(record => ids.Contains(record.Id)).ToListAsync();

            foreach (var record in existingRecords)
            {
                record.IsDeleted = true;
            }
            
            _db.UpdateRange(existingRecords);

            await _db.SaveChangesAsync();
            await t.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<IEnumerable<TDto>> GetAsync(IQueryable<TEntity>? query = null)
    {
        var qry = query ?? GetQuery();

        var queryResult = await qry
            .ToListAsync();
        return _mapper.Map<IEnumerable<TDto>>(queryResult);
    }

    public virtual async Task<PagedResponse<TDto>> GetAsync(QueryParams parameters, IQueryable<TEntity>? query = null)
    {
        var q = query ?? GetQuery();
        var queryable = GetPaginatedQueryable(parameters, q);

        var result = await queryable.query!
            .ToListAsync();
        var mappedResult = _mapper.Map<IEnumerable<TDto>>(result);
        return new PagedResponse<TDto>(mappedResult, queryable.rowCount);
    }

    public (IQueryable<T>? query, int rowCount) GetPaginatedQueryable<T>(QueryParams parameters, IQueryable<T>? qry)
    {
        if (parameters.PageSize < 1)
        {
            throw new ArgumentException("Page Size cannot be 0");
        }
        
        if (parameters.PageIndex < 0)
        {
            return (null, 0);
        }
        
        var rowCount = (qry?
            .Count()) ?? 0;

        var skip = rowCount <= parameters.PageSize ? 0 : parameters.PageSize * parameters.PageIndex;

        var pagedQuery = qry?
            .Skip(skip)
            .Take(parameters.PageSize);

        return (pagedQuery, rowCount);
    }

    public virtual async Task<TDto> GetByIdAsync(long id)
    {
        var record = await GetQuery()
            .FirstOrDefaultAsync(x => x.Id == id);

        return _mapper.Map<TDto>(record);
    }

    public virtual async Task<TDto?> UpdateAsync(TDto dto, TEntity? existingRecord = null)
    {
        await using var t = await _db.Database.BeginTransactionAsync();

        try
        {
            existingRecord ??= await GetQuery()
                .FirstOrDefaultAsync(record => record.Id == dto.Id);

            if (existingRecord == null)
            {
                return null;
            }

            _mapper.Map(dto, existingRecord);
            _db.Update(existingRecord);
            await _db.SaveChangesAsync();
            await t.CommitAsync();

            return _mapper.Map<TDto>(existingRecord);
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<TDto> UpdateAsync(TDto dto, IQueryable<TEntity> trackingQuery)
    {
        await using var t = await _db.Database.BeginTransactionAsync();

        try
        {
            var existingRecord = await trackingQuery.FirstOrDefaultAsync();
            _mapper.Map(dto, existingRecord);
            await _db.SaveChangesAsync();
            await t.CommitAsync();

            var updatedRecord = await _db.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == dto.Id);

            return _mapper.Map<TDto>(updatedRecord);
        }
        catch
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<IEnumerable<TDto>> UpdateManyAsync(IEnumerable<TDto> dtos, IEnumerable<TEntity>? existingRecords = null)
    {
        await using var t = await _db.Database.BeginTransactionAsync();

        try
        {
            existingRecords ??= await GetQuery()
                .Where(record => dtos.Select(dto => dto.Id).Contains(record.Id))
                .ToListAsync();

            var baseEntities = existingRecords
                .ToList();
            _mapper.Map(dtos, baseEntities);

            _db.UpdateRange(baseEntities);
            await _db.SaveChangesAsync();
            await t.CommitAsync();

            return _mapper.Map<IEnumerable<TDto>>(existingRecords);
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    protected virtual IQueryable<TEntity> GetQuery(bool includeDeleted = false)
    {
        return includeDeleted
            ? _db.Set<TEntity>()
                .AsNoTracking()
            : _db.Set<TEntity>()
                .Where(r => !r.IsDeleted)
                .AsNoTracking();
    }

    /*private static string GetProperPropertyName(DbContext context, string s)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        var property = context.Model.FindEntityType(typeof(TEntity))
            .GetDerivedTypesInclusive()
            .SelectMany(x => x.GetProperties()).Distinct()
            .FirstOrDefault(x => string.Equals(x.Name, s, StringComparison.CurrentCultureIgnoreCase));

        return property != null ? property.Name : string.Empty;
    }*/

    public virtual async Task<bool> DeletePermanentAsync(long id)
    {
        await using var t = await _db.Database.BeginTransactionAsync();

        try
        {
            var existingRecord = await GetQuery().FirstOrDefaultAsync(x => x.Id == id);

            if (existingRecord == null)
            {
                return false;
            }
            _db.Set<TEntity>().Remove(existingRecord);

            await _db.SaveChangesAsync();
            await t.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<bool> DeletePermanentManyAsync(IEnumerable<long> ids)
    {
        await using var t = await _db.Database.BeginTransactionAsync();

        try
        {
            var existingRecords = await GetQuery()
                .Where(record => ids.Contains(record.Id)).ToListAsync();

            _db.Set<TEntity>().RemoveRange(existingRecords);

            await _db.SaveChangesAsync();
            await t.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    //Detach all entities
    public void DetachAllEntities()
    {
        var changedEntriesCopy = _db.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || 
                      e.State == EntityState.Deleted || 
                      e.State == EntityState.Modified ||
                      e.State == EntityState.Unchanged)
            .ToList();

        
        foreach (var entry in changedEntriesCopy)
            entry.State = EntityState.Detached;
    }
    //Get History from Temporal Tables
    public async Task<ICollection<TDto>> GetHistory(long id)
    {
        var history = await _db.Set<TEntity>()
            .TemporalAll()
            .Where(r => r.Id == id)
            .ToListAsync();

        return _mapper.Map<ICollection<TDto>>(history);
    }

    public async Task<TDto> GetHistory(DateTime asOf, long id)
    {
        var history = await _db.Set<TEntity>()
            .TemporalAsOf(asOf.AddMilliseconds(-1))
            .SingleAsync(r => r.Id == id);

        return _mapper.Map<TDto>(history);
    }
}