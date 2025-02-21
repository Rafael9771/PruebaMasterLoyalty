use DBMasterLoyalty
go

if (SELECT count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'articulo') is not null AND (SELECT count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'articulo') > 0 begin
	drop table articulo
end

create table articulo (
	arId int not null identity(1,1) constraint pk_articulo primary key,
	arGuid uniqueidentifier not null constraint const_articulo_guid default newid(),
	ar_codigo nvarchar(max) not null constraint const_articulo_codigo default '',
	ar_descripcion nvarchar(max) not null constraint const_articulo_descripcion default '',
	ar_precio DECIMAL(10,2) NOT NULL CONSTRAINT const_articulo_precio DEFAULT 0.00,
	ar_imagen nvarchar(max) not null constraint const_articulo_imagen default '',
	ar_stock int not null constraint const_artuculo_stock default 0,
	ar_dcreate datetime not null constraint const_articulo_dcreate default getdate(),
	ar_status tinyint not null constraint const_articulo_status default 1
)