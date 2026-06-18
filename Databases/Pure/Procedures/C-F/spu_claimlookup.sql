SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_claimlookup'
GO


CREATE PROCEDURE spu_claimlookup
    @claimlookupid int
AS


Declare @tablename varchar(30)
Declare @sql varchar(100)
Select @tablename=lookup_tablename from Claim_lookup where claim_lookup_id=@claimlookupid
Select @sql='Select ' + @tablename + '_id,description from ' + @tablename
exec(@sql)
GO


