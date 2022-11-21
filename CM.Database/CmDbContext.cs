﻿using CM.Database.Helpers;
using CM.Entities;
using CM.Entities.Base;
using CM.Entities.Helpers;
using CM.Model.Services;
using Microsoft.EntityFrameworkCore;

namespace CM.Database;

public class CmDbContext: DbContext
{
    private readonly IUserResolverService _userResolverService;
    private readonly IDateConverterService _dateConverterService;

    // C
    public DbSet<Club> Clubs { get; set; }
    public DbSet<Country> Countries { get; set; }
    
    // M
    public DbSet<Member> Members { get; set; }
    
    // R
    public DbSet<Role> Roles { get; set; }
    
    // U
    public DbSet<User> Users { get; set; }
    public DbSet<UserPassword> UserPasswords { get; set; }
    public DbSet<UserRequestToken> UserRequestTokens { get; set; }
    public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    public CmDbContext(DbContextOptions<CmDbContext> options, IUserResolverService userResolverService, IDateConverterService dateConverterService) : base(options)
    {
        _userResolverService = userResolverService;
        _dateConverterService = dateConverterService;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        foreach (var type in EntityTypes.GetEntityTypes<IConfigurableEntity>())
        {
            var instance = (IConfigurableEntity)Activator.CreateInstance(type)!;
            instance.OnModelCreating(modelBuilder);
        }

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.IsKeyless)
                continue;

            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                    property.SetValueConverter(_dateConverterService.DateConverter());
                else if (property.ClrType == typeof(DateTime?))
                    property.SetValueConverter(_dateConverterService.NullableDateConverter());
            }
        }
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateEntityMetadata();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateEntityMetadata();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        UpdateEntityMetadata();
        return base.SaveChanges();
    }

    private void UpdateEntityMetadata()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IBaseEntity && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified));

        var userId = _userResolverService.GetUserId();

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                ((IBaseEntity)entityEntry.Entity).CreatedBy = userId;
                ((IBaseEntity)entityEntry.Entity).CreatedOn = DateTime.UtcNow;
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                ((IBaseEntity)entityEntry.Entity).ModifiedBy = userId;
                ((IBaseEntity)entityEntry.Entity).ModifiedOn = DateTime.UtcNow;
            } 
            else if (entityEntry.State == EntityState.Deleted)
            {
                ((IBaseEntity) entityEntry.Entity).IsDeleted = true;
                ((IBaseEntity)entityEntry.Entity).ModifiedBy = userId;
                ((IBaseEntity)entityEntry.Entity).ModifiedOn = DateTime.UtcNow;
                entityEntry.State = EntityState.Modified;
            }
        }
    }
}