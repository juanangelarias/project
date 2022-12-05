using AutoMapper;
using CM.Database;
using CM.Entities;
using CM.Model.Dto;
using CM.Repositories.Base;

namespace CM.Repositories;

public class ConferencePaymentMethodRepository: BaseRepository<ConferencePaymentMethod, ConferencePaymentMethodDto>, IConferencePaymentMethodRepository
{
    public ConferencePaymentMethodRepository(IMapper mapper, CmDbContext db) : base(mapper, db)
    {
    }
}