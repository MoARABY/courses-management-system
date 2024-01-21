namespace FinalProject.Models
{
    public class CourseDetailsModule
    {
        public int Course_Id { get; set; }
        public string Course_Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; } 
        public int Capacity { get; set; }
        public  DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
