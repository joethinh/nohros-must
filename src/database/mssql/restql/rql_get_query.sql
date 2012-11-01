/* Checks if the current database version is greater
 * than the version of this patch. To use this in the
 * SSMS the SQLCMD mode should be enabled.
 */
:on error exit

declare @continue bit,
  @objectname varchar(120),
  @objectversion int

/* The name of the object related with the script */
set @objectname = 'rql_query_get'

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
alter proc rql_query_get (
  @name varchar(800)
)
as

declare @query_id int

select @query_id = query_id
from rql_query q
where query_name = @name

select query_name,
  query,
  query_type,
  query_method_id as query_method
from rql_query q
  inner join rql_query_type qt on qt.query_type_id = q.query_type_id
where query_id = @query_id

select option_name
  ,option_value
from rql_query_option
where query_id = @query_id