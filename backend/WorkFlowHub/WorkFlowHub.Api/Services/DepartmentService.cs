using Microsoft.EntityFrameworkCore;
using WorkFlowHub.Api.Data;
using WorkFlowHub.Api.DTOs.Departments;
using WorkFlowHub.Api.Entities;

namespace WorkFlowHub.Api.Services
{
    public class DepartmentService
    {
        private readonly AppDbContext _context;

        public DepartmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DepartmentResponseDto>> GetAllDepartmentsAsync()
        {
            return await _context.Departments
                .OrderBy(d => d.Name)
                .Select(d => new DepartmentResponseDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    IsActive = d.IsActive,
                    CreatedAt = d.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<DepartmentResponseDto?> GetDepartmentByIdAsync(int id)
        {
            return await _context.Departments
                .Where(d => d.Id == id)
                .Select(d => new DepartmentResponseDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    IsActive = d.IsActive,
                    CreatedAt = d.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<DepartmentResponseDto> CreateDepartmentAsync(CreateDepartmentDto request)
        {
            var departmentNameExists = await _context.Departments
                .AnyAsync(d => d.Name.ToLower() == request.Name.ToLower());

            if (departmentNameExists)
            {
                throw new InvalidOperationException("Department with the same name already exists.");
            }

            var department = new Department
            {
                Name = request.Name.Trim(),
                Description = request.Description?.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return new DepartmentResponseDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                IsActive = department.IsActive,
                CreatedAt = department.CreatedAt
            };
        }

        public async Task<bool> UpdateDepartmentAsync(int id, UpdateDepartmentDto request)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return false;
            }

            var departmentNameExists = await _context.Departments
                .AnyAsync(d => d.Id != id && d.Name.ToLower() == request.Name.ToLower());

            if (departmentNameExists)
            {
                throw new InvalidOperationException("Department with the same name already exists.");
            }

            department.Name = request.Name.Trim();
            department.Description = request.Description?.Trim();
            department.IsActive = request.IsActive;
            department.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeactivateDepartmentAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return false;
            }

            department.IsActive = false;
            department.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}