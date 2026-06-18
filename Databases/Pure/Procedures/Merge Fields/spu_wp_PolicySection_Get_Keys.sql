ddldropprocedure 'spu_wp_PolicySection_Get_Keys'
go

CREATE PROCEDURE spu_wp_PolicySection_Get_Keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

SELECT Insurance_section_id FROM insurance_cob_section WHERE Insurance_File_Cnt = @InsuranceFileCnt
go