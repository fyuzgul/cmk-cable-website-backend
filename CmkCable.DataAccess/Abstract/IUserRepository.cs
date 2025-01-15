using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Abstract
{
    public interface IUserRepository
    {
        User AuthenticateAsync(string username, string password);
    }
}
