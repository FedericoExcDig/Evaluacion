namespace ApiObjetos.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Pacientes
    {
        // Constructor
        public Pacientes()
        {
        }

        // Primary Key
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        // Columns
        [StringLength(100)]
        public string nombre { get; set; }

        public DateTime? fecha_nacimiento { get; set; }

        [StringLength(10)]
        public string genero { get; set; }

        [StringLength(20)]
        public string numero_telefono { get; set; }

        [StringLength(200)]
        public string direccion { get; set; }
    }
}