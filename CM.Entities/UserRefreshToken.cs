using System.Text.Json.Serialization;
using CM.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class UserRefreshToken : BaseEntity, IBaseEntity
{
    [JsonIgnore] public long Id { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public string CreatedByIp { get; set; }
    public DateTime? Revoked { get; set; }
    public string RevokedByIp { get; set; }
    public string ReplacedByToken { get; set; }
    public string ReasonRevoked { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsRevoked => Revoked != null;
    public bool IsActive => !IsRevoked && !IsExpired;

    public long UserId { get; set; }
    public User User { get; set; }

    //FK - CreatedBy/ModifiedBy with User.UserId
    public User CreatedByUser { get; set; }

    public User ModifiedByUser { get; set; }
    //End

    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<UserRefreshToken>(entity =>
        {
            MapBaseEntityProperties(entity);

            //FK - CreatedBy/ModifiedBy with User.UserId
            entity.HasOne(o => o.CreatedByUser)
                .WithMany().HasForeignKey(m => m.CreatedBy);
            entity.HasOne(o => o.ModifiedByUser)
                .WithMany().HasForeignKey(m => m.ModifiedBy);
            //End
        });
    }
}
