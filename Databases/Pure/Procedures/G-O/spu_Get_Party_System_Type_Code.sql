SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Get_Party_System_Type_Code'
GO

CREATE PROCEDURE spu_Get_Party_System_Type_Code
    	@PartyTypeCode char(10)
AS
BEGIN

	SELECT	pst.Code
	FROM		party_type pt
	INNER JOIN	party_system_type pst
	ON		pt.party_system_type_id = pst.party_system_type_id
	WHERE	pt.Code = @PartyTypeCode

END
