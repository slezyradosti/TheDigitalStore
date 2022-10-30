namespace DigitalStore.Models.NotForDB
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        public List<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }
}
