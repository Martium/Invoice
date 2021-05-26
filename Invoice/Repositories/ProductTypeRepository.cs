using System.Data.SQLite;
using Dapper;
using Invoice.Models;

namespace Invoice.Repositories
{
    public class ProductTypeRepository
    {
        public ProductTypeModel GetExistingProductType(int invoiceNumber, int invoiceNumberYearCreation)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getExistingServiceQuery =
                    $@"SELECT
                        PT.FirstProductType, PT.SecondProductType, PT.ThirdProductType, PT.FourthProductType, PT.FifthProductType, PT.SixthProductType, PT.SeventhProductType, PT.EighthProductType, PT.NinthProductType, PT.TenProductType, PT.EleventhProductType, PT.TwelfthProductType, PT.FirstProductTypeQuantity, PT.SecondProductTypeQuantity, PT.ThirdProductTypeQuantity, PT.FourthProductTypeQuantity, PT.FifthProductTypeQuantity, PT.SixthProductTypeQuantity, PT.SeventhProductTypeQuantity, PT.EighthProductTypeQuantity, PT.NinthProductTypeQuantity, PT.TenProductTypeQuantity, PT.EleventhProductTypeQuantity, PT.TwelfthProductTypeQuantity, PT.FirstProductTypePrice, PT.SecondProductTypePrice, PT.ThirdProductTypePrice, PT.FourthProductTypePrice, PT.FifthProductTypePrice, PT.SixthProductTypePrice, PT.SeventhProductTypePrice, PT.EighthProductTypePrice, PT.NinthProductTypePrice, PT.TenProductTypePrice, PT.EleventhProductTypePrice, PT.TwelfthProductTypePrice
                      FROM ProductType PT
                      WHERE IdByInvoiceNumber = {invoiceNumber} AND IdByInvoiceNumberYearCreation = {invoiceNumberYearCreation}
                    ";

                ProductTypeModel getExistingProductType = dbConnection.QuerySingleOrDefault(getExistingServiceQuery);

                return getExistingProductType;
            }
        }
    }
}
