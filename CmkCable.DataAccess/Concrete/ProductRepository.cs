using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class ProductRepository : IProductRepository
    {
        public List<Product> GetAllProdcuts()
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
               return  cmkCableDbContext.Products.ToList();
            }
        }

        public List<Product> GetProductsByCategory(int categoryId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.Products.Where(p=> p.CategoryId == categoryId).ToList();
            }
        }

        public Product GetProductById(int id)
        {
            using(var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.Products.Find(id);
            }
        }

        public List<Product> GetProductsByStandart(int standartId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.Products
                    .Where(p => p.ProductStandarts.Any(ps => ps.StandartId == standartId))
                    .ToList();
            }
        }

        public List<Product> GetProductsByStructure(int structureId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.Products
                    .Where(p => p.ProductStructures.Any(ps => ps.StructureId == structureId))
                    .ToList();
            }
        }

        public List<Product> GetProductsByCertificate(int certificateId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.Products
                    .Where(p => p.ProductCertificates.Any(ps => ps.CertificateId == certificateId))
                    .ToList();
            }

        }

        public Product CreateProduct(Product product)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.Products.Add(product);
                cmkCableDbContext.SaveChanges();
                return product;
            }

        }

        public Product UpdateProduct(Product product)
        {

            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.Update(product);
                cmkCableDbContext.SaveChanges();
                return product;
            }
        }

        public void DeleteProduct(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var deletedProduct = cmkCableDbContext.Products.Find(id);
                cmkCableDbContext.Products.Remove(deletedProduct);
                cmkCableDbContext.SaveChanges();
            }
        }
    }
}
