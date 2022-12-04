using CM.Entities;
using CM.Model.Dto;
using CM.Repositories.Base;

namespace CM.Repositories;

public interface IProgramRepository: IBaseRepository<Program,ProgramDto>
{
    Task<IEnumerable<ProgramDto>> GetByConference(long conferenceId);
}