﻿using System.Collections;
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
                        I.InvoiceNumber, I.InvoiceDate, I.BuyerName
                      FROM Invoice I
                    ";

                if (!string.IsNullOrWhiteSpace(searchPhrase))
                {
                    getExistingInvoiceQuery += @" WHERE 
                                                I.InvoiceNumber LIKE @SearchPhrase OR I.InvoiceDate LIKE @SearchPhrase OR I.BuyerName LIKE @SearchPhrase
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
    }
}
