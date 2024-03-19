using System;
using System.Collections.Generic;
using Inmobiliaria.Models;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;


namespace Inmobiliaria.Models;
public class UsuariosRepository
{

    protected readonly string connectionString;
    
    private string[] enumRol = Enum.GetNames(typeof(EnumRol));
    public List<string> getEnumRol()
    {
        return enumRol.ToList();
    }

    public UsuariosRepository()
    {
        connectionString = Conexion.GetConnection;
    }

    public int CreateUsuario(Usuario usuario)
    {
        var res = -1;

        bool flag = CorreoExistente(usuario);
        bool flag2 = DniExistente(usuario);
        Console.WriteLine("FLAG 2: " + flag2);
        Console.WriteLine("FLAG: " + flag);
        if (!flag2)
        {
            res = -3;
            return res;
        }
        if (!flag)
        {
            res = -2;
            return res;
        }


        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            var query = @"INSERT INTO usuarios 
            (Apellido,Nombre,Dni,Telefono,Correo,Estado,Clave,Avatar,Rol)
            VALUES (@apellido, @nombre, @dni, @telefono, @correo, @estado,@clave,@avatar,@rol);
            SELECT SCOPE_IDENTITY()"; // devuelve el id insertado
            using (var command = new SqlCommand(query, connection))
            {
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@apellido", usuario.Apellido);
                command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@dni", usuario.Dni);
                command.Parameters.AddWithValue("@telefono", usuario.Telefono);
                command.Parameters.AddWithValue("@correo", usuario.Correo);
                command.Parameters.AddWithValue("@estado", true);
                command.Parameters.AddWithValue("@clave", usuario.Clave);
                if (String.IsNullOrEmpty(usuario.Avatar))
                {
                    command.Parameters.AddWithValue("@avatar", "");
                }
                else
                {
                    command.Parameters.AddWithValue("@avatar", usuario.Avatar);
                }
                command.Parameters.AddWithValue("@rol", usuario.Rol.ToString());
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                usuario.Id = res;
                connection.Close();
            }
        }
        return res;


    }

    public Usuario GetUserById(int id)
    {
        Usuario usuario = new();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = @"SELECT Id, Apellido,Nombre,Dni,Telefono,Correo,Estado,Clave,Avatar,Rol FROM usuarios WHERE Id = @id";
            string rol = "";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rol = reader.GetString("rol");
                        usuario = new()
                        {
                            Id = reader.GetInt32("id"),
                            Dni = reader.GetString("dni"),
                            Apellido = reader.GetString("apellido"),
                            Nombre = reader.GetString("nombre"),
                            Telefono = reader.GetString("telefono"),
                            Correo = reader.GetString("correo"),
                            Estado = reader.GetByte("estado"),
                            Clave = reader.GetString("clave"),
                            Avatar = reader.GetString("avatar"),
                        };
                        usuario.Rol = (EnumRol)Enum.Parse(typeof(EnumRol), rol);

                    }
                    connection.Close();
                }


            }
        }
        return usuario;
    }
    public Usuario GetUserByMail(string correo)
    {
        Usuario usuario = new();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = @"SELECT Id, Apellido,Nombre,Dni,Telefono,Correo,Estado,Clave,Avatar,Rol FROM usuarios WHERE correo = @correo";
            string rol = "";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@correo", correo);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rol = reader.GetString("rol");
                        usuario = new()
                        {
                            Id = reader.GetInt32("id"),
                            Dni = reader.GetString("dni"),
                            Apellido = reader.GetString("apellido"),
                            Nombre = reader.GetString("nombre"),
                            Telefono = reader.GetString("telefono"),
                            Correo = reader.GetString("correo"),
                            Estado = reader.GetByte("estado"),
                            Clave = reader.GetString("clave"),
                            Avatar = reader.GetString("avatar"),

                        };
                        usuario.Rol = (EnumRol)Enum.Parse(typeof(EnumRol), rol);

                    }
                    connection.Close();
                }


            }
        }
        return usuario;
    }


    public int UpdateAvatar(Usuario usuario)
    {
        var res = 0;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            var query = @"UPDATE usuarios SET
                Avatar = @avatar
                WHERE Id = @id";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", usuario.Id);
                command.Parameters.AddWithValue("@avatar", usuario.Avatar);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }
    public int UpdateUsuarioDatosPersonales(Usuario usuario)
    {

        var res = 0;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            var query = @"UPDATE usuarios SET
            Nombre = @nombre,
            Apellido = @apellido,
            Correo = @correo,
            Dni = @dni,
            Telefono = @telefono,
            Rol = @rol,
            Estado=@estado
            WHERE Id = @id";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", usuario.Id);
                command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@apellido", usuario.Apellido);
                command.Parameters.AddWithValue("@correo", usuario.Correo);
                command.Parameters.AddWithValue("@dni", usuario.Dni);
                command.Parameters.AddWithValue("@rol", usuario.Rol.ToString());
                command.Parameters.AddWithValue("@telefono", usuario.Telefono);
                command.Parameters.AddWithValue("@estado", usuario.Estado);

                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }
    public int UpdateUsuarioDatosLogueo(Usuario usuario)
    {
        var res = 0;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            var query = @"UPDATE usuarios SET           
            Clave = @clave 
            WHERE Id = @id";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", usuario.Id);
                command.Parameters.AddWithValue("@clave", usuario.Clave);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public int DeleteUsuario(int id)
    {
        var res = -1;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = @"UPDATE usuarios SET           
            Estado = 0 
            WHERE Id = @id";

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

    public List<Usuario> GetAllUsuarios()
    {
        List<Usuario> usuarios = new();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT id, dni, apellido, nombre, telefono, correo,avatar,estado,rol  FROM usuarios WHERE estado=1 ORDER BY apellido";
            string rol = "";
            using (SqlCommand command = new(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rol = reader.GetString("rol");
                        Usuario usuario = new()
                        {
                            Id = reader.GetInt32("id"),
                            Dni = reader.GetString("dni"),
                            Apellido = reader.GetString("apellido"),
                            Nombre = reader.GetString("nombre"),
                            Telefono = reader.GetString("telefono"),
                            Correo = reader.GetString("correo"),
                            Avatar = reader.GetString("avatar"),
                            Estado = reader.GetByte("estado"),

                        };
                        usuario.Rol = (EnumRol)Enum.Parse(typeof(EnumRol), rol);

                        usuarios.Add(usuario);
                    }
                    connection.Close();
                }
            }
        }
        Console.WriteLine("PropietarioS" + usuarios.Count);
        return usuarios;
    }

    public bool CorreoExistente(Usuario usuario)
    {
        Usuario usuarioAux = null; // Inicializa como null

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = @"SELECT 'Usuario' AS Tipo, Id, Nombre, Apellido, DNI, Correo
                     FROM Usuarios
                     WHERE Correo = @Correo
                     AND estado=1;";

            using (SqlCommand command = new(query, connection))
            {
                command.Parameters.AddWithValue("@Correo", usuario.Correo);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuarioAux = new Usuario()
                        {
                            Id = reader.GetInt32("Id"),
                            Dni = reader.GetString("DNI"),
                            Apellido = reader.GetString("Apellido"),
                            Nombre = reader.GetString("Nombre"),
                            Correo = reader.GetString("Correo")
                        };
                    }
                }
            }
        }

        return usuarioAux == null;
    }
    public bool DniExistente(Usuario usuario)
    {
        Usuario? usuarioAux = null;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = @"SELECT 'Usuario' AS Tipo, Id, Nombre, Apellido, DNI, Correo
                     FROM Usuarios
                     WHERE Dni = @Dni
                     AND estado=1;";

            using (SqlCommand command = new(query, connection))
            {
                command.Parameters.AddWithValue("@Dni", usuario.Dni);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuarioAux = new Usuario()
                        {
                            Id = reader.GetInt32("id"),
                            Dni = reader.GetString("dni"),
                            Apellido = reader.GetString("apellido"),
                            Nombre = reader.GetString("nombre"),
                            Correo = reader.GetString("correo")
                        };
                    }
                }
            }
        }

        return usuarioAux == null;
    }
    // public List<String> GetEnumsTipes()
    // {

    //     {
    //         List<string> enumValues = new List<string>();

    //         using (SqlConnection connection = new SqlConnection(connectionString))
    //         {
    //             connection.Open();

    //             string query = $"SHOW COLUMNS FROM usuarios LIKE 'rol'";

    //             using (SqlCommand command = new SqlCommand(query, connection))
    //             {
    //                 using (SqlDataReader reader = command.ExecuteReader())
    //                 {
    //                     if (reader.Read())
    //                     {
    //                         string enumDefinition = reader["Type"].ToString();
    //                         enumDefinition = enumDefinition.Replace("enum(", "").Replace(")", "");
    //                         string[] enumOptions = enumDefinition.Split(',');

    //                         foreach (string option in enumOptions)
    //                         {
    //                             enumValues.Add(option.Trim('\''));
    //                         }
    //                     }
    //                 }
    //             }
    //         }

    //         return enumValues;
    //     }
    // }
}
