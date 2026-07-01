using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WorkFlowHub.Api.Data;
using WorkFlowHub.Api.DTOs.Employees;
using WorkFlowHub.Api.Entities;
using WorkFlowHub.Api.Helpers;

namespace WorkFlowHub.Api.Services
{
    public class EmployeeService
    {
        private readonly AppDbContext _context;

        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeResponseDto>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Include(e => e.User)
                .Include(e => e.Department)
                .Where(e => e.User.IsActive)
                .OrderBy(e => e.User.FullName)
                .Select(e => new EmployeeResponseDto
                {
                    Id = e.Id,
                    UserId = e.UserId,
                    FullName = e.User.FullName,
                    Email = e.User.Email,
                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department.Name,
                    EmployeeCode = e.EmployeeCode,
                    Designation = e.Designation,
                    JoiningDate = e.JoiningDate,
                    Status = e.Status,
                    IsActive = e.User.IsActive
                })
                .ToListAsync();
        }

        public async Task<EmployeeResponseDto?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.User)
                .Include(e => e.Department)
                .Where(e => e.Id == id)
                .Select(e => new EmployeeResponseDto
                {
                    Id = e.Id,
                    UserId = e.UserId,
                    FullName = e.User.FullName,
                    Email = e.User.Email,
                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department.Name,
                    EmployeeCode = e.EmployeeCode,
                    Designation = e.Designation,
                    JoiningDate = e.JoiningDate,
                    Status = e.Status,
                    IsActive = e.User.IsActive
                })
                .FirstOrDefaultAsync();
        }

        public async Task<EmployeeResponseDto> CreateEmployeeAsync(CreateEmployeeDto request)
        {
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email.ToLower() == request.Email.ToLower());

            if (emailExists)
            {
                throw new InvalidOperationException("User with the same email already exists.");
            }

            var employeeCodeExists = await _context.Employees
                .AnyAsync(e => e.EmployeeCode.ToLower() == request.EmployeeCode.ToLower());

            if (employeeCodeExists)
            {
                throw new InvalidOperationException("Employee with the same employee code already exists.");
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == request.DepartmentId && d.IsActive);

            if (department == null)
            {
                throw new InvalidOperationException("Selected department does not exist or is inactive.");
            }

            var user = new User
            {
                FullName = request.FullName.Trim(),
                Email = request.Email.Trim().ToLower(),
                PasswordHash = PasswordHelper.HashPassword(request.Password),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
//We only add employee, but because employee.User = user, EF understands:
//Create User first
//Then create Employee with that UserId
//This is EF relationship tracking.
            var employee = new Employee
            {
                User = user,
                DepartmentId = request.DepartmentId,
                EmployeeCode = request.EmployeeCode.Trim(),
                Designation = request.Designation.Trim(),
                JoiningDate = request.JoiningDate,
                Status = "Active",
                CreatedAt = DateTime.UtcNow
            };

            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            return new EmployeeResponseDto
            {
                Id = employee.Id,
                UserId = employee.UserId,
                FullName = user.FullName,
                Email = user.Email,
                DepartmentId = employee.DepartmentId,
                DepartmentName = department.Name,
                EmployeeCode = employee.EmployeeCode,
                Designation = employee.Designation,
                JoiningDate = employee.JoiningDate,
                Status = employee.Status,
                IsActive = user.IsActive
            };
        }

        public async Task<bool> UpdateEmployeeAsync(int id, UpdateEmployeeDto request)
        {
            var employee = await _context.Employees
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return false;
            }

            var departmentExists = await _context.Departments
                .AnyAsync(d => d.Id == request.DepartmentId && d.IsActive);

            if (!departmentExists)
            {
                throw new InvalidOperationException("Selected department does not exist or is inactive.");
            }

            employee.User.FullName = request.FullName.Trim();
            employee.User.IsActive = request.IsActive;
            employee.User.UpdatedAt = DateTime.UtcNow;

            employee.DepartmentId = request.DepartmentId;
            employee.Designation = request.Designation.Trim();
            employee.JoiningDate = request.JoiningDate;
            employee.Status = request.Status.Trim();
            employee.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeactivateEmployeeAsync(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return false;
            }

            employee.Status = "Inactive";
            employee.UpdatedAt = DateTime.UtcNow;

            employee.User.IsActive = false;
            employee.User.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}