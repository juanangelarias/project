using CM.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class UserRefreshToken : BaseEntity, IBaseEntity
{
    public string? Token { get; set; }
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public string? CreatedByIp { get; set; }
    public DateTime? Revoked { get; set; }
    public string? RevokedByIp { get; set; }
    public string? ReplacedByToken { get; set; }
    public string? ReasonRevoked { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsRevoked => Revoked != null;
    public bool IsActive => !IsRevoked && !IsExpired;

    public long UserId { get; set; }
    public User? User { get; set; }

    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<UserRefreshToken>(e =>
        {
            MapBaseEntityProperties(e);
            
            
        });
    }
}
