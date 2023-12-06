namespace MercadoCampesinoo.Modelos
{
    public class Cliente(string correo, string contraseña)
    {
        public int idCliente { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string contraseña { get; set; }
        public string direccion { get; set; }
    }
}
