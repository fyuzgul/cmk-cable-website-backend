using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class ProductStandartManager : IProductStandartService
    {
        private IProductStandartRepository repository;
        public ProductStandartManager()
        {
            repository = new ProductStandartRepository();   
        }
        public ProductStandart Create(ProductStandart productStandart)
        {
            return repository.Create(productStandart);  
        }
    }
}
