using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiTuEvento_.Models
{
    public class Persona
    {
        [Key] public int PersonaId { get; set; }
        public string Rol { get; set; } //Cliente/Administrador
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public DateTime FechaNacimiento { get; set; }
        [JsonIgnore] public ICollection<Boleto>? boletos { get; set; }
        [JsonIgnore] public virtual Carrito? Carrito { get; set; }

    }



    public class LoginDTO
    {
        public string NombreUsuario { get; set;}
        public string Contraseña { get; set; }

    }

    public class RegisterDTO
    {
        [Key] public string Correo { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Rol { get; set; }

    }
    


}
