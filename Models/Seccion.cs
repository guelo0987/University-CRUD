using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CRUD.Models
{
    public class Seccion
    {
        [Key]
        public string CodigoSeccion 
        { 
            get => _codigoSeccion;
            set
            {
                if (!int.TryParse(value, out int codigo) || codigo < 1 || codigo > 9 || value.Length != 2 || !value.StartsWith("0"))
                {
                    throw new ArgumentOutOfRangeException(nameof(CodigoSeccion), "El código de la sección debe estar entre 01 y 09.");
                }
                _codigoSeccion = value;
            }
        }
        private string _codigoSeccion;

        [MaxLength(50)]
        public string Horario { get; set; }

        public int Cupo { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<MateriaSeccion>? MateriaSecciones { get; set; }
    }
}