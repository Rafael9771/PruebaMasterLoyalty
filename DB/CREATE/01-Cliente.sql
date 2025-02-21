use DBMasterLoyalty
go

if (SELECT count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'cliente') is not null AND (SELECT count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'cliente') > 0 begin
	drop table cliente
end

create table cliente (
	ciId int not null identity(1,1) constraint pk_cliente primary key,
	ciGuid uniqueidentifier not null constraint const_cliente_guid default newid(),
	ci_nombre nvarchar(max) not null constraint const_cliente_nombre default '',
	ci_primer_apellido nvarchar(max) not null constraint const_cliente_primer_apellido default '',
	ci_segundo_apellido nvarchar(max) not null constraint const_cliente_segundo_apellido default '',
	ci_password nvarchar(max) not null constraint const_cliente_password default '',
	ci_correo nvarchar(max) not null constraint const_cliente_correo default '',
	ci_dcreate datetime not null constraint const_config_dcreate default getdate(),
	ci_status tinyint not null constraint const_config_status default 1
)