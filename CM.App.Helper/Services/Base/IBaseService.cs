using CM.Model.Dto.Base;
using CM.Model.General;

namespace CM.App.Helper.Services;

public interface IBaseService<T>
    where T: BaseDto
{
    Task<List<T>> Get();
    Task<PagedResponse<T>> GetPage(QueryParams parameters);
    Task<T?> Get(long id);
    Task<T?> Create(T dto);
    Task<T?> Update(T dto);
    Task Delete(long id);
    Task<HttpResponseMessage?> GetResponse(HttpRequestMessage request);
}