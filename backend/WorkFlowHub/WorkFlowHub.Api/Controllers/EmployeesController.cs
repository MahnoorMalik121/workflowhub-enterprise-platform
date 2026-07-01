using Microsoft.AspNetCore.Mvc;
using WorkFlowHub.Api.DTOs.Employees;
using WorkFlowHub.Api.Services;

namespace WorkFlowHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeesController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();

            return Ok(employees);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                return NotFound(new
                {
                    Message = "Employee not found."
                });
            }

            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeDto request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                return BadRequest(new { Message = "Full name is required." });
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new { Message = "Email is required." });
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { Message = "Password is required." });
            }

            if (string.IsNullOrWhiteSpace(request.EmployeeCode))
            {
                return BadRequest(new { Message = "Employee code is required." });
            }

            if (string.IsNullOrWhiteSpace(request.Designation))
            {
                return BadRequest(new { Message = "Designation is required." });
            }

            try
            {
                var createdEmployee = await _employeeService.CreateEmployeeAsync(request);

                return CreatedAtAction(
                    nameof(GetEmployeeById),
                    new { id = createdEmployee.Id },
                    createdEmployee
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
        public async Task<IActionResult> UpdateEmployee(int id, UpdateEmployeeDto request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                return BadRequest(new { Message = "Full name is required." });
            }

            if (string.IsNullOrWhiteSpace(request.Designation))
            {
                return BadRequest(new { Message = "Designation is required." });
            }

            if (string.IsNullOrWhiteSpace(request.Status))
            {
                return BadRequest(new { Message = "Status is required." });
            }

            try
            {
                var updated = await _employeeService.UpdateEmployeeAsync(id, request);

                if (!updated)
                {
                    return NotFound(new
                    {
                        Message = "Employee not found."
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
        public async Task<IActionResult> DeactivateEmployee(int id)
        {
            var deactivated = await _employeeService.DeactivateEmployeeAsync(id);

            if (!deactivated)
            {
                return NotFound(new
                {
                    Message = "Employee not found."
                });
            }

            return NoContent();
        }
    }
}