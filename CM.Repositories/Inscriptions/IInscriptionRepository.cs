using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories.Base;

namespace CM.Repositories;

public interface IInscriptionRepository: IBaseRepository<Inscription, InscriptionDto>
{
    Task<PagedResponse<InscriptionDto>> GetInscriptionsByConference(InscriptionQueryParams parameters);
}