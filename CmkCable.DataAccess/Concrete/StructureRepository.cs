using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class StructureRepository : IStructureRepository
    {
        public Structure CreateStructure(Structure structure)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.Structures.Add(structure);
                cmkCableDbContext.SaveChanges();
                return structure;
            }
        }

        public void DeleteStructure(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var deletedStructure = cmkCableDbContext.Structures.Find(id);
                cmkCableDbContext.Structures.Remove(deletedStructure);
                cmkCableDbContext.SaveChanges();
            }
        }

        public List<Structure> GetAllStructures()
        {
            using (var cmkCableDbContext = new CmkCableDbContext()) { return cmkCableDbContext.Structures.ToList(); }
        }

        public Structure GetStructureById(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.Structures.Find(id);
            }
        }

        public List<Structure> GetStructuresByProductId(int prodcutId)
        {
            throw new NotImplementedException();
        }

        public Structure UpdateStructure(Structure structure)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.Structures.Update(structure);
                return structure;
            }
        }

      
    }
}