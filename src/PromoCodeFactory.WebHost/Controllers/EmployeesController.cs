using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.DataAccess.Data;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Role> _rolesRepository;

        public EmployeesController(IRepository<Employee> employeeRepository, IRepository<Role> rolesRepository)
        {
            _employeeRepository = employeeRepository;
            _rolesRepository = rolesRepository;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse(x)).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        /// <summary>
        /// Создает сотрудника по запросу
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> CreateEmployeeAsync([FromBody] EmployeeCreateRequest request)
        {
            var roleByType = await _rolesRepository.GetByPredicate(role1 => role1.Type == request.Role);
            var employee = await _employeeRepository.AddAsync(new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Roles = new List<Role>() { roleByType }
            });

            var response = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                FullName = employee.FullName,
                Roles = employee.Roles.Select(role => new RoleItemResponse(role)).ToList(),
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return response;
        }

        /// <summary>
        /// Обновляет сотрудника по запросу
        /// </summary>
        /// <param name="request"></param>
        /// <returns>bool result</returns>
        [HttpPatch]
        public async Task<ActionResult> UpdateEmployeeAsync([FromBody] EmployeeUpdateRequest request)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.Id);
            if (employee == null) return NotFound();
            employee.FirstName = string.IsNullOrEmpty(request.FirstName) ? employee.FirstName : request.FirstName;
            employee.LastName = string.IsNullOrEmpty(request.LastName) ? employee.LastName : request.LastName;
            employee.Email = string.IsNullOrEmpty(request.Email) ? employee.Email : request.Email;

            if (request.RoleType != null)
            {
                employee.Roles.Clear();
                employee.Roles.Add(await _rolesRepository.GetByPredicate(role1 => role1.Type == request.RoleType));
            }

            return Ok();
        }

        /// <summary>
        /// Удаляет сотрудника по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteEmployeeAsync(Guid id)
        {
            var employee = await _employeeRepository.DeleteAsync(id);
            if (employee == null) return NotFound();
            return Ok();
        }
    }
}