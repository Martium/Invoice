using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using Invoice.Models;

namespace Invoice.Repositories
{
    public class InvoiceRepository
    {
        public IEnumerable<InvoiceListModel> GetInvoiceList(string searchPhrase = null)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                object queryParameters = new { };

                string getExistingInvoiceQuery =
                    @"SELECT  
                        I.InvoiceNumber,I.InvoiceNumberYearCreation, I.InvoiceDate, I.BuyerName
                      FROM Invoice I
                    ";

                if (!string.IsNullOrWhiteSpace(searchPhrase))
                {
                    getExistingInvoiceQuery += @" WHERE 
                                                I.InvoiceNumber LIKE @SearchPhrase OR I.InvoiceNumberYearCreation LIKE @SearchPhrase OR I.InvoiceDate LIKE @SearchPhrase OR I.BuyerName LIKE @SearchPhrase
                                                ";

                    queryParameters = new
                    {
                        SearchPhrase = $"%{searchPhrase}%"
                    };
                }

                getExistingInvoiceQuery += @" ORDER BY 
                                               I.InvoiceNumberYearCreation DESC, I.InvoiceNumber DESC";

                IEnumerable<InvoiceListModel> InvoiceList = dbConnection.Query<InvoiceListModel>(getExistingInvoiceQuery, queryParameters);

                return InvoiceList;
            }
        }

        public int GetNextInvoiceNumber()
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getBiggestInvoiceNumberQuery =
                    @"SELECT  
                        MAX(I.InvoiceNumber)
                      FROM Invoice I
                      WHERE I.InvoiceNumberYearCreation = @InvoiceNumberYearCreation
                    ";

                object queryParameters = new
                {
                    InvoiceNumberYearCreation = DateTime.Now.Year
                };

                int? biggestOrderNumber = dbConnection.QuerySingleOrDefault<int?>(getBiggestInvoiceNumberQuery, queryParameters) ?? 0;

                return biggestOrderNumber.Value + 1;
            }
        }
    }
}
