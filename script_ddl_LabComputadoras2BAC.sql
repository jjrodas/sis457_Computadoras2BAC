-- Creación de la base de datos:
CREATE DATABASE LabComputadoras2BAC

-- Cración del Login y contraseña para el usuario de la base de datos:
CREATE LOGIN usrLabComputadoras2BAC WITH PASSWORD='C0MPUMUND0',
  DEFAULT_DATABASE = LabComputadoras2BAC,
  CHECK_EXPIRATION = OFF,
  CHECK_POLICY = ON
GO

USE LabComputadoras2BAC
-- Creación del usuario en base al login y poner la base de datos como propiedad del usuario creado:
CREATE USER usrLabComputadoras2BAC FOR LOGIN usrLabComputadoras2BAC
GO
ALTER ROLE db_owner ADD MEMBER usrLabComputadoras2BAC
GO

DROP TABLE VentaDetalle;
DROP TABLE Venta;
DROP TABLE Usuario;
DROP TABLE Empleado;
DROP TABLE Cliente;
DROP TABLE Producto;
DROP TABLE Categoria;
-- Creación de las tablas:
CREATE TABLE Categoria (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  nombre VARCHAR(30) NOT NULL,
  descripcion VARCHAR(150) NOT NULL,
  usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
  fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
  estado SMALLINT NOT NULL DEFAULT 1
);
CREATE TABLE Producto (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  idCategoria INT NOT NULL,
  codigo VARCHAR(10) NOT NULL,
  descripcion VARCHAR(120) NOT NULL,
  marca VARCHAR(30) NOT NULL,
  precioVenta DECIMAL NOT NULL CHECK(precioVenta > 0),
  usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
  fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
  estado SMALLINT NOT NULL DEFAULT 1,
  CONSTRAINT fk_Producto_Categoria FOREIGN KEY(idCategoria) REFERENCES Categoria(id)
);
CREATE TABLE Cliente (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  cedulaIdentidad VARCHAR(15) NOT NULL,
  nombres VARCHAR(40) NOT NULL,
  apellidos VARCHAR(40) NOT NULL,
  celular INT NOT NULL,
  usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
  fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
  estado SMALLINT NOT NULL DEFAULT 1
);
CREATE TABLE Empleado (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  cedulaIdentidad VARCHAR(15) NOT NULL,
  nombres VARCHAR(40) NOT NULL,
  apellidos VARCHAR(40) NOT NULL,
  direccion VARCHAR(200) NOT NULL,
  celular INT NOT NULL,
  cargo VARCHAR(30) NOT NULL,
  usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
  fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
  estado SMALLINT NOT NULL DEFAULT 1
);
CREATE TABLE Usuario (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  idEmpleado INT NOT NULL,
  usuario VARCHAR(15) NOT NULL,
  clave VARCHAR(30) NOT NULL,
  usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
  fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
  estado SMALLINT NOT NULL DEFAULT 1,
  CONSTRAINT fk_Usuario_Empleado FOREIGN KEY(idEmpleado) REFERENCES Empleado(id)
);
CREATE TABLE Venta (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  idUsuario INT NOT NULL,
  idCliente INT NOT NULL,
  fecha DATE NOT NULL DEFAULT GETDATE(),
  usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
  fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
  estado SMALLINT NOT NULL DEFAULT 1,
  CONSTRAINT fk_Venta_Usuario FOREIGN KEY(idUsuario) REFERENCES Usuario(id),
  CONSTRAINT fk_Venta_CLiente FOREIGN KEY(idCliente) REFERENCES Cliente(id)
);
CREATE TABLE VentaDetalle (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  idProducto INT NOT NULL,
  idVenta INT NOT NULL,
  precioUnitario DECIMAL NOT NULL,
  cantidad INT NOT NULL CHECK(cantidad > 0),
  total DECIMAL NOT NULL,
  usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
  fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
  estado SMALLINT NOT NULL DEFAULT 1,
  CONSTRAINT fk_VentaDetalle_Producto FOREIGN KEY(idProducto) REFERENCES Producto(id),
  CONSTRAINT fk_VentaDetalle_Venta FOREIGN KEY(idVenta) REFERENCES Venta(id)
);

-- Creación de los procedimientos almacenados:
CREATE PROC paCategoriaListar @parametro VARCHAR(50)
AS
  SELECT id,nombre,descripcion,usuarioRegistro,fechaRegistro,estado
  FROM Categoria
  WHERE estado<>-1 AND descripcion LIKE '%'+REPLACE(@parametro,' ','%')+'%';

CREATE PROC paProductoListar @parametro VARCHAR(50)
AS
  SELECT id,idCategoria,codigo,descripcion,marca,precioVenta,usuarioRegistro,fechaRegistro,estado
  FROM Producto
  WHERE estado<>-1 AND descripcion LIKE '%'+REPLACE(@parametro,' ','%')+'%';

CREATE PROC paClienteListar @parametro VARCHAR(50)
AS
  SELECT id,cedulaIdentidad,nombres,apellidos,celular,usuarioRegistro,fechaRegistro,estado
  FROM Cliente
  WHERE estado<>-1 AND nombres LIKE '%'+REPLACE(@parametro,' ','%')+'%';

CREATE PROC paEmpleadoListar @parametro VARCHAR(50)
AS
  SELECT id,cedulaIdentidad,nombres,apellidos,direccion,celular,cargo,usuarioRegistro,fechaRegistro,estado
  FROM Empleado
  WHERE estado<>-1 AND nombres LIKE '%'+REPLACE(@parametro,' ','%')+'%';

CREATE PROC paUsuarioListar @parametro VARCHAR(50)
AS
  SELECT id,idEmpleado,usuario,clave,usuarioRegistro,fechaRegistro,estado
  FROM Usuario
  WHERE estado<>-1 AND usuario LIKE '%'+REPLACE(@parametro,' ','%')+'%';

-- Procedimientos almacenados aún no creados

CREATE PROC paVentaListar @parametro VARCHAR(50)
AS
  SELECT id,idUsuario,idCliente,fecha,usuarioRegistro,fechaRegistro,estado
  FROM Venta
  WHERE estado<>-1 AND descripcion LIKE '%'+REPLACE(@parametro,' ','%')+'%';

CREATE PROC paVentaDetalleListar @parametro VARCHAR(50)
AS
  SELECT id,idProducto,idVenta,precioUnitario,cantidad,precioVenta,total,usuarioRegistro,fechaRegistro,estado
  FROM VentaDetalle
  WHERE estado<>-1 AND descripcion LIKE '%'+REPLACE(@parametro,' ','%')+'%';