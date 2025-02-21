use DBMasterLoyalty
go

if (SELECT count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'tienda_articulo') is not null AND (SELECT count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'tienda_articulo') > 0 begin
	drop table tienda_articulo
end

create table tienda_articulo (
	tiarId int not null identity(1,1) constraint pk_tienda_articulo primary key,
	tiarGuid uniqueidentifier not null constraint const_tienda_articulo_guid default newid(),
	tiar_tiId int not null constraint const_tienda_articulo_tiendaId default 0,
	tiar_arId int not null constraint const_tienda_articulo_articuloId default 0,
	tiar_dcreate datetime not null constraint const_tienda_articulo_dcreate default getdate(),
	tiar_status tinyint not null constraint const_tienda_articulo_status default 1
)