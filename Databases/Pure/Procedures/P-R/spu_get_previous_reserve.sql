EXECUTE DDLDropProcedure 'spu_get_previous_reserve'
GO
CREATE PROCEDURE spu_get_previous_reserve
    @nReserveID INT,
	@crPreviousReserve currency output
AS

BEGIN

	SELECT @crPreviousReserve= ISNULL(SUM(RES.this_revision),0) from reserve RES

	JOIN claim_peril CPRL

	ON RES.claim_Peril_id=CPRL.Claim_Peril_id

	JOIN Claim CLM

	ON CPRL.Claim_id=CLM.Claim_id

	JOIN reserve CRES

	On CRES.base_reserve_id=RES.base_reserve_id

	WHERE CLM.is_dirty=0
	AND RES.base_reserve_id<>CRES.Reserve_id
	AND CRES.reserve_id =@nReserveID


END  
Go

