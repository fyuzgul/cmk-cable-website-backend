using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Concrete
{
    public class ManagerMailRepository : IManagerMailRepository
    {
        public ManagerMail Add(ManagerMail managerMail, List<int> formTypeIds)
        {
            using (var context = new CmkCableDbContext())
            {
                context.ManagerMails.Add(managerMail);
                context.SaveChanges(); 

                foreach (var formType in formTypeIds)
                {
                    var mailFormType = new MailFormType
                    {
                        MailId = managerMail.Id,      
                        FormTypeId = formType     
                    };

                    context.MailFormTypes.Add(mailFormType);
                }

                context.SaveChanges();  
            }

            return managerMail;  
        }


        public void Delete(int id)
        {
            using(var context = new CmkCableDbContext())
            {
                var managerMail = context.ManagerMails.Find(id);
                var mailFormTypes = context.MailFormTypes.Where(mft => mft.MailId == id).ToList();
                context.MailFormTypes.RemoveRange(mailFormTypes);
                context.ManagerMails.Remove(managerMail);
                context.SaveChanges();
            }   
        }

        public List<ManagerMailDTO> GetAll()
        {
            using(var context = new CmkCableDbContext())
            {
                var managerMails = context.ManagerMails.ToList();
                var managerMailDTOs = new List<ManagerMailDTO>();

                foreach (var managerMail in managerMails)
                {
                    var managerMailDTO = new ManagerMailDTO
                    {
                        Id = managerMail.Id,
                        Email = managerMail.Email,
                        FormTypeIds = context.MailFormTypes.Where(mft => mft.MailId == managerMail.Id).Select(mft => mft.FormTypeId).ToList()
                    };

                    managerMailDTOs.Add(managerMailDTO);
                }

                return managerMailDTOs;

            }   
        }

        public ManagerMail GetById(int id)
        {
            using(var context = new CmkCableDbContext())
            {
                return context.ManagerMails.Find(id);
            }   
        }

        public List<ManagerMail> GetByType(string type)
        {
            try
            {
                using (var context = new CmkCableDbContext())
                {
                    if (string.IsNullOrEmpty(type))
                    {
                        Console.WriteLine("GetByType called with null or empty type parameter");
                        return new List<ManagerMail>();
                    }

                    var mailFormTypes = context.MailFormTypes
                        .Where(mft => mft.FormType != null && mft.FormType.FormTypes == type)
                        .ToList();

                    if (!mailFormTypes.Any())
                    {
                        Console.WriteLine($"No MailFormTypes found for type: {type}");
                        return new List<ManagerMail>();
                    }

                    var managerMails = new List<ManagerMail>();

                    foreach (var mailFormType in mailFormTypes)
                    {
                        if (mailFormType.MailId > 0)
                        {
                            var managerMail = context.ManagerMails.Find(mailFormType.MailId);
                            if (managerMail != null && !string.IsNullOrEmpty(managerMail.Email))
                            {
                                managerMails.Add(managerMail);
                            }
                        }
                    }

                    Console.WriteLine($"Found {managerMails.Count} manager emails for type: {type}");
                    return managerMails;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByType for type '{type}': {ex.Message}");
                return new List<ManagerMail>();
            }
        }

        public ManagerMail Update(ManagerMail managerMail, List<FormType> formTypes )
        {
            using(var context = new CmkCableDbContext())
            {
                var mailFormTypes = context.MailFormTypes.Where(mft => mft.MailId == managerMail.Id).ToList();
                context.MailFormTypes.RemoveRange(mailFormTypes);

                foreach (var formType in formTypes)
                {
                    var mailFormType = new MailFormType
                    {
                        MailId = managerMail.Id,
                        FormTypeId = formType.Id
                    };

                    context.MailFormTypes.Add(mailFormType);
                }

                context.SaveChanges();

                return managerMail;
            }
        }
    }
}
