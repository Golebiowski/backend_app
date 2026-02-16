using Microsoft.EntityFrameworkCore;
using backend_app.Models;
using MediatR;
using backend_app.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using backend_app.Common.Interfaces;

namespace backend_app.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    { 
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator, ICurrentUserService currentUserService) : base(options)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        // Ta linia mówi: "Chcę mieć tabelę o nazwie 'Todos' opartą na klasie TodoItem"
        public virtual DbSet<TodoItem> Todos { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userName = _currentUserService.UserName;

            foreach (var entry in ChangeTracker.Entries<TodoItem>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = userName;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                foreach (var domainEvent in entity.DomainEvents)
                {
                    await _mediator.Publish(domainEvent, cancellationToken);
                }

                entity.ClearDomainEvents();
            }

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoItem>()
                .HasQueryFilter(t => _currentUserService.isAdmin 
                                || (t.CreatedBy == _currentUserService.UserId));

            modelBuilder.Entity<TodoItem>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.CreatedBy);

            modelBuilder.Entity<Category>().HasData(
                new { Id = 1, Name = "Praca" },
                new { Id = 2, Name = "Dom" },
                new { Id = 3, Name = "Hobby" }
                );
        }
    }
}
