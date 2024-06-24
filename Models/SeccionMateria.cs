using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Models
{
    public class MateriaSeccion
    {
        public string? CodigoMateria { get; set; }
        [ForeignKey("CodigoMateria")]
        public virtual Materia? Materia { get; set; }

        public string? CodigoSeccion { get; set; }
        [ForeignKey("CodigoSeccion")]
        public virtual Seccion? Seccion { get; set; }
    }
}