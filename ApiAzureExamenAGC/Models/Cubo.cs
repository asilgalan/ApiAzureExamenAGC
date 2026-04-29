using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAzureExamenAGC.Models
{

    [Table("CUBOS")]
    public class Cubo
    {

        [Key]
        [Column("ID_CUBO")]
        public int Id { get; set; }
        [Column("NOMBRE")]
        public string Nombre { get; set; }
        [Column("MARCA")]
        public string Marca { get; set; }
        [Column("IMAGEN")]
        public string Imagen { get; set; }
        [Column("PRECIO")]
        public int Precio { get; set; }

    }
}
