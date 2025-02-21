use DBMasterLoyalty
go

if (SELECT count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'tienda') is not null AND (SELECT count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'tenda') > 0 begin
	drop table tienda
end

create table tienda (
	tiId int not null identity(1,1) constraint pk_tienda primary key,
	tiGuid uniqueidentifier not null constraint const_tienda_guid default newid(),
	ti_sucursal nvarchar(max) not null constraint const_tienda_sucursal default '',
	ti_direccion nvarchar(max) not null constraint const_tienda_direccion default '',
	ti_dcreate datetime not null constraint const_tienda_dcreate default getdate(),
	ti_status tinyint not null constraint const_tienda_status default 1
)