using anime_comics.Utils.Enum;
using Microsoft.AspNetCore.Authorization;

namespace anime_comics.Utils.Attributes;

public class AuthorizeStatusAttribute : AuthorizeAttribute {
    private const string POLICY_PREFIX = "Status";
    public AuthorizeStatusAttribute(Status status = Status.Active){
        Policy = $"{POLICY_PREFIX}:{status}";
    }
}