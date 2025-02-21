use DBMasterLoyalty
go

if (SELECT count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'compra') is not null AND (SELECT count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'compra') > 0 begin
	drop table compra
end

create table compra (
	coId int not null identity(1,1) constraint pk_compra primary key,
	coGuid uniqueidentifier not null constraint const_compra_guid default newid(),
	co_clId int not null constraint const_compra_cliente default 0,--FK con cliente
	co_tiId int not null constraint const_compra_tienda default 0,--FK con tienda
	co_arId int NOT NULL CONSTRAINT const_compra_articulo DEFAULT 0,--FK con articulo
	co_folio nvarchar(max) not null constraint conts_compra_folio default '',
	co_cantidad int not null constraint const_compra_cantidad default 0,
	co_dcreate datetime not null constraint const_compra_dcreate default getdate(),
	co_status tinyint not null constraint const_compra_status default 1
)