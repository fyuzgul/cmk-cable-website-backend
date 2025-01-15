using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Concrete
{
    public class UserRepository : IUserRepository
    {
       
        public  User AuthenticateAsync(string username, string password)
        {
            using(var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            }
        }
    }
}
