using GoalMaster.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalMaster.Model
{
    public class GoalMasterDatabaseContext : DbContext
    {

        public GoalMasterDatabaseContext() : base(new RijndaelCrypter().Decode(System.Configuration.ConfigurationManager.
                ConnectionStrings["GoalMasterDatabaseContext"].ConnectionString))
        {
        }

        public GoalMasterDatabaseContext(string connectionString) : base(connectionString)
        {
        }
        public virtual DbSet<User>Users { get; set; }
        public virtual DbSet<RelationshipStatus> RelationshipStatuses { get; set; }
        public virtual DbSet<Relationship> Relationships { get; set; }
        public virtual DbSet<GoalType> GoalTypes { get; set; }
        public virtual DbSet<GoalDefinition> GoalDefinitions { get; set; }
        public virtual DbSet<GoalRecord> GoalRecords { get; set; }
        public virtual DbSet<GoalMember> GoalMembers { get; set; }
        public virtual DbSet<UserInfo> UsersInfo { get; set; }

        public IEnumerable<T> InsertOrUpdate<T, TKey>(IEnumerable<T> entities, Func<T, TKey> idExpression) where T : class
        {
            foreach (var entity in entities)
            {
                var existingEntity = this.Set<T>().Find(idExpression(entity));
                if (existingEntity != null)
                {
                    this.Entry(existingEntity).CurrentValues.SetValues(entity);
                    yield return existingEntity;
                }
                else
                {
                    this.Set<T>().Add(entity);
                    yield return entity;
                }
            }
            this.SaveChanges();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany<GoalDefinition>(s => s.GoalDefinitions)
                .WithMany(c => c.Users)
                .Map(cs =>
                {
                    cs.MapLeftKey("UserRefId");
                    cs.MapRightKey("GoalDefinitionRefId");
                    cs.ToTable("GoalDefinitionsUsers");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
