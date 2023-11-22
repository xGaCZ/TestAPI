using TestNetAPI.Entities;

namespace TestNetAPI
{
    public class Seeder
    {
        private readonly NetDbContext _context;
        public Seeder(NetDbContext context)
        {
            _context = context;
        }
        public void Seed()
        {
            if (_context.Database.CanConnect())
            {
                if(!_context.Roles.Any()) 
                { 
                var roles = GetRoles();
                    _context.Roles.AddRange(roles);
                    _context.SaveChanges();
                }
                if (!_context.Contacts.Any())
                {
                    var contacts = GetContacts();
                    _context.Contacts.AddRange(contacts);
                    _context.SaveChanges();
                }
            }
        }
        private IEnumerable<Role> GetRoles() 
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "Admin",
                },
                new Role()
                {
                    Name= "User",
                }
            };
            return roles;
        }
        private IEnumerable<Contact> GetContacts()
        {
            var contacts = new List<Contact>()
        {
            new Contact()
            {
                Name = "karol",
                Phone="555447757", 
               Detail = new Detail
               {
                        LastName = "LastName1",
                        Email = "karol@example.com",
                        Category = "Category1",
                        Subcategory = "Subcategory1",
                        Password = "password1",
                        Born = DateTime.Parse("1990-01-01")
               }
            },
            new Contact()
            {
                Name = "domino",
                Phone="555217757",
                Detail = new Detail
                    {
                        LastName = "LastName2",
                        Email = "domino@example.com",
                        Category = "Category2",
                        Subcategory = "Subcategory2",
                        Password = "password2",
                        Born = DateTime.Parse("1995-01-01")
                    }
            }
        };
            return contacts;
        }
    }
}
//* kod wypełnia dane w bazie jeśli baza jest pusta 