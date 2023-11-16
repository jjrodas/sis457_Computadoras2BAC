﻿using CadComputadoras2BAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnComputadoras2BAC
{
    public class ProductoCln
    {
        public static int insertar(Producto producto)
        {
            using (var context = new LabComputadoras2BACEntities1())
            {
                context.Producto.Add(producto);
                context.SaveChanges();
                return producto.id;
            }
        }

        public static int actualizar(Producto producto)
        {
            using (var context = new LabComputadoras2BACEntities1())
            {
                var existente = context.Producto.Find(producto.id);
                existente.idCategoria = producto.idCategoria;
                existente.codigo = producto.codigo;
                existente.descripcion = producto.descripcion;
                existente.marca = producto.marca;
                existente.precioVenta = producto.precioVenta;
                existente.usuarioRegistro = producto.usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id, string usuarioRegistro)
        {
            using (var context = new LabComputadoras2BACEntities1())
            {
                var existente = context.Producto.Find(id);
                existente.estado = -1;
                existente.usuarioRegistro = usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static Producto get(int id)
        {
            using (var context = new LabComputadoras2BACEntities1())
            {
                return context.Producto.Find(id);
            }
        }

        public static List<Producto> listar()
        {
            using (var context = new LabComputadoras2BACEntities1())
            {
                return context.Producto.Where(x => x.estado != -1).ToList();
            }
        }

        public static List<paProductoListar_Result> listarPa(string parametro)
        {
            using (var context = new LabComputadoras2BACEntities1())
            {
                return context.paProductoListar(parametro).ToList();
            }
        }
    }
}
