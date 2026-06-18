
EXECUTE DDLDropProcedure 'spu_get_recoveries_by_peril'
GO

CREATE PROCEDURE spu_get_recoveries_by_peril
@lPerilID INT

AS

BEGIN

		SELECT 
		r.recovery_id, 
		rt.recovery_type_id, 
		rt.is_salvage, 
		NULL 'this_reserve', 
		0 'is_updated',
		r.recovery_party_type_id, 
		r.recovery_party_cnt,
		r.Initial_reserve,
		r.revised_reserve

		FROM recovery_type rt

		Left Join recovery r ON r.recovery_type_id = rt.recovery_type_id AND r.claim_peril_id = @lPerilID

	WHERE is_deleted = 0  

END  
GO

