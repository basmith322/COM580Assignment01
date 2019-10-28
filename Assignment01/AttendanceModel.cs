namespace Assignment01
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AttendanceModel : DbContext
    {
        public AttendanceModel()
            : base("name=AttendanceModel")
        {
        }

        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<LearningEvent> LearningEvents { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Register> Registers { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Instructor>()
                .HasMany(e => e.Modules)
                .WithRequired(e => e.Instructor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LearningEvent>()
                .HasMany(e => e.Attendances)
                .WithRequired(e => e.LearningEvent)
                .HasForeignKey(e => e.eventID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Module>()
                .HasMany(e => e.LearningEvents)
                .WithRequired(e => e.Module)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Module>()
                .HasMany(e => e.Registers)
                .WithRequired(e => e.Module)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.Attendances)
                .WithRequired(e => e.Student)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.Registers)
                .WithRequired(e => e.Student)
                .WillCascadeOnDelete(false);
        }
    }
}
