using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PromoCodeFactory.Core.Domain.Administration;

namespace PromoCodeFactory.DataAccess.Data;

public record EmployeeCreateRequest
{
    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }

    [Required] public string Email { get; set; }

    public RoleType Role { get; set; }
}