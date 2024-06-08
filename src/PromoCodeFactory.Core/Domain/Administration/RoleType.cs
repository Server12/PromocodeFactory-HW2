using System;

namespace PromoCodeFactory.Core.Domain.Administration;

[Flags]
public enum RoleType
{
    Administrator = 0,
    PartnerManager = 1
}