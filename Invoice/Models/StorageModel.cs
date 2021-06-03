using System;

namespace Invoice.Models
{
    public class StorageModel
    {
        public string StorageSerialNumber { get; set; }
        public string StorageProductName { get; set; }
        public DateTime StorageProductMadeDate { get; set; }
        public DateTime StorageProductExpireDate { get; set; }
        public double StorageProductQuantity { get; set; }
        public double StorageProductPrice { get; set; }
    }
}
