SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXEC DDLDropProcedure 'spu_SAM_Check_RetainedCoInsurer'
GO

create PROC spu_SAM_Check_RetainedCoInsurer (@partCnt AS INT)
AS
BEGIN
	SELECT ISNULL(pin.is_retained, 0) is_retained
	FROM Party p
	LEFT JOIN Party_insurer pin ON pin.party_cnt = p.party_cnt
	WHERE p.party_cnt = @partCnt
END

GO
