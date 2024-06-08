using System;
using System.ComponentModel.DataAnnotations;
using PromoCodeFactory.Core.Domain.Administration;

namespace PromoCodeFactory.DataAccess.Data;

public class EmployeeUpdateRequest
{
    [Required] public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public RoleType? RoleType { get; set; } = null;
}