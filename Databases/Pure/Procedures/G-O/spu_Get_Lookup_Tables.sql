SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Get_Lookup_Tables'
GO


CREATE PROCEDURE spu_Get_Lookup_Tables
    @Lookup varchar(255)
AS


declare @SQLString varchar(255)
    select @SQLString = 'select claim_lookup_id, Lookup_tablename '
        + 'from claim_lookup where claim_lookup_id in ('
        + @Lookup + ')'
    exec (@SQLString)
GO


