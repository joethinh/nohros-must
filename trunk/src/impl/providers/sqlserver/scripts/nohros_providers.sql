/* Checks if the current database version is greater
 * than the version of this patch. To use this in the
 * SSMS the SQLCMD mode should be enabled.
 */
:on error exit

declare @continue bit,
  @objectname varchar(120),
  @objectversion int

set @objectname = 'providers' /* the name of the object related with the script */
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
 */
create table nohros_provider (
  provider_id int identity(1,1) not null,
  provider_name varchar(120) not null,
  provider_type varchar(240) not null,
  provider_group varchar(120) not null
)