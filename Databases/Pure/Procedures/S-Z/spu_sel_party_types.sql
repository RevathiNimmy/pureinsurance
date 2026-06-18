SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_sel_party_types'
GO


CREATE PROCEDURE spu_sel_party_types
AS

/*
SP name:    sp_sel_party_types
Desc:       Used by Define Feilds Screen
        Selects all the Party Types
Author:     SK
Date:       06/09/2000
*/
SELECT Claim_Party_type_id, Description
FROM Claim_Party_type
GO


