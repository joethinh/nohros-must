/* Checks if the current database version is greater
 * than the version of this patch. To use this in the
 * SSMS the SQLCMD mode should be enabled.
 */
:on error exit

declare @continue bit,
  @objectname varchar(120),
  @objectversion int

set @objectname = 'restql' /* the name of the object related with the script */
set @objectversion = 1 /* the current object version */

exec @continue = nohros_updateversion @objectname=@objectname, @objectversion=@objectversion
if @continue != 1
begin /* version guard */
  raiserror(
    'The version of the database is greater than the version of this script. The batch will be stopped', 11, 1
  )
end /* version guard */
go

/**
 * Copyright (c) 2011 by Nohros Inc, All rights reserved.
 *
 * Script that create the database tables.
 *
 */
create table rql_query_method (
  query_method_id int not null,
  query_method varchar(30) not null
)

alter table rql_query_method
add constraint PK_rql_query_method
primary key (
  query_method_id
)

create unique nonclustered index IX_rql_query_method_natural
on rql_query_method (
  query_method
)

create table rql_query_type (
  query_type_id int identity(1,1) not null,
  query_type varchar(60) not null
)

alter table rql_query_type
add constraint PK_rql_query_type
primary key (
  query_type_id
)

create unique nonclustered index IX_rql_query_type_natural
on rql_query_type (
  query_type_id
)

create table rql_query (
  query_id int identity(1,1) not null,
  query_name varchar(800) not null,
  query varchar(8000) not null,
  query_method_id int not null,
  query_type_id int not null
  query_use_space_as_terminator bit,
  query_delimiter char(1),
)

alter table rql_query
add constraint PK_rql_query
primary key (
  query_id
)

alter table rql_query
add constraint FK_rql_query_query_method
foreign key (
  query_method_id
) references rql_query_method (
  query_method_id
)

alter table rql_query
add constraint FK_rql_query_query_type
foreign key (
  query_type_id
) references rql_query_type (
  query_type_id
)

alter table rql_query
add constraint DF_rql_query_delimiter
default (
  '$'
) for query_delimiter

alter table rql_query
add constraint DF_rql_query_use_space_as_terminator
default (
  0
) for query_use_space_as_terminator

create unique nonclustered index IX_rql_query_get
on rql_query (
  query_name
) include (
  query, query_method_id, query_type_id
)

create table rql_query_option_group (
  query_option_group_id int identity(1,1) not null,
  query_option_group_name varchar(120) not null
)

alter table rql_query_option_group
add constraint PK_rql_query_option_group
primary key (
  query_option_group_id
)

create unique nonclustered index IX_rql_query_option_group
on rql_query_option_group (
  query_option_group_name
)

create table rql_query_option (
  query_option_id int identity(1,1) not null,
  query_option_group_id int not null,
  option_name varchar(80) not null,
  option_value varchar(80) not null
)

alter table rql_query_option
add constraint PK_rql_query_option
primary key (
  query_option_id
)

alter table rql_query_option
add constraint FK_rql_query_option_group
foreign key (
  query_option_group_id
) references rql_query_option_group (
  query_option_group_id
)

create unique nonclustered index IX_rql_query_option_natural
on rql_query_option (
  query_option_group_id, option_name
) include (
  option_value
)

create table rql_query_option_crossref (
  query_id int not null,
  query_option_group_id int not null
)

alter table rql_query_option_crossref
add constraint FK_rql_query_option_crossref_query
foreign key (
  query_id
) references rql_query (
  query_id
)

alter table rql_query_option_crossref
add constraint FK_rql_query_option_crossref_group
foreign key (
  query_option_group_id
) references rql_query_option_group (
  query_option_group_id
)

create unique nonclustered index IX_rql_query_option_crossref
on rql_query_option_crossref (
  query_id, query_option_group_id
)