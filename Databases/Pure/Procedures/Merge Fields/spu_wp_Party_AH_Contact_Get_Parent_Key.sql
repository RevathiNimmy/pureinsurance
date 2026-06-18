SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_Party_AH_Contact_Get_Parent_Key'
GO


CREATE PROCEDURE spu_wp_Party_AH_Contact_Get_Parent_Key
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

IF @InsuranceFileCnt<>0
	SELECT account_handler_cnt FROM insurance_file WHERE insurance_file_cnt=@InsuranceFileCnt
ELSE
	SELECT 0
	
GO
