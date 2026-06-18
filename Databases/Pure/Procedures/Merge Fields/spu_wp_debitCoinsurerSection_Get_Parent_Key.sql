ddldropprocedure spu_wp_debitCoinsurerSection_Get_Parent_Key
go

CREATE PROCEDURE spu_wp_debitCoinsurerSection_Get_Parent_Key
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

SELECT @Instance2

