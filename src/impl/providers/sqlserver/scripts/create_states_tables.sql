if exists (select name from sys.tables where name = 'nohros_state_bool')
begin
  if ((select count(*) from nohros_state_bool) = 0)
  begin
    drop table nohros_state_bool
  end
end
create table nohros_state_bool (
  state_name nvarchar(1024),
  [state] bit
)

if exists (select name from sys.tables where name = 'nohros_state_short')
begin
  if ((select count(*) from nohros_state_short) = 0)
  begin
    drop table nohros_state_short
  end
end
create table nohros_state_short (
  state_name nvarchar(1024),
  [state] smallint
)

if exists (select name from sys.tables where name = 'nohros_state_int')
begin
  if ((select count(*) from nohros_state_int) = 0)
  begin
    drop table nohros_state_int
  end
end
create table nohros_state_int (
  state_name nvarchar(1024),
  [state] int
)

if exists (select name from sys.tables where name = 'nohros_state_long')
begin
  if ((select count(*) from nohros_state_long) = 0)
  begin
    drop table nohros_state_long
  end
end
create table nohros_state_long (
  state_name nvarchar(1024),
  [state] bigint
)

if exists (select name from sys.tables where name = 'nohros_state_decimal')
begin
  if ((select count(*) from nohros_state_decimal) = 0)
  begin
    drop table nohros_state_decimal
  end
end
create table nohros_state_decimal (
  state_name nvarchar(1024),
  [state] decimal
)

if exists (select name from sys.tables where name = 'nohros_state_double')
begin
  if ((select count(*) from nohros_state_double) = 0)
  begin
    drop table nohros_state_double
  end
end
create table nohros_state_double (
  state_name nvarchar(1024),
  [state] float
)

if exists (select name from sys.tables where name = 'nohros_state_string')
begin
  if ((select count(*) from nohros_state_string) = 0)
  begin
    drop table nohros_state_string
  end
end
create table nohros_state_string (
  state_name nvarchar(1024),
  [state] nvarchar(1024)
)

if exists (select name from sys.tables where name = 'nohros_state_guid')
begin
  if ((select count(*) from nohros_state_guid) = 0)
  begin
    drop table nohros_state_guid
  end
end
create table nohros_state_guid (
  state_name nvarchar(1024),
  [state] uniqueidentifier
)

if exists (select name from sys.tables where name = 'nohros_state_date')
begin
  if ((select count(*) from nohros_state_date) = 0)
  begin
    drop table nohros_state_date
  end
end
create table nohros_state_date (
  state_name nvarchar(1024),
  [state] datetime
)