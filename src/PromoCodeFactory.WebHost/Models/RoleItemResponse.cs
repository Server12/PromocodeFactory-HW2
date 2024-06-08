using System;
using PromoCodeFactory.Core.Domain.Administration;

namespace PromoCodeFactory.WebHost.Models
{
    public class RoleItemResponse
    {
        public RoleItemResponse(Role role)
        {
            Id = role.Id;
            Type = role.Type;
            Description = role.Description;
        }

        public Guid Id { get; }

        public RoleType Type { get; }

        public string Description { get; }
    }
}