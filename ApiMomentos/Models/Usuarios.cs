namespace ApiObjetos.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Usuarios
    {
   
            public int Id { get; set; }
            public string Username { get; set; }
            public string PasswordHash { get; set; } // Store hashed passwords securely
                                                     // Add more user properties as needed
    }
}