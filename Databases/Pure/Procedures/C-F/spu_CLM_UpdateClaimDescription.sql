SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


Execute DDLDropProcedure 'spu_CLM_UpdateClaimDescription'
GO

Create Procedure spu_CLM_UpdateClaimDescription
    @Claim_id bigint,
    @description varchar(1000)
As

UPDATE claim SET claim_version_description = @description WHERE claim_id = @claim_id
