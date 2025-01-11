using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace Salon_De_Belleza
{
    internal class Program
    {
        static string ConnectionString = ConfigurationManager.ConnectionStrings["CN"].ConnectionString;

        static void Main(string[] args)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConnectionString;
            conn.Open();

            Console.WriteLine("Bienvenido al sistema de citas del salón de belleza.");
            bool continuar = true;

            while (continuar)
            {
                // Paso 1: Registrar cliente
                Console.WriteLine("\nPor favor, ingresa tus datos:");
                Console.Write("Nombre: ");
                string nombre = Console.ReadLine();
                Console.Write("Apellido: ");
                string apellido = Console.ReadLine();
                Console.Write("Sexo (M/F): ");
                string sexo = Console.ReadLine();
                Console.Write("Email: ");
                string email = Console.ReadLine();
                Console.Write("Teléfono: ");
                string telefono = Console.ReadLine();

                int idCliente = RegistrarCliente(nombre, apellido, sexo, email, telefono);
                Console.WriteLine($"Cliente registrado con éxito. ID Cliente: {idCliente}");

                // Paso 2: Mostrar servicios disponibles
                Console.WriteLine("\nServicios disponibles:");
                MostrarServicios();

                Console.Write("\nSelecciona el número del servicio deseado: ");
                int idServicio = int.Parse(Console.ReadLine());

                // Paso 3: Seleccionar fecha y hora de la cita
                Console.WriteLine("\nSelecciona la fecha y hora de tu cita:");
                DateTime fechaHora;
                while (true)
                {
                    Console.Write("Fecha (yyyy-MM-dd): ");
                    string fecha = Console.ReadLine();
                    Console.Write("Hora (HH:mm): ");
                    string hora = Console.ReadLine();
                    if (DateTime.TryParse($"{fecha} {hora}", out fechaHora))
                        break;

                    Console.WriteLine("Fecha y hora inválidas. Intenta nuevamente.");
                }

                List<string> serviciosSeleccionados = new List<string>();
                List<decimal> preciosServicios = new List<decimal>();
                List<string> personalAsignado = new List<string>();

                // Seleccionar y agregar el primer servicio
                string especialidadServicio = ObtenerEspecialidadServicio(idServicio);
                serviciosSeleccionados.Add(especialidadServicio);
                decimal precio = ObtenerPrecioServicio(idServicio);
                preciosServicios.Add(precio);
                string personal = ObtenerNombrePersonal(ObtenerPersonalDisponiblePorEspecialidad(ConnectionString, especialidadServicio));
                personalAsignado.Add($"Personal: {personal} - Servicio: {especialidadServicio} - Precio: ${precio}");

                // Paso 4: Preguntar si el cliente quiere agregar otro servicio
                Console.Write("\n¿Deseas agregar otro servicio? (s/n): ");
                while (Console.ReadLine()?.ToLower() == "s")
                {
                    // Mostrar los servicios nuevamente
                    MostrarServicios();
                    Console.Write("\nSelecciona el número del servicio deseado: ");
                    int otroServicioId = int.Parse(Console.ReadLine());

                    // Obtener los detalles del servicio seleccionado
                    string otroServicio = ObtenerEspecialidadServicio(otroServicioId);
                    serviciosSeleccionados.Add(otroServicio);
                    preciosServicios.Add(ObtenerPrecioServicio(otroServicioId));
                    string otroPersonal = ObtenerNombrePersonal(ObtenerPersonalDisponiblePorEspecialidad(ConnectionString, otroServicio));
                    personalAsignado.Add($"Personal: {otroPersonal} - Servicio: {otroServicio} - Precio: ${preciosServicios[preciosServicios.Count - 1]}");

                    Console.Write("\n¿Deseas agregar otro servicio? (s/n): ");
                }

                // Paso 5: Registrar la cita
                RegistrarCita(idCliente, idServicio, ObtenerPersonalDisponiblePorEspecialidad(ConnectionString, especialidadServicio), fechaHora);

                // Mostrar los servicios y el total
                Console.WriteLine("\nGracias por agendar tu cita. Estos son los servicios seleccionados:");
                decimal totalPagar = 0;
                foreach (var personalServicio in personalAsignado)
                {
                    Console.WriteLine(personalServicio);
                    totalPagar += preciosServicios[personalAsignado.IndexOf(personalServicio)];
                }

                Console.WriteLine($"\nTotal a pagar: ${totalPagar}");

                // Confirmación de otra cita
                Console.Write("\n¿Deseas agendar otra cita? (s/n): ");
                continuar = Console.ReadLine()?.ToLower() == "s";
            }

            Console.WriteLine("\nGracias por usar el sistema de citas. ¡Hasta luego!");
        }

        static int RegistrarCliente(string nombre, string apellido, string sexo, string email, string telefono)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("RegistrarCliente", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Nombre", nombre);
                command.Parameters.AddWithValue("@Apellido", apellido);
                command.Parameters.AddWithValue("@Sexo", sexo);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Telefono", telefono);
                command.Parameters.AddWithValue("@UltimaVisita", DateTime.Now);
                command.Parameters.AddWithValue("@Estatus", "Activo");

                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar()); // Devuelve el ID del cliente
            }
        }

        static void MostrarServicios()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "SELECT NumServicios, Descripcion, Precio FROM Servicios WHERE Estado = 'Disponible' AND NumServicios BETWEEN 8 AND 15;";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["NumServicios"]}. {reader["Descripcion"]} - ${reader["Precio"]}");
                }
            }
        }

        static string ObtenerEspecialidadServicio(int idServicio)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "SELECT Descripcion FROM Servicios WHERE NumServicios = @NumServicios";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NumServicios", idServicio);

                connection.Open();
                object result = command.ExecuteScalar();
                return result != null ? result.ToString() : throw new Exception("Servicio no encontrado.");
            }
        }

        static decimal ObtenerPrecioServicio(int idServicio)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "SELECT Precio FROM Servicios WHERE NumServicios = @NumServicios";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NumServicios", idServicio);

                connection.Open();
                object result = command.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : throw new Exception("Precio no encontrado.");
            }
        }

        static int ObtenerPersonalDisponiblePorEspecialidad(string connectionString, string especialidadServicio)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT TOP 1 IdPersonal FROM Personal " +
                               "WHERE Estatus = 'Activo' AND Especialidad = @Especialidad";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Especialidad", especialidadServicio);

                connection.Open();
                object result = command.ExecuteScalar();
                return result != null ? (int)result : throw new Exception("No hay personal disponible para este servicio.");
            }
        }

        static string ObtenerNombrePersonal(int idPersonal)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "SELECT Nombre, Apellido FROM Personal WHERE IdPersonal = @IdPersonal";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdPersonal", idPersonal);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return $"{reader["Nombre"]} {reader["Apellido"]}";
                }
                return "Personal no encontrado";
            }
        }

        static void RegistrarCita(int idCliente, int idServicio, int idPersonal, DateTime fechaHora)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("RegistrarCita", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdCliente", idCliente);
                command.Parameters.AddWithValue("@NumServicios", idServicio);
                command.Parameters.AddWithValue("@IdPersonal", idPersonal);
                command.Parameters.AddWithValue("@FechaHoraCita", fechaHora);
                command.Parameters.AddWithValue("@EstadoCita", "Programada");

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
