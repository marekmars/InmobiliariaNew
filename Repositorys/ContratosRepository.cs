using System;
using System.Collections.Generic;
using Inmobiliaria.Models;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;


namespace Inmobiliaria.Models;
public class ContratosRepository
{

    protected readonly string connectionString;
    public ContratosRepository()
    {
        connectionString = Conexion.GetConnection;
    }

    public List<Contrato> GetAllContratos(bool vig)
    {
        Console.WriteLine("vigentes: " + vig);
        List<Contrato> contratos = new();
        string query;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            if (vig)
            {
                query = @"SELECT id, idInquilino, idInmueble, montoMensual, fechaInicio, fechaFin, estado 
                              FROM contratos 
                              WHERE estado = 1 AND fechaFin >= GETDATE()"; // Cambiar CURDATE() a GETDATE()
            }
            else
            {
                query = @"SELECT id, idInquilino, idInmueble, montoMensual, fechaInicio, fechaFin, estado 
                              FROM contratos";
            }

            InquilinosRepository inquilinosRepo = new();
            InmueblesRepository inmueblesRepo = new();
            using (SqlCommand command = new(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inquilino inquilino = inquilinosRepo.GetInquilinoById(reader.GetInt32("idInquilino"));
                        Inmueble inmueble = inmueblesRepo.GetInmuebleById(reader.GetInt32("idInmueble"));

                        Contrato contrato = new()
                        {
                            Id = reader.GetInt32("id"),
                            IdInquilino = inquilino.Id,
                            Inquilino = inquilino,
                            IdInmueble = inmueble.Id,
                            Inmueble = inmueble,
                            MontoMensual = reader.GetDouble("montoMensual"),
                            FechaInicio = reader.GetDateTime("fechaInicio"),
                            FechaFin = reader.GetDateTime("fechaFin"),
                            Estado = reader.GetByte("estado")
                        };

                        contratos.Add(contrato);
                    }
                    connection.Close();
                }
            }
        }

        return contratos;
    }



    public List<Contrato> GetAllContratosFecha(DateTime desde, DateTime hasta)
    {

        List<Contrato> contratos = new();
        string query = @"SELECT  id ,  idInquilino ,  idInmueble ,  montoMensual ,  fechaInicio ,  fechaFin ,  estado  
        FROM  contratos  
        WHERE  fechaInicio  <=@Hasta  AND  fechaFin  >= @Desde and estado = 1";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();


            InquilinosRepository inquilinosRepo = new();
            InmueblesRepository inmueblesRepo = new();
            using (SqlCommand command = new(query, connection))
            {
                command.Parameters.AddWithValue("@Desde", desde);
                command.Parameters.AddWithValue("@Hasta", hasta);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inquilino inquilino = inquilinosRepo.GetInquilinoById(reader.GetInt32("idInquilino"));
                        Inmueble inmueble = inmueblesRepo.GetInmuebleById(reader.GetInt32("idInmueble"));

                        Contrato contrato = new()
                        {
                            Id = reader.GetInt32("id"),
                            IdInquilino = inquilino.Id,
                            Inquilino = inquilino,
                            IdInmueble = inmueble.Id,
                            Inmueble = inmueble,
                            MontoMensual = reader.GetDouble("montoMensual"),
                            FechaInicio = reader.GetDateTime("fechaInicio"),
                            FechaFin = reader.GetDateTime("fechaFin"),
                            Estado = reader.GetByte("estado"),

                        };

                        contratos.Add(contrato);
                    }
                    connection.Close();
                }
            }
        }

        return contratos;
    }
    public List<Contrato> GetAllContratosInmueble(int idInmueble)
    {

        List<Contrato> contratos = new();
        string query = @"SELECT  id ,  idInquilino ,  idInmueble ,  montoMensual ,  fechaInicio ,  fechaFin ,  estado  
        FROM  contratos  
        WHERE  idInmueble  =@idInmueble  and estado = 1";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();


            InquilinosRepository inquilinosRepo = new();
            InmueblesRepository inmueblesRepo = new();
            using (SqlCommand command = new(query, connection))
            {
                command.Parameters.AddWithValue("@idInmueble", idInmueble);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inquilino inquilino = inquilinosRepo.GetInquilinoById(reader.GetInt32("idInquilino"));
                        Inmueble inmueble = inmueblesRepo.GetInmuebleById(reader.GetInt32("idInmueble"));

                        Contrato contrato = new()
                        {
                            Id = reader.GetInt32("id"),
                            IdInquilino = inquilino.Id,
                            Inquilino = inquilino,
                            IdInmueble = inmueble.Id,
                            Inmueble = inmueble,
                            MontoMensual = reader.GetDouble("montoMensual"),
                            FechaInicio = reader.GetDateTime("fechaInicio"),
                            FechaFin = reader.GetDateTime("fechaFin"),
                            Estado = reader.GetByte("estado"),

                        };

                        contratos.Add(contrato);
                    }
                    connection.Close();
                }
            }
        }

        return contratos;
    }

    public Contrato GetContratoById(int id)
    {
        Contrato contrato = new();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = @"SELECT  id ,  idInquilino ,  idInmueble ,  fechaInicio ,  fechaFin , montoMensual ,  estado  FROM  contratos  WHERE id=@id";
            InquilinosRepository inquilinosRepo = new();
            InmueblesRepository inmueblesRepo = new();

            using (SqlCommand command = new(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inquilino inquilino = inquilinosRepo.GetInquilinoById(reader.GetInt32("idInquilino"));
                        Inmueble inmueble = inmueblesRepo.GetInmuebleById(reader.GetInt32("idInmueble"));

                        contrato = new()
                        {
                            Id = reader.GetInt32("id"),
                            IdInquilino = inquilino.Id,
                            Inquilino = inquilino,
                            IdInmueble = inmueble.Id,
                            Inmueble = inmueble,
                            MontoMensual = reader.GetDouble("montoMensual"),
                            FechaInicio = reader.GetDateTime("fechaInicio"),
                            FechaFin = reader.GetDateTime("fechaFin"),
                            Estado = reader.GetByte("estado"),

                        };


                    }
                    connection.Close();
                }
            }
        }
        return contrato;
    }

    public int CreateContrato(Contrato contrato)
    {
        var res = -1;


        bool disponibilidad = VerificarDisponibilidad(contrato);
        Console.WriteLine("ENTRO CONTRATOS");
        try
        {

            if (contrato.IdInquilino != 0 && contrato.IdInmueble != 0 && contrato.FechaFin != DateTime.MinValue &&
            contrato.FechaInicio != DateTime.MinValue)
            {
                if (!disponibilidad)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = @"INSERT INTO  contratos ( idInquilino ,  idInmueble , montoMensual ,  fechaInicio ,  fechaFin ,  estado )
                     VALUES (@idInquilino, @idInmueble,@montoMensual, @fechaInicio, @fechaFin, @estado);
                     SELECT SCOPE_IDENTITY()";

                        using (SqlCommand command = new(query, connection))
                        {
                            connection.Open();
                            command.Parameters.AddWithValue("@idInquilino", contrato.IdInquilino);
                            command.Parameters.AddWithValue("@idInmueble", contrato.IdInmueble);
                            command.Parameters.AddWithValue("@montoMensual", contrato.MontoMensual);
                            command.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
                            command.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
                            command.Parameters.AddWithValue("@estado", true);
                            res = Convert.ToInt32(command.ExecuteScalar());
                            connection.Close();

                        }

                    }
                }
                else
                {
                    res = -3;
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

    public int DeleteContrato(int id)
    {
        var res = -1;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = @"UPDATE  contratos  SET  estado  = 0 WHERE  id  = @Id;";


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

    public int UpdateContrato(Contrato contrato)
    {
        var res = -1;

        bool disponibilidad = VerificarDisponibilidad(contrato);


        if (contrato.IdInquilino != 0 && contrato.IdInmueble != 0 && contrato.MontoMensual != 0 && contrato.FechaFin != DateTime.MinValue &&
            contrato.FechaInicio != DateTime.MinValue)
        {
            if (!disponibilidad)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE  contratos  SET  fechaInicio  = @fechaInicio, fechaFin  = @fechaFin, montoMensual = @montoMensual,
                    idInquilino  = @idInquilino ,  idInmueble  = @idInmueble WHERE  id = @Id";

                    using (SqlCommand command = new(query, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@Id", contrato.Id);
                        command.Parameters.AddWithValue("@idInquilino", contrato.IdInquilino);
                        command.Parameters.AddWithValue("@idInmueble", contrato.IdInmueble);
                        command.Parameters.AddWithValue("@montoMensual", contrato.MontoMensual);
                        command.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
                        command.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
                        res = command.ExecuteNonQuery();
                        connection.Close();
                    }

                }
            }
            else
            {
                res = -3;
            }
        }

        return res;

    }


    public bool VerificarDisponibilidad(Contrato contrato)
    {
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string checkQuery = @"SELECT COUNT(*) FROM  contratos 
                              WHERE  idInmueble  = @idInmueble
                              AND  id  != @id
                              AND (@fechaInicio BETWEEN  fechaInicio  AND  fechaFin 
                                   OR @fechaFin BETWEEN  fechaInicio  AND  fechaFin ) AND  estado  = 1";

                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@id", contrato.Id);
                    checkCommand.Parameters.AddWithValue("@idInmueble", contrato.IdInmueble);
                    checkCommand.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
                    checkCommand.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);

                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                    Console.WriteLine(count);
                    return count > 0;
                }
            }
        }
    }
    public List<Contrato> GetAllContratosVencidos()
    {
        List<Contrato> contratos = new();
        string query;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            query = "SELECT  id ,  idInquilino ,  idInmueble , montoMensual ,  fechaInicio ,  fechaFin ,  estado  FROM  contratos  WHERE  estado  = 1 AND fechaFin <= CURDATE()";

            InquilinosRepository inquilinosRepo = new();
            InmueblesRepository inmueblesRepo = new();
            using (SqlCommand command = new(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Inquilino inquilino = inquilinosRepo.GetInquilinoById(reader.GetInt32("idInquilino"));
                        Inmueble inmueble = inmueblesRepo.GetInmuebleById(reader.GetInt32("idInmueble"));

                        Contrato contrato = new()
                        {
                            Id = reader.GetInt32("id"),
                            IdInquilino = inquilino.Id,
                            Inquilino = inquilino,
                            IdInmueble = inmueble.Id,
                            Inmueble = inmueble,
                            MontoMensual = reader.GetDouble("montoMensual"),
                            FechaInicio = reader.GetDateTime("fechaInicio"),
                            FechaFin = reader.GetDateTime("fechaFin"),
                            Estado = reader.GetByte("estado"),

                        };

                        contratos.Add(contrato);
                    }
                    connection.Close();
                }
            }
        }

        return contratos;
    }


}

