namespace WorkFlowHub.Api.DTOs.Employees
{
    public class UpdateEmployeeDto
    {
        public string FullName { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        public string Designation { get; set; } = string.Empty;

        public DateTime JoiningDate { get; set; }

        public string Status { get; set; } = "Active";

        public bool IsActive { get; set; }
    }
}