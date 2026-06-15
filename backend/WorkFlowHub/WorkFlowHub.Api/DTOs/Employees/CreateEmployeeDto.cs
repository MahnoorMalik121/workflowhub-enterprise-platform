namespace WorkFlowHub.Api.DTOs.Employees
{
    public class CreateEmployeeDto
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        public string EmployeeCode { get; set; } = string.Empty;

        public string Designation { get; set; } = string.Empty;

        public DateTime JoiningDate { get; set; }
    }
}