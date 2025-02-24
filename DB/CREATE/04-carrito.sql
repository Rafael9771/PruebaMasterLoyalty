use DBMasterLoyalty
go

if (SELECT count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'carrito') is not null AND (SELECT count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'carrito') > 0 begin
	drop table carrito
end

create table carrito (
	caId int not null identity(1,1) constraint pk_carrito primary key,
	caGuid uniqueidentifier not null constraint const_carrito_guid default newid(),
	ca_clId int not null constraint const_carrito_cliente default 0,--FK con cliente
	ca_arTiId int NOT NULL CONSTRAINT const_carrito_articuloTienda DEFAULT 0,--FK con articulo en tienda
	ca_cantidad int not null constraint const_carrito_cantidad default 0,
	ca_dcreate datetime not null constraint const_carrito_dcreate default getdate(),
	ca_status tinyint not null constraint const_carrito_status default 1 --status 1 creado y activo, estatus 0 eliminado, status 2 carrito comprado
)