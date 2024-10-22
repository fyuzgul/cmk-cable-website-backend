using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using DTOs;
using DTOs.Translations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Concrete
{
    public class ProductRepository : IProductRepository
    {
        public async Task<Product> CreateProduct(Product product, List<string> translations, List<int> languageIds)

        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                await cmkCableDbContext.Products.AddAsync(product);

                await cmkCableDbContext.SaveChangesAsync();

                for (int i = 0; i < translations.Count; i++)
                {
                    var productTranslation = new ProductTranslation
                    {
                        UsageLocations = translations[i],
                        ProductId = product.Id,
                        LanguageId = languageIds[i]
                    };

                    await cmkCableDbContext.ProductTranslations.AddAsync(productTranslation);
                }

                await cmkCableDbContext.SaveChangesAsync();

                return product;
            }

        }
        public void DeleteProduct(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var product = cmkCableDbContext.Products.Find(id);

                if (product != null)
                {
                    cmkCableDbContext.Products.Remove(product);
                    cmkCableDbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("Product not found.");
                }
            }
        }
        public List<ProductDTO> GetAllProducts(int languageId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var products = cmkCableDbContext.Products.ToList();
                var productDtos = new List<ProductDTO>();

                foreach (var product in products)
                {
                    var translations = cmkCableDbContext.ProductTranslations
                        .Where(pt => pt.ProductId == product.Id && pt.LanguageId == languageId)
                        .Select(pt => new ProductTranslationDTO
                        {
                            LanguageId = pt.LanguageId,
                            UsageLocation = pt.UsageLocations,
                        }).ToList();

                    var standarts = cmkCableDbContext.ProductStandarts
                        .Where(ps => ps.ProductId == product.Id)
                        .Select(ps => ps.Standart)
                        .ToList();

                    var certificates = cmkCableDbContext.Certificates
                        .Where(c => cmkCableDbContext.ProductCertificates.Any(pc => pc.CertificateId == c.Id && pc.ProductId == product.Id))
                        .Select(c => new CertificateDTO
                        {
                            Id = c.Id,
                            Name = c.Name,
                            CertificateType = new CertificateTypeDTO
                            {
                                Id = c.TypeId,
                                Name = cmkCableDbContext.CertificateTypes
                                    .Where(ct => ct.Id == c.TypeId)
                                    .Select(ct => ct.Name)
                                    .FirstOrDefault(),
                                Image = cmkCableDbContext.CertificateTypes
                                    .Where(ct => ct.Id == c.TypeId)
                                    .Select(ct => ct.ImageData)
                                    .FirstOrDefault()
                            },
                            FileContent = c.FileContentData,
                            Image = c.ImageData,
                            ProductNames = cmkCableDbContext.ProductCertificates
                                .Where(pc => pc.CertificateId == c.Id)
                                .Select(pc => cmkCableDbContext.Products
                                    .Where(p => p.Id == pc.ProductId)
                                    .Select(p => p.Type)
                                    .FirstOrDefault())
                                .ToList()
                        }).ToList();

                    var category = cmkCableDbContext.Categories
                        .FirstOrDefault(c => c.Id == product.CategoryId);
                    var categoryTranslations = cmkCableDbContext.CategoryTranslations
                        .Where(ct => ct.CategoryId == product.CategoryId && ct.LanguageId == languageId)
                        .ToList();

                    var productDto = new ProductDTO
                    {
                        Id = product.Id,
                        Type = product.Type,
                        Image = product.Image,
                        DetailImage = product.DetailImage,
                        UsageLocations = translations,
                        Category = new CategoryDTO
                        {
                            Image = category.Image,
                            Id = category.Id,
                            Translations = categoryTranslations.Select(ct => new CategoryTranslationDTO
                            {
                                LanguageId = ct.LanguageId,
                                Name = ct.Name
                            }).ToList()
                        },
                        Standarts = standarts,
                        Certificates = certificates,
                        Structures = cmkCableDbContext.ProductStructures
                            .Where(ps => ps.ProductId == product.Id)
                            .Select(ps => ps.Structure)
                            .ToList()
                    };

                    productDtos.Add(productDto);
                }

                return productDtos;
            }
        }
        public ProductDTO GetProductWithAllTranslations(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var product = cmkCableDbContext.Products
                .FirstOrDefault(p => p.Id == id);

                if (product == null)
                {
                    throw new Exception("Product not found");
                }

                var translations = cmkCableDbContext.ProductTranslations
                    .Where(pt => pt.ProductId == id)
                    .Select(pt => new ProductTranslationDTO
                    {
                        LanguageId = pt.LanguageId,
                        UsageLocation = pt.UsageLocations,
                    }).ToList();

                var standarts = cmkCableDbContext.ProductStandarts
                    .Where(ps => ps.ProductId == id)
                    .Select(ps => ps.Standart)
                    .ToList();

                var certificates = cmkCableDbContext.Certificates
                    .Where(c => cmkCableDbContext.ProductCertificates.Any(pc => pc.CertificateId == c.Id && pc.ProductId == id))
                    .Select(c => new CertificateDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        CertificateType = new CertificateTypeDTO
                        {
                            Id = c.TypeId,
                            Name = cmkCableDbContext.CertificateTypes
                                      .Where(ct => ct.Id == c.TypeId)
                                      .Select(ct => ct.Name)
                                      .FirstOrDefault(),
                            Image = cmkCableDbContext.CertificateTypes
                                      .Where(ct => ct.Id == c.TypeId)
                                      .Select(ct => ct.ImageData)
                                      .FirstOrDefault()
                        },
                        FileContent = c.FileContentData,
                        Image = c.ImageData,
                        ProductNames = cmkCableDbContext.ProductCertificates
                            .Where(pc => pc.CertificateId == c.Id)
                            .Select(pc => cmkCableDbContext.Products
                                .Where(p => p.Id == pc.ProductId)
                                .Select(p => p.Type)
                                .FirstOrDefault())
                            .ToList()
                    }).ToList();

                var category = cmkCableDbContext.Categories
                    .FirstOrDefault(c => c.Id == product.CategoryId);

                var categoryTranslations = cmkCableDbContext.CategoryTranslations
                    .Where(ct => ct.CategoryId == product.CategoryId)
                    .ToList();

                var productDto = new ProductDTO
                {
                    Id = product.Id,
                    Type = product.Type,
                    Image = product.Image,
                    DetailImage = product.DetailImage,
                    UsageLocations = translations,
                    Category = new CategoryDTO
                    {
                        Image = category.Image,
                        Id = category.Id,
                        Translations = categoryTranslations.Select(ct => new CategoryTranslationDTO
                        {
                            LanguageId = ct.LanguageId,
                            Name = ct.Name
                        }).ToList()
                    },
                    Standarts = standarts,
                    Certificates = certificates,
                    Structures = cmkCableDbContext.ProductStructures
                        .Where(ps => ps.ProductId == id)
                        .Select(ps => ps.Structure)
                        .ToList()
                };

                return productDto;
            }
        }
        public ProductDTO GetProductById(int id, int languageId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var product = cmkCableDbContext.Products
                    .FirstOrDefault(p => p.Id == id);

                if (product == null)
                {
                    throw new Exception("Product not found");
                }

                var translation = cmkCableDbContext.ProductTranslations
                    .Where(pt => pt.ProductId == id && pt.LanguageId == languageId)
                    .Select(pt => new ProductTranslationDTO
                    {
                        LanguageId = pt.LanguageId,
                        UsageLocation = pt.UsageLocations,
                    })
                    .FirstOrDefault();

                var standarts = cmkCableDbContext.ProductStandarts
                    .Where(ps => ps.ProductId == id)
                    .Select(ps => ps.Standart)
                    .ToList();

                var certificates = cmkCableDbContext.Certificates
                    .Where(c => cmkCableDbContext.ProductCertificates.Any(pc => pc.CertificateId == c.Id && pc.ProductId == id))
                    .Select(c => new CertificateDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        CertificateType = new CertificateTypeDTO
                        {
                            Id = c.TypeId,
                            Name = cmkCableDbContext.CertificateTypes
                                      .Where(ct => ct.Id == c.TypeId)
                                      .Select(ct => ct.Name)
                                      .FirstOrDefault(),
                            Image = cmkCableDbContext.CertificateTypes
                                      .Where(ct => ct.Id == c.TypeId)
                                      .Select(ct => ct.ImageData)
                                      .FirstOrDefault()
                        },
                        FileContent = c.FileContentData,
                        Image = c.ImageData,
                        ProductNames = cmkCableDbContext.ProductCertificates
                            .Where(pc => pc.CertificateId == c.Id)
                            .Select(pc => cmkCableDbContext.Products
                                .Where(p => p.Id == pc.ProductId)
                                .Select(p => p.Type)
                                .FirstOrDefault())
                            .ToList()
                    }).ToList();

                var category = cmkCableDbContext.Categories
                    .FirstOrDefault(c => c.Id == product.CategoryId);

                var categoryTranslations = cmkCableDbContext.CategoryTranslations
                    .Where(ct => ct.CategoryId == product.CategoryId && ct.LanguageId == languageId)
                    .ToList();

                var productDto = new ProductDTO
                {
                    Id = product.Id,
                    Type = product.Type,
                    Image = product.Image,
                    DetailImage = product.DetailImage   ,
                    UsageLocations = translation != null ? new List<ProductTranslationDTO> { translation } : new List<ProductTranslationDTO>(),
                    Category = new CategoryDTO
                    {
                        Image = category.Image,
                        Id = category.Id,
                        Translations = categoryTranslations.Select(ct => new CategoryTranslationDTO
                        {
                            LanguageId = ct.LanguageId,
                            Name = ct.Name
                        }).ToList()
                    },
                    Standarts = standarts,
                    Certificates = certificates,
                    Structures = cmkCableDbContext.ProductStructures
                        .Where(ps => ps.ProductId == id)
                        .Select(ps => ps.Structure)
                        .Where(s => s.LanguageId == languageId)
                        .ToList()
                };

                return productDto;
            }
        }

        public List<ProductDTO> GetProductsByCategory(int categoryId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var products = cmkCableDbContext.Products
                    .Where(p => p.CategoryId == categoryId)
                    .ToList();

                var productDtos = products.Select(product => new ProductDTO
                {
                    Id = product.Id,
                    Type = product.Type,
                    Image = product.Image,
                    DetailImage = product.DetailImage,
                    UsageLocations = cmkCableDbContext.ProductTranslations
                        .Where(pt => pt.ProductId == product.Id)
                        .Select(pt => new ProductTranslationDTO
                        {
                            LanguageId = pt.LanguageId,
                            UsageLocation = pt.UsageLocations,
                        }).ToList(),
                    Category = new CategoryDTO
                    {
                        Id = categoryId,
                        Image = cmkCableDbContext.Categories
                            .Where(c => c.Id == categoryId)
                            .Select(c => c.Image)
                            .FirstOrDefault(),
                        Translations = cmkCableDbContext.CategoryTranslations
                            .Where(ct => ct.CategoryId == categoryId)
                            .Select(ct => new CategoryTranslationDTO
                            {
                                LanguageId = ct.LanguageId,
                                Name = ct.Name
                            }).ToList()
                    },
                    Standarts = cmkCableDbContext.ProductStandarts
                        .Where(ps => ps.ProductId == product.Id)
                        .Select(ps => ps.Standart)
                        .ToList(),
                    Certificates = cmkCableDbContext.Certificates
                        .Where(c => cmkCableDbContext.ProductCertificates.Any(pc => pc.CertificateId == c.Id && pc.ProductId == product.Id))
                        .Select(c => new CertificateDTO
                        {
                            Id = c.Id,
                            Name = c.Name,
                            CertificateType = new CertificateTypeDTO
                            {
                                Id = c.TypeId,
                                Name = cmkCableDbContext.CertificateTypes
                                    .Where(ct => ct.Id == c.TypeId)
                                    .Select(ct => ct.Name)
                                    .FirstOrDefault(),
                                Image = cmkCableDbContext.CertificateTypes
                                    .Where(ct => ct.Id == c.TypeId)
                                    .Select(ct => ct.ImageData)
                                    .FirstOrDefault()
                            },
                            FileContent = c.FileContentData,
                            Image = c.ImageData,
                            ProductNames = cmkCableDbContext.ProductCertificates
                                .Where(pc => pc.CertificateId == c.Id)
                                .Select(pc => cmkCableDbContext.Products
                                    .Where(p => p.Id == pc.ProductId)
                                    .Select(p => p.Type)
                                    .FirstOrDefault())
                                .ToList()
                        }).ToList(),
                    Structures = cmkCableDbContext.ProductStructures
                        .Where(ps => ps.ProductId == product.Id)
                        .Select(ps => ps.Structure)
                        .ToList()
                }).ToList();

                return productDtos;
            }
        }
        public List<ProductDTO> GetProductsByCertificate(int certificateId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var products = cmkCableDbContext.ProductCertificates
                    .Where(pc => pc.CertificateId == certificateId)
                    .Select(pc => pc.Product)
                    .ToList();

                var productDTOs = products.Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Type = p.Type,
                    Image = p.Image,
                    DetailImage = p.DetailImage,
                    UsageLocations = cmkCableDbContext.ProductTranslations
                        .Where(pt => pt.ProductId == p.Id)
                        .Select(pt => new ProductTranslationDTO
                        {
                            LanguageId = pt.LanguageId,
                            UsageLocation = pt.UsageLocations,
                        }).ToList(),
                    Standarts = cmkCableDbContext.ProductStandarts
                        .Where(ps => ps.ProductId == p.Id)
                        .Select(ps => ps.Standart)
                        .ToList(),
                    Structures = cmkCableDbContext.ProductStructures
                        .Where(ps => ps.ProductId == p.Id)
                        .Select(ps => ps.Structure)
                        .ToList(),
                    Certificates = cmkCableDbContext.Certificates
                        .Where(c => cmkCableDbContext.ProductCertificates.Any(pc => pc.CertificateId == c.Id && pc.ProductId == p.Id))
                        .Select(c => new CertificateDTO
                        {
                            Id = c.Id,
                            Name = c.Name,
                            CertificateType = new CertificateTypeDTO
                            {
                                Id = c.TypeId,
                                Name = cmkCableDbContext.CertificateTypes
                                    .Where(ct => ct.Id == c.TypeId)
                                    .Select(ct => ct.Name)
                                    .FirstOrDefault(),
                                Image = cmkCableDbContext.CertificateTypes
                                    .Where(ct => ct.Id == c.TypeId)
                                    .Select(ct => ct.ImageData)
                                    .FirstOrDefault()
                            },
                            FileContent = c.FileContentData,
                            Image = c.ImageData,
                        }).ToList()
                }).ToList();

                return productDTOs;
            }
        }
        public List<ProductDTO> GetProductsByStandart(int standartId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var products = cmkCableDbContext.ProductStandarts
               .Where(ps => ps.StandartId == standartId)
               .Select(ps => ps.Product)
               .ToList();
                var productDTOs = products.Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Type = p.Type,
                    Image = p.Image,
                    DetailImage = p.DetailImage,
                    UsageLocations = cmkCableDbContext.ProductTranslations
                        .Where(pt => pt.ProductId == p.Id)
                        .Select(pt => new ProductTranslationDTO
                        {
                            LanguageId = pt.LanguageId,
                            UsageLocation = pt.UsageLocations,
                        }).ToList(),
                    Standarts = cmkCableDbContext.ProductStandarts
                        .Where(ps => ps.ProductId == p.Id)
                        .Select(ps => ps.Standart)
                        .ToList(),
                    Structures = cmkCableDbContext.ProductStructures
                        .Where(ps => ps.ProductId == p.Id)
                        .Select(ps => ps.Structure)
                        .ToList(),
                    Certificates = cmkCableDbContext.Certificates
                        .Where(c => cmkCableDbContext.ProductCertificates.Any(pc => pc.CertificateId == c.Id && pc.ProductId == p.Id))
                        .Select(c => new CertificateDTO
                        {
                            Id = c.Id,
                            Name = c.Name,
                            CertificateType = new CertificateTypeDTO
                            {
                                Id = c.TypeId,
                                Name = cmkCableDbContext.CertificateTypes
                                    .Where(ct => ct.Id == c.TypeId)
                                    .Select(ct => ct.Name)
                                    .FirstOrDefault(),
                                Image = cmkCableDbContext.CertificateTypes
                                    .Where(ct => ct.Id == c.TypeId)
                                    .Select(ct => ct.ImageData)
                                    .FirstOrDefault()
                            },
                            FileContent = c.FileContentData,
                            Image = c.ImageData,
                        }).ToList()
                }).ToList();

                return productDTOs;
            }
        }
        public async Task<Product> UpdateProduct(Product product, List<string> translations, List<int> languageIds)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var existingProduct = await cmkCableDbContext.Products
                    .FirstOrDefaultAsync(p => p.Id == product.Id);
                if (existingProduct == null)
                {
                    throw new ArgumentException("Ürün Bulunamadı");
                }

               

                existingProduct.Type = product.Type; 
                existingProduct.CategoryId = product.CategoryId;

                for (int i = 0; i < translations.Count; i++)
                {
                    var translation = await cmkCableDbContext.ProductTranslations
                        .FirstOrDefaultAsync(pt => pt.ProductId == existingProduct.Id && pt.LanguageId == languageIds[i]);
                    if (translation != null)
                    {
                        translation.UsageLocations = translations[i];
                    }
                    else
                    {
                        var newTranslation = new ProductTranslation
                        {
                            UsageLocations = translations[i],
                            ProductId = existingProduct.Id,
                            LanguageId = languageIds[i]
                        };
                        cmkCableDbContext.ProductTranslations.Add(newTranslation);
                    }
                }

                var existingTranslations = await cmkCableDbContext.ProductTranslations
                    .Where(pt => pt.ProductId == existingProduct.Id)
                    .ToListAsync();

                var existingLanguageIds = existingTranslations.Select(pt => pt.LanguageId).ToList();

                foreach (var languageId in existingLanguageIds)
                {
                    if (!languageIds.Contains(languageId))
                    {
                        var translationToRemove = existingTranslations.First(pt => pt.LanguageId == languageId);
                        cmkCableDbContext.ProductTranslations.Remove(translationToRemove);
                    }
                }

                await cmkCableDbContext.SaveChangesAsync();
                return existingProduct;
            }
        }


    }
}