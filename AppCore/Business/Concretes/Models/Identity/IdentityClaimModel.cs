using AppCore.Entities.Abstracts.Identity;
using System.Collections.Generic;

namespace AppCore.Business.Concretes.Models.Identity
{
    public class IdentityClaimModel : IdentityClaimBase
    {
        public List<IdentityUserModel> IdentityUsers { get; set; }
    }
}
