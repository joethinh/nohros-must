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
create table rql_query (
  queryid int identity(1,1) not null,
  queryname varchar(800) not null,
  query varchar(8000) not null
)

alter table rql_query
add constraint PK_rql_query
primary key (
  queryid
)

create unique nonclustered index IX_rql_query
on rql_query (
  queryname
) include (
  query
)