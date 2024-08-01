using Web.Models.Base;

namespace Web.Models
{
    public class UserRole : BaseModel
    {
        public string UserId { get; set; }
        public int RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
