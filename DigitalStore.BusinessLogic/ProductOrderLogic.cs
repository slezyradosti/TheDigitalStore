﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalStore.BusinessLogic.Interfaces;
using DigitalStore.Models;
using DigitalStore.Models.NotForDB;
using DigitalStore.Repos.Interfaces;

namespace DigitalStore.BusinessLogic
{
    public class ProductOrderLogic : IProductOrderLogic
    {
        private readonly IProductOrderRepo _productOrderRepo;

        public ProductOrderLogic(IProductOrderRepo productOrderRepo)
        {
            _productOrderRepo = productOrderRepo;
        }
        public void AddOrderListToDb(Cart cart, Order order)
        {
            foreach (var line in cart.Lines)
            {
                ProductOrder newProductOrder = new ProductOrder();
                newProductOrder.ProductId = line.Product.Id;
                newProductOrder.OrderId = order.Id;

                _productOrderRepo.Add(newProductOrder);
            }
        }
    }
}
