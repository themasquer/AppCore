using System;
using System.ComponentModel.DataAnnotations;
using AppCore.Records.Abstracts;

namespace AppCore.Entities.Abstracts.Identity
{
    public abstract class IdentityUserBase : RecordBase, IRecordSoftDelete, IRecordCreatedBy, IRecordUpdatedBy
    {
        [Required]
        public string UserName { get; set; }

        public string Email { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Active { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
