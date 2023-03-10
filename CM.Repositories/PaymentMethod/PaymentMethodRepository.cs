using AutoMapper;
using CM.Database;
using CM.Entities;
using CM.Model.Dto;
using CM.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Repositories;

public class PaymentMethodRepository: BaseRepository<PaymentMethod, PaymentMethodDto>, IPaymentMethodRepository
{
    public PaymentMethodRepository(IMapper mapper, CmDbContext db) : base(mapper, db)
    {
    }
}