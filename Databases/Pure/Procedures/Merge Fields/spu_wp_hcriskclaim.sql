SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_hcriskclaim'
GO

CREATE PROCEDURE spu_wp_hcriskclaim
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

-- WARNING: This cursor is required for absolute positioning. The code
-- cannot be changed without changing the way Field Manager handles loops.
DECLARE c_hcriskclaim CURSOR SCROLL KEYSET READ_ONLY FOR
    SELECT claim_date,
    claim_value
    FROM risk_claim
    WHERE insurance_file_cnt = @InsuranceFileCnt
    AND is_buildings = 0

OPEN c_hcriskclaim

FETCH ABSOLUTE @Instance1 FROM c_hcriskclaim

CLOSE c_hcriskclaim
DEALLOCATE c_hcriskclaim

GO

