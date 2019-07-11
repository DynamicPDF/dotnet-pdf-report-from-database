using System;
using System.Data.SqlClient;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.LayoutEngine;
using ceTe.DynamicPDF.LayoutEngine.Data;

/* This example requires one of these licenses or higher to remove the watermark:
 *   * DynamicPDF ReportWriter Professional for .NET
 *   * DynamicPDF Core Suite Professional for .NET
*/

namespace PdfReportFromDatabase.Examples
{
    internal class ReportWithSubReportExample
    {
        private static string connectionString = "Data Source=(local);Initial Catalog=Northwind;Integrated Security=true";

        internal static void Run(string outputFilePath)
        {
            // Create the document's layout from a DLEX template
            DocumentLayout documentLayout = new DocumentLayout(Util.GetResourcePath("ReportWithSubReport.dlex"));

            // Attach to the ReportDataRequired event
            documentLayout.ReportDataRequired += DocumentLayout_ReportDataRequired;

            NameValueLayoutData layoutData = new NameValueLayoutData();
            layoutData.Add("ReportCreatedFor", "Alex Smith");

            // Layout the document and save the PDF
            Document document = documentLayout.Layout(layoutData);
            document.Draw(outputFilePath);
        }

        private static void DocumentLayout_ReportDataRequired(object sender, ReportDataRequiredEventArgs args)
        {
            if (args.ElementId == "ProductsByCategoryReport")
            {
                string sqlString =
                    "SELECT CategoryID, CategoryName " +
                    "FROM   Categories ";

                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sqlString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                args.ReportData = new DataReaderReportData(connection, reader);
                // Note that a DataTableReportData class is also available if you need to use a DataTable instead of a DataReader
            }
            else if (args.ElementId == "ProductsSubReport")
            {
                string sqlString =
                    "SELECT ProductName, QuantityPerUnit, UnitPrice " +
                    "FROM   Products " +
                    "WHERE  CategoryID = " + args.Data["CategoryID"] + " " +
                    "ORDER BY ProductName ";

                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sqlString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                args.ReportData = new DataReaderReportData(connection, reader);
                // Note that a DataTableReportData class is also available if you need to use a DataTable instead of a DataReader
            }
        }
    }
}
