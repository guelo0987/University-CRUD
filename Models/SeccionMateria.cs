using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Models
{
    public class MateriaSeccion
    {
        public int? CodigoMateria { get; set; }
        [ForeignKey("CodigoMateria")]
        public virtual Materia? Materia { get; set; }

        public int? CodigoSeccion { get; set; }
        [ForeignKey("CodigoSeccion")]
        public virtual Seccion? Seccion { get; set; }
    }
}