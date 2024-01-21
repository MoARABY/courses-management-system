namespace FinalProject.EntityF
{
    public class Course
    { 
            public int Course_Id { get; set; }
            public string Course_Name { get; set; }
            public string Description { get; set; }
            public int Duration { get; set; }
            public int Capacity { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public int Ins_ID { get; set; }

        public List<CourseEnrollment> Enrollments { get; set; } = new();
    }
}
