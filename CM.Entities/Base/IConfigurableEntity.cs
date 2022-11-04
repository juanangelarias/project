using Microsoft.EntityFrameworkCore;

namespace CM.Entities.Base;

public interface IConfigurableEntity
{
    public void OnModelCreating(ModelBuilder m);
}