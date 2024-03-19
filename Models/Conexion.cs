using System;

public class Conexion
{
    private static readonly string Server = "inmobiliaria.mssql.somee.com";
    private static readonly string Database = "inmobiliaria";
    private static readonly string User = "marekmars_SQLLogin_1";
    private static readonly string Password = "bvq1vxxnsk";
    private static readonly string ConnectionString = $"Data Source={Server};Initial Catalog={Database};Persist Security Info=True;User ID={User};Password={Password};";

    public static string GetConnection
    {
        get
        {
            return ConnectionString;
        }
    }

}