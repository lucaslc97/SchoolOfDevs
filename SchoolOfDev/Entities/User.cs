using SchoolOfDev.Enums;

namespace SchoolOfDev.Entities
{
    public class User:BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public TypeUser TypeUser { get; set; }
    }
}
