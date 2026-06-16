using Microsoft.AspNetCore.Mvc;
using WorkFlowHub.Api.DTOs.Departments;
using WorkFlowHub.Api.Services;

namespace WorkFlowHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly DepartmentService _departmentService;

        public DepartmentsController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();

            return Ok(departments);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);

            if (department == null)
            {
                return NotFound(new
                {
                    Message = "Department not found."
                });
            }

            return Ok(department);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(CreateDepartmentDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(new
                {
                    Message = "Department name is required."
                });
            }

            try
            {
                var createdDepartment = await _departmentService.CreateDepartmentAsync(request);

                return CreatedAtAction(
                    nameof(GetDepartmentById),
                    new { id = createdDepartment.Id },
                    createdDepartment
                );
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDepartment(int id, UpdateDepartmentDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(new
                {
                    Message = "Department name is required."
                });
            }

            try
            {
                var updated = await _departmentService.UpdateDepartmentAsync(id, request);

                if (!updated)
                {
                    return NotFound(new
                    {
                        Message = "Department not found."
                    });
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeactivateDepartment(int id)
        {
            var deactivated = await _departmentService.DeactivateDepartmentAsync(id);

            if (!deactivated)
            {
                return NotFound(new
                {
                    Message = "Department not found."
                });
            }

            return NoContent();
        }
    }
}