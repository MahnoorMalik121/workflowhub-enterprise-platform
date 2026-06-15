namespace WorkFlowHub.Api.DTOs.Employees
{
    public class EmployeeResponseDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public string EmployeeCode { get; set; } = string.Empty;

        public string Designation { get; set; } = string.Empty;

        public DateTime JoiningDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}