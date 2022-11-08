using CM.Entities.Base;
using CM.Model.Dto.Base;
using CM.Model.General;

namespace CM.Repositories.Base;

public interface IBaseRepository<TEntity, TDto>
    where TEntity : class, IBaseEntity
    where TDto : class, IBaseDto
{
    public Task<TDto> CreateAsync(TDto dto);
    public Task<IEnumerable<TDto>> CreateManyAsync(IEnumerable<TDto> dtos);
    public Task<bool> DeleteAsync(long id);
    public Task<bool> DeleteManyAsync(IEnumerable<long> ids);
    public Task<IEnumerable<TDto>> GetAsync(IQueryable<TEntity>? query = null);
    public Task<PagedResponse<TDto>> GetAsync(QueryParams parameters, IQueryable<TEntity>? query = null);
    public (IQueryable<T>? query, int rowCount) GetPaginatedQueryable<T>(QueryParams parameters, IQueryable<T>? query = null);
    public Task<TDto> GetByIdAsync(long id);
    public Task<TDto?> UpdateAsync(TDto dto, TEntity? existingRecord = null);
    public Task<IEnumerable<TDto>> UpdateManyAsync(IEnumerable<TDto> dtos, IEnumerable<TEntity>? existingRecords = null);
    public Task<bool> DeletePermanentAsync(long id);
    public Task<bool> DeletePermanentManyAsync(IEnumerable<long> ids);
}