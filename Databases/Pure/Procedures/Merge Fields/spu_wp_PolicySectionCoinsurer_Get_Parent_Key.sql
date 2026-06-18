ddldropprocedure 'spu_wp_PolicySectionCoinsurer_Get_Parent_Key'
go

CREATE PROCEDURE spu_wp_PolicySectionCoinsurer_Get_Parent_Key
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

SELECT @InsuranceFileCnt

