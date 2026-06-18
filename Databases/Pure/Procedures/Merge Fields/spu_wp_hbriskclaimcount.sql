SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_hbriskclaimcount'
GO


CREATE PROCEDURE spu_wp_hbriskclaimcount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT  count(record_no) 'how_many'
    FROM    risk_claim
    WHERE   insurance_file_cnt = @InsuranceFileCnt
    AND is_buildings = 1
GO


