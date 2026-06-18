SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_sel_lookup_tables'
GO


CREATE PROCEDURE spu_sel_lookup_tables
AS

/*
SP name:    sp_sel_lookup_tables
Desc:       Used by Define Feilds Screen
        Selects all the Lookup table names
Author:     SK
Date:       06/09/2000
*/
SELECT claim_lookup_id, Lookup_tablename, description
FROM Claim_Lookup
GO


