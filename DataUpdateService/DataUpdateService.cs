using System.Data.SqlClient;
using System.Data;
using SharedLibrary;

static void Main()
{

    string UpdateStatus = UpdatePurchase()
}
public class DataUpdate
{
    private SqlConnection con;
    private SqlCommand com;
    private void connection()
    {
        con = new SqlConnection(GetConnectionString());
    }

    static private string GetConnectionString()
    {
        // To avoid storing the connection string in your code,
        // you can retrieve it from a configuration file.
        return "Data Source=DESKTOP-HJULM31;Initial Catalog=AmpliFLILinkly;"
            + "Integrated Security=true;";
    }
    
}