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
create table rql_querymethod (
  querymethodid int not null,
  querymethod varchar(30) not null
)

alter table rql_querymethod
add constraint PK_rql_querymethod
primary key (
  querymethodid
)

create unique nonclustered index IX_rql_querymethod_natural
on rql_querymethod (
  querymethod
)

create table rql_querytype (
  querytypeid int identity(1,1) not null,
  querytype varchar(60) not null
)

alter table rql_querytype
add constraint PK_rql_querytype
primary key (
  querytypeid
)

create unique nonclustered index IX_rql_querytype_natural
on rql_querytype (
  querytypeid
)

create table rql_query (
  queryid int identity(1,1) not null,
  queryname varchar(800) not null,
  query varchar(8000) not null,
  querymethodid int not null,
  querytypeid int not null
)

alter table rql_query
add constraint PK_rql_query
primary key (
  queryid
)

alter table rql_query
add constraint FK_rql_query_querymethod
foreign key (
  querymethodid
) references rql_querymethod (
  querymethodid
)

alter table rql_query
add constraint FK_rql_query_querytype
foreign key (
  querytypeid
) references rql_querytype (
  querytypeid
)

create unique nonclustered index IX_rql_query_get_query
on rql_query (
  queryname
) include (
  query, querymethodid, querytypeid
)

create table rql_queryoption (
  queryoptionid int identity(1,1) not null,
  queryid int not null,
  optionname varchar(80) not null,
  optionvalue varchar(80) not null
)

alter table rql_queryoption
add constraint PK_rql_queryoption
primary key (
  queryoptionid
)

alter table rql_queryoption
add constraint FK_rql_queryoption_query
foreign key (
  queryid
) references rql_query (
  queryid
)

create unique nonclustered index IX_rql_queryoption_natural
on rql_queryoption (
  queryid, optionname
) include (
  optionvalue
)