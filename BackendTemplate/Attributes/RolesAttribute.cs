using BackendTemplate.Shared.Enums;
using Microsoft.AspNetCore.Authorization;

namespace BackendTemplate.API.Attributes;

public class RolesAttribute : AuthorizeAttribute
{
    public RolesAttribute(params ROLE_TYPE[] roles)
    {
        Roles = string.Join(",", roles.Select(r => r.ToString()));
    }
}