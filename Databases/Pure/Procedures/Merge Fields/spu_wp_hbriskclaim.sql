SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_hbriskclaim'
GO

CREATE PROCEDURE spu_wp_hbriskclaim
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
DECLARE c_hbriskclaim CURSOR SCROLL KEYSET READ_ONLY FOR
    SELECT claim_date,
    claim_value
    FROM risk_claim
    WHERE insurance_file_cnt = @InsuranceFileCnt
    AND is_buildings = 1

OPEN c_hbriskclaim

FETCH ABSOLUTE @Instance1 FROM c_hbriskclaim

CLOSE c_hbriskclaim
DEALLOCATE c_hbriskclaim

GO

