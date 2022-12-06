using CM.Entities;
using CM.Model.Dto;
using CM.Repositories.Base;

namespace CM.Repositories;

public interface IConferencePaymentMethodRepository: IBaseRepository<ConferencePaymentMethod, ConferencePaymentMethodDto>
{
    Task<IEnumerable<ConferencePaymentMethodDto>> GetPaymentMethodByConference(long conferenceId);
    Task<IEnumerable<ConferencePaymentMethodDto>> SetPaymentMethodByConference(List<ConferencePaymentMethodDto> methods);
}