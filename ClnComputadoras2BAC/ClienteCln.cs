using CadComputadoras2BAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnComputadoras2BAC
{
    public class ClienteCln
    {
        public static int insertar(Cliente cliente)
        {
            using (var context = new LabComputadoras2BACEntities1())
            {
                context.Cliente.Add(cliente);
                context.SaveChanges();
                return cliente.id;
            }
        }

        public static int actualizar(Cliente cliente)
        {
            using (var context = new LabComputadoras2BACEntities1())
            {
                var existente = context.Cliente.Find(cliente.id);
                existente.cedulaIdentidad = cliente.cedulaIdentidad;
                existente.nombres = cliente.nombres;
                existente.apellidos = cliente.apellidos;
                existente.celular = cliente.celular;
                existente.usuarioRegistro = cliente.usuarioRegistro;
                return context.SaveChanges();
            }
        }
        public static int eliminar(int id, string usuarioRegistro)
        {
            using (var context = new LabComputadoras2BACEntities1())
            {
                var existente = context.Cliente.Find(id);
                existente.estado = -1;
                existente.usuarioRegistro = usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static Cliente get(int id)
        {
            using (var context = new LabComputadoras2BACEntities1())
            {
                return context.Cliente.Find(id);
            }
        }

        public static List<Cliente> listar()
        {
            using (var context = new LabComputadoras2BACEntities1())
            {
                return context.Cliente.Where(x => x.estado != -1).ToList();
            }
        }

        public static List<paClienteListar_Result> listarPa(string parametro)
        {
            using (var context = new LabComputadoras2BACEntities1())
            {
                return context.paClienteListar(parametro).ToList();
            }
        }
    }
}
