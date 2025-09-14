

using Microsoft.EntityFrameworkCore;

namespace Api.Model
{
    //[PrimaryKey(nameof(UserId), nameof(permision))]
    //I did it with fluent API
    public class UserPermission
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public Permision permision { get; set; }
    }
}
