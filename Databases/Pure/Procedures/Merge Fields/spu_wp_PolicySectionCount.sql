DDLDROPPROCEDURE 'spu_wp_PolicySectionCount'
GO

CREATE PROCEDURE spu_wp_PolicySectionCount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS
	SELECT Count(1) as how_many FROM insurance_cob_section WHERE insurance_file_cnt = @InsuranceFileCnt
GO