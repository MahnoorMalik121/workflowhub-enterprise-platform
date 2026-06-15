namespace WorkFlowHub.Api.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public int DepartmentId { get; set; }

        public Department Department { get; set; } = null!;

        public string EmployeeCode { get; set; } = string.Empty;

        public string Designation { get; set; } = string.Empty;

        public DateTime JoiningDate { get; set; }

        public string Status { get; set; } = "Active";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}