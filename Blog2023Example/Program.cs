using ceTe.DynamicPDF;
using ceTe.DynamicPDF.LayoutEngine;
using ceTe.DynamicPDF.LayoutEngine.Data;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Blog2023Example
{
    class Program
    {
        static string CONNECTION_STRING = "Data Source=(local);Initial Catalog=Northwind;Integrated Security=true";
        static string BASE_PATH = "C:/temp/blog-post/";

        static void Main(string[] args)
        {
            // NOTE: Assumed you obtained these resources from the CloudAPI Resource Manager samples folder
            // samples folder name: subreport and then downloaded them into a local folder c:/temp/blog-post

            string dlexString = BASE_PATH + "subreport.dlex";
            string jsonString = BASE_PATH +"subreport.json";
            
            // Three different ways to create report with subreport
            // 1. JSON document
            // 2. Obtain data from database and then convert to JSON
            // 3. Obtain data from database and programatically use data
            
            JsonVersion(dlexString, jsonString);
            DatabaseVersionWithJson(dlexString);
            DatabaseVersionSql(dlexString);

        }

        // Create a PDF using DLEX and JSON document.

        static void JsonVersion(String dlexString, String jsonString)
        {
            string outputPdf = BASE_PATH + "subreport_json_output.pdf";

            DocumentLayout docLayout = new DocumentLayout(dlexString);
            LayoutData layoutData = new LayoutData(
                JsonConvert.DeserializeObject(
                    File.ReadAllText(jsonString)));
            Document document = docLayout.Layout(layoutData);
            document.Draw(outputPdf);
        }

        // Create a PDF using DLEX, obtain data from dtabase as JSON using "for json auto" clause

        static void DatabaseVersionWithJson(String dlexString)
        {
            string outputPdf = BASE_PATH + "subreport_db_json_output.pdf";

            string sql = "select CategoryName Name, ProductID, ProductName, QuantityPerUnit, Discontinued, UnitPrice " 
                + "from Products, Categories as ProductsByCategory "
                + "where Products.CategoryID = ProductsByCategory.CategoryID " +
                "order by CategoryName for json auto, root('ProductsByCategory')";

            var jsonResult = new StringBuilder();

            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                using (var cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    var reader = cmd.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        jsonResult.Append("[]");
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            jsonResult.Append(
                                reader.GetValue(0).ToString());
                        }
                    }
                }
            }

            DocumentLayout docLayout = new DocumentLayout(dlexString);
            LayoutData layoutData = new LayoutData(JsonConvert.DeserializeObject(jsonResult.ToString()));
            Document document = docLayout.Layout(layoutData);
            document.Draw(outputPdf);
        }

        //Create a PDF using data from database and the ReportDataRequired event.

        private static void DatabaseVersionSql(String dlexString)
        {
            string outputPdf = BASE_PATH + "subreport_db_sql_output.pdf";

             DocumentLayout documentLayout = new DocumentLayout(dlexString); ;

            // Attach to the ReportDataRequired event
            documentLayout.ReportDataRequired += DocumentLayout_ReportDataRequired;

            LayoutData layoutData = new LayoutData();

            Document document = documentLayout.Layout(layoutData);
            document.Draw(outputPdf);
        }

        private static void DocumentLayout_ReportDataRequired(object sender, ReportDataRequiredEventArgs args)
        {

            if (args.ElementId == "ProductsByCategoryReport")
            {
                string sqlString =
                    "SELECT CategoryID, CategoryName Name " +
                    "FROM   Categories ";

                SqlConnection connection = new SqlConnection(CONNECTION_STRING);
                SqlCommand command = new SqlCommand(sqlString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                args.ReportData = new DataReaderReportData(connection, reader);
            }
            else if (args.ElementId == "ProductsByCategorySubReport")
            {
                string sqlString =
                    "SELECT ProductID, ProductName, QuantityPerUnit, UnitPrice, Discontinued " +
                    "FROM   Products " +
                    "WHERE  CategoryID = " + args.Data["CategoryID"] + " " +
                    "ORDER BY ProductName ";

                SqlConnection connection = new SqlConnection(CONNECTION_STRING);
                SqlCommand command = new SqlCommand(sqlString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                args.ReportData = new DataReaderReportData(connection, reader);
            }
        }

    }
}
