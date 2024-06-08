namespace PromoCodeFactory.Core.Domain.Administration
{
    public class Role : BaseEntity
    {
        public RoleType Type { get; set; }

        public string Description { get; set; }
    }
}