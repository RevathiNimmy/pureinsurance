EXECUTE DDLDropProcedure 'spu_wp_insurancefileassociates_get_parent_key'
GO


CREATE PROCEDURE spu_wp_insurancefileassociates_get_parent_key

    @PartyCnt INT,
	@InsuranceFileCnt INT,
	@RiskId INT,
	@ClaimCnt INT,
	@DocumentRef VARCHAR(25),
	@Instance1 INT,
	@Instance2 INT,
	@Instance3 INT
AS
	
SELECT @InsuranceFileCnt

GO


