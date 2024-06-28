using System;

namespace ApiObjetos.Models.Sistema
{
    public class Respuesta
    {
        public bool Ok { get; set; }
        public string Message { get; set; }
        public Object Data { get; set; }
    }
}
