
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FinalProject.EntityF;
namespace FinalProject.EntityF
{
    public class AppDbContext : IdentityDbContext<User, Role, int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder SqlCon)
        {
            SqlCon.UseSqlServer(@"server=DESKTOP-JCR97IL;database=FinalProject;Trusted_Connection=true;Encrypt=false");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Course>(course =>
            {
                course.HasKey("Course_Id");
                course.HasOne<Instuctor>().WithMany().HasForeignKey(c => c.Ins_ID);

            });
            modelBuilder.Entity<Instuctor>(course =>
            {
                course.HasKey("Instructur_ID");
            });
            modelBuilder.Entity<Student>(course =>
            {
                course.HasKey("Student_ID");
            });
            modelBuilder.Entity<CourseEnrollment>(R =>
            {
                R.HasKey(c=> new { c.Course_ID, c.Student_ID });
                R.HasOne<Course>().WithMany(x=>x.Enrollments).HasForeignKey(c => c.Course_ID);
                R.HasOne<Student>().WithMany().HasForeignKey(c => c.Student_ID);
            });
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = nameof(UserType.Student),
                    NormalizedName = nameof(UserType.Student).ToUpper(),
                },

                new Role
                {
                    Id = 2,
                    Name = nameof(UserType.instructur),
                    NormalizedName = nameof(UserType.instructur).ToUpper(),
                }
                );
        }

        public DbSet<Course> Courses {get;set;}
        public DbSet<Student> Students { get; set; }
        public DbSet<Instuctor> Instuctors { get; set; }
        public DbSet<CourseEnrollment> CourseEnrollments { get; set; }  

    }
}
