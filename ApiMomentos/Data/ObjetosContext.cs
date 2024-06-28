using ApiObjetos.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace ApiObjetos.Data
{
    public class ObjetosContext : DbContext
    {
        public ObjetosContext(DbContextOptions<ObjetosContext> options)
            : base(options)
        {
            
        }
        public DbSet<Pacientes> Pacientes { get; set; }

        public DbSet<Usuarios> Usuarios { get; set; }



    }
}