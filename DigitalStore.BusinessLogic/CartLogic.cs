using DigitalStore.BusinessLogic.Interfaces;
using DigitalStore.Models;
using DigitalStore.Models.NotForDB;

namespace DigitalStore.BusinessLogic
{
    public static class CartLogic
    {
        public static void AddItem(this Cart cart, Product product, int quantity)
        {
            CartLine line = cart.Lines
                .Where(g => g.Product.Id == product.Id)
                .FirstOrDefault();

            if (line == null)
            {
                cart.Lines.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public static void RemoveLine(this Cart cart, Product product)
        {
            cart.Lines.RemoveAll(l => l.Product.Id == product.Id);
        }

        public static decimal ComputeTotalValue(this Cart cart)
        {
            return cart.Lines.Sum(e => e.Product.ProductPrice * e.Quantity);
        }
        public static void Clear(this Cart cart)
        {
            cart.Lines.Clear();
        }
    }
}
