using System;
using System.Collections.Generic;
using Inmobiliaria.Models;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;


namespace Inmobiliaria.Models;
public class InmueblesRepository
{
    private string[] enumTipo = Enum.GetNames(typeof(EnumTipo));
    public List<string> getEnumTipos()
    {
        return enumTipo.ToList();
    }
    private string[] enumUso = Enum.GetNames(typeof(EnumUso));
    public List<string> getEnumUso()
    {
        return enumUso.ToList();
    }



    protected readonly string connectionString;

    public InmueblesRepository()
    {
        connectionString = Conexion.GetConnection;
    }

public List<Inmueble> GetAllInmuebles()
{
    List<Inmueble> inmuebles = new List<Inmueble>();
    PropietariosRepository repoPropietarios = new PropietariosRepository();

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        string query = @"SELECT id, idPropietario, direccion, uso, tipo, cantAmbientes, latitud, longitud, precio, estado, disponible
                    FROM inmuebles
                    WHERE estado = 1";
        string tipo = "";
        string uso = "";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Propietario propietario = repoPropietarios.GetPropietarioById(reader.GetInt32("idPropietario"));
                    uso = reader.GetString("uso");
                    tipo = reader.GetString("tipo");

                    Inmueble inmueble = new Inmueble()
                    {
                        Id = reader.GetInt32("id"),
                        Propietario = propietario,
                        IdPropietario = reader.GetInt32("idPropietario"),
                        Direccion = reader.GetString("direccion"),
                        CantAmbientes = reader.GetInt32("cantAmbientes"),
                        Latitud = reader.GetDouble("latitud"),
                        Longitud = reader.GetDouble("longitud"),
                        Precio = reader.GetDouble("precio"),
                        Estado = reader.GetByte("estado") == 1,
                        Disponible = reader.GetByte("disponible") == 1
                    };

                    inmueble.Tipo = (EnumTipo)Enum.Parse(typeof(EnumTipo), tipo);
                    inmueble.Uso = (EnumUso)Enum.Parse(typeof(EnumUso), uso);

                    inmuebles.Add(inmueble);
                }
            }
        }
    }

    return inmuebles;
}



    public List<Inmueble> GetAllInmueblesDisponibles()
    {
        List<Inmueble> inmuebles = new();
        PropietariosRepository repoPropietarios = new();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = @"SELECT id, idPropietario, direccion, uso, tipo, cantAmbientes, latitud, longitud, precio, estado,disponible
            FROM inmuebles
            WHERE estado = 1 and disponible = 1";
            string tipo = "";
            string uso = "";


            PropietariosRepository propietariosRepo = new();
            using (SqlCommand command = new(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Propietario propietario = propietariosRepo.GetPropietarioById(reader.GetInt32("idPropietario"));
                        uso = reader.GetString("uso");
                        tipo = reader.GetString("tipo");


                        Inmueble inmueble = new()
                        {
                            Id = reader.GetInt32("id"),
                            Propietario = propietario,
                            IdPropietario = reader.GetInt32("idPropietario"),
                            Direccion = reader.GetString("direccion"),
                            CantAmbientes = reader.GetInt32("cantAmbientes"),
                            Latitud = reader.GetDouble("latitud"),
                            Longitud = reader.GetDouble("longitud"),
                            Precio = reader.GetDouble("precio"),
                            Estado = reader.GetByte("estado") == 1,
                        Disponible = reader.GetByte("disponible") == 1
                        };

                        inmueble.Tipo = (EnumTipo)Enum.Parse(typeof(EnumTipo), tipo);
                        inmueble.Uso = (EnumUso)Enum.Parse(typeof(EnumUso), uso);

                        inmuebles.Add(inmueble);
                    }
                    connection.Close();
                }
            }
        }

        return inmuebles;
    }
    public List<Inmueble> GetAllInmueblesFecha(DateTime fechaInicio, DateTime fechaFin)
    {

        List<Inmueble> inmuebles = new();
        PropietariosRepository propietariosRepo = new();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = @"SELECT DISTINCT inmuebles.* FROM inmuebles
LEFT JOIN contratos ON inmuebles.id = contratos.idInmueble
AND (contratos.fechaFin >= @fechaInicio AND contratos.fechaInicio <= @fechaFin)
WHERE contratos.id IS NULL OR contratos.estado = 0
AND NOT EXISTS (
    SELECT 1 FROM contratos c2
    WHERE c2.idInmueble = inmuebles.id
    AND (c2.fechaFin >= @fechaInicio AND c2.fechaInicio <= @fechaFin)
    AND c2.estado = 1
)

";

            string tipo = "";
            string uso = "";


            using (SqlCommand command = new(query, connection))
            {
                command.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                command.Parameters.AddWithValue("@fechaFin", fechaFin);
                using (SqlDataReader reader = command.ExecuteReader())
                {


                    while (reader.Read())
                    {
                        Propietario propietario = propietariosRepo.GetPropietarioById(reader.GetInt32("idPropietario"));
                        uso = reader.GetString("uso");
                        tipo = reader.GetString("tipo");


                        Inmueble inmueble = new()
                        {
                            Id = reader.GetInt32("id"),
                            Propietario = propietario,
                            IdPropietario = reader.GetInt32("idPropietario"),
                            Direccion = reader.GetString("direccion"),
                            CantAmbientes = reader.GetInt32("cantAmbientes"),
                            Latitud = reader.GetDouble("latitud"),
                            Longitud = reader.GetDouble("longitud"),
                            Precio = reader.GetDouble("precio"),
                          Estado = reader.GetByte("estado") == 1,
                        Disponible = reader.GetByte("disponible") == 1
                        };

                        inmueble.Tipo = (EnumTipo)Enum.Parse(typeof(EnumTipo), tipo);
                        inmueble.Uso = (EnumUso)Enum.Parse(typeof(EnumUso), uso);

                        inmuebles.Add(inmueble);
                    }
                    connection.Close();
                }
            }
        }

        return inmuebles;
    }
    public List<Inmueble> GetInmueblesPropietario(int idPropietario)
    {
        List<Inmueble> inmuebles = new();
        PropietariosRepository repoPropietarios = new();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = @"SELECT id, idPropietario, direccion, uso, tipo, cantAmbientes, latitud, longitud, precio, estado,disponible
            FROM inmuebles
            WHERE estado = 1 AND idPropietario = @idPropietario";
            string tipo = "";
            string uso = "";


            PropietariosRepository propietariosRepo = new();
            using (SqlCommand command = new(query, connection))
            {
                command.Parameters.AddWithValue("@idPropietario", idPropietario);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Propietario propietario = propietariosRepo.GetPropietarioById(reader.GetInt32("idPropietario"));
                        uso = reader.GetString("uso");
                        tipo = reader.GetString("tipo");


                        Inmueble inmueble = new()
                        {
                            Id = reader.GetInt32("id"),
                            Propietario = propietario,
                            IdPropietario = reader.GetInt32("idPropietario"),
                            Direccion = reader.GetString("direccion"),
                            CantAmbientes = reader.GetInt32("cantAmbientes"),
                            Latitud = reader.GetDouble("latitud"),
                            Longitud = reader.GetDouble("longitud"),
                            Precio = reader.GetDouble("precio"),
                           Estado = reader.GetByte("estado") == 1,
                        Disponible = reader.GetByte("disponible") == 1
                        };

                        inmueble.Tipo = (EnumTipo)Enum.Parse(typeof(EnumTipo), tipo);
                        inmueble.Uso = (EnumUso)Enum.Parse(typeof(EnumUso), uso);

                        inmuebles.Add(inmueble);
                    }
                    connection.Close();
                }
            }
        }

        return inmuebles;
    }

    public Inmueble GetInmuebleById(int id)
    {
        Inmueble inmueble = new();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT id, idPropietario, direccion, uso, tipo, cantAmbientes, latitud, longitud, precio,estado,disponible FROM inmuebles WHERE id=@id";
            PropietariosRepository propietariosRepo = new();
            string tipo = "";
            string uso = "";
            using (SqlCommand command = new(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Propietario propietario = propietariosRepo.GetPropietarioById(reader.GetInt32("idPropietario"));
                        uso = reader.GetString("uso");
                        tipo = reader.GetString("tipo");

                        inmueble = new()
                        {
                            Id = reader.GetInt32("id"),
                            IdPropietario = propietario.Id, //
                            Propietario = propietario,
                            Direccion = reader.GetString("direccion"),
                            CantAmbientes = reader.GetInt32("cantAmbientes"),
                            Latitud = reader.GetDouble("latitud"),
                            Longitud = reader.GetDouble("longitud"),
                            Precio = reader.GetDouble("precio"),
                            Estado = reader.GetByte("estado") == 1,
                        Disponible = reader.GetByte("disponible") == 1
                        };
                        inmueble.Tipo = (EnumTipo)Enum.Parse(typeof(EnumTipo), tipo);
                        inmueble.Uso = (EnumUso)Enum.Parse(typeof(EnumUso), uso);


                    }
                    connection.Close();
                }
            }
        }
        return inmueble;
    }

    public int CreateInmueble(Inmueble inmueble)
    {
        var res = -1;
        Console.WriteLine(inmueble.Uso.ToString());
        Console.WriteLine(inmueble.Tipo.ToString());

        try
        {
            if (inmueble.Direccion != "" && inmueble.Uso.ToString() != "" && inmueble.Tipo.ToString() != "" && inmueble.CantAmbientes != 0 && inmueble.IdPropietario != 0
                && inmueble.Latitud != 0 && inmueble.Longitud != 0 && inmueble.Precio != 0)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO inmuebles (idPropietario, direccion, uso, tipo, cantAmbientes, latitud, longitud, precio, estado, disponible)
                 VALUES (@IdPropietario, @Direccion, @Uso, @Tipo, @CantAmbientes, @Latitud, @Longitud, @Precio, @Estado, @Disponible);
                 SELECT SCOPE_IDENTITY()";

                    using (SqlCommand command = new(query, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@IdPropietario", inmueble.IdPropietario);
                        command.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
                        command.Parameters.AddWithValue("@Uso", inmueble.Uso.ToString());
                        command.Parameters.AddWithValue("@Tipo", inmueble.Tipo.ToString());
                        command.Parameters.AddWithValue("@CantAmbientes", inmueble.CantAmbientes);
                        command.Parameters.AddWithValue("@Latitud", inmueble.Latitud);
                        command.Parameters.AddWithValue("@Longitud", inmueble.Longitud);
                        command.Parameters.AddWithValue("@Precio", inmueble.Precio);
                        command.Parameters.AddWithValue("@Estado", true);
                        command.Parameters.AddWithValue("@Disponible", true);
                        res = Convert.ToInt32(command.ExecuteScalar());
                        connection.Close();

                    }

                }
            }
            else
            {
                res = -1;
            }
        }

        catch (System.Exception)
        {
            throw;
        }
        return res;

    }


    public int DeleteInmueble(int id)
    {
        var res = -1;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = @"UPDATE inmuebles SET estado = 0,disponible = 0 WHERE id = @Id;";

            using (SqlCommand command = new(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                connection.Close();
            }

        }
        return res;

    }

    public int UpdateInmueble(Inmueble inmueble)
    {
       
        var res = -1;

        if (inmueble.Direccion != "" && inmueble.Uso.ToString() != "" && inmueble.Tipo.ToString() != "" && inmueble.CantAmbientes != 0 && inmueble.IdPropietario != 0
                        && inmueble.Latitud != 0 && inmueble.Longitud != 0 && inmueble.Precio != 0)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE inmuebles SET idPropietario = @IdPropietario, direccion = @Direccion, uso = @Uso, tipo = @Tipo,
                cantAmbientes = @CantAmbientes, latitud = @Latitud, longitud = @Longitud, precio = @Precio WHERE id = @Id;";


                using (SqlCommand command = new(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Id", inmueble.Id);
                    command.Parameters.AddWithValue("@IdPropietario", inmueble.IdPropietario);
                    command.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
                    command.Parameters.AddWithValue("@Uso", inmueble.Uso.ToString());
                    command.Parameters.AddWithValue("@Tipo", inmueble.Tipo.ToString());
                    command.Parameters.AddWithValue("@CantAmbientes", inmueble.CantAmbientes);
                    command.Parameters.AddWithValue("@Latitud", inmueble.Latitud);
                    command.Parameters.AddWithValue("@Longitud", inmueble.Longitud);
                    command.Parameters.AddWithValue("@Precio", inmueble.Precio);
                    res = command.ExecuteNonQuery();
                    connection.Close();
                    Console.WriteLine("RESULT: " + res);
                }

            }
        }

        return res;

    }

   public int UpdateInmuebleDisponible(Inmueble inmueble)
{
    var res = -1;
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        string query = @"UPDATE inmuebles SET disponible = @Disponible WHERE id = @Id";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id", inmueble.Id);
            command.Parameters.AddWithValue("@Disponible", inmueble.Disponible ? 1 : 0); // Convertir bool a int (1 o 0)
            
            connection.Open();
            res = command.ExecuteNonQuery();
            connection.Close();
        }
    }

    return res;
}



}
