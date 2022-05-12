using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DynamicQuestionnaire.DynamicQuestionnaire.ORM
{
    public partial class ContextModel : DbContext
    {
        public ContextModel()
            : base("name=ContextModel")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CommonQuestion> CommonQuestions { get; set; }
        public virtual DbSet<Questionnaire> Questionnaires { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Typing> Typings { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserAnswer> UserAnswers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Questionnaire>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Questionnaire)
                .WillCascadeOnDelete(false);
        }
    }
}
