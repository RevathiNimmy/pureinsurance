SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GetCurrentReserveFromRI_SFU'
GO

CREATE PROCEDURE spu_GetCurrentReserveFromRI_SFU
	@ClaimID int
AS
BEGIN
	SELECT ISNULL(reserve-this_reserve,0)
	FROM claim_ri_arrangement_line
	WHERE claim_id=@ClaimID
END

GO				
