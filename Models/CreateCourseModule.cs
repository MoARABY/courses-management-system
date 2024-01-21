namespace FinalProject.Models
{
    public class CreateCourseModule
    {
        public string Course_Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; } /* in weeks  */
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
