/* Checks if the current database version is greater
 * than the version of this patch. To use this in the
 * SSMS the SQLCMD mode should be enabled.
 */
:on error exit

declare @continue bit,
  @objectname varchar(120),
  @objectversion int

/* The name of the object related with the script */
set @objectname = 'rql_get_query'

/* The current object version */
set @objectversion = 1

exec @continue = nohros_updateversion
  @objectname = @objectname,
  @objectversion = @objectversion

if @continue != 1
begin /* version guard */
  raiserror(
    'The version of the database is greater than the version of this script. The batch will be stopped', 11, 1
  )
end /* version guard */

/* create and empty procedure with the name [@pbjectname]. So, we
 * can use the [alter proc [@objectname] statement]. This simulates
 * the behavior of the ALTER OR REPLACE statement that exists in other
 * datbases products. */
exec nohros_createproc @name = @objectname
go

/**
 * Copyright (c) 2011 by Nohros Inc, All rights reserved.
 *
 * Gets a information about the query whose name is @queryname
 *
 * @queryname varchar(800) The name of the query to get information
 */
alter proc rql_get_query (
  @queryname varchar(800)
)
as

select queryname,
  querytype,
  query
from rql_qeury
where queryname = @queryname