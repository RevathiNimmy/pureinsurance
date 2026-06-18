SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_OthPtyDrvPrevAcc'
GO


CREATE PROCEDURE spu_wp_OthPtyDrvPrevAcc
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT       pa.description oth_pty_accdesc,
         pa.date oth_pty_accdte,
         pa.is_at_fault oth_pty_accatfault

FROM         previous_accidents pa,
         claim_party_link cpl

WHERE        cpl.claim_id = @ClaimCnt
AND      cpl.party_cnt = @instance2
AND      pa.party_cnt = @instance2
AND      pa.previous_accidents_id = @instance3
GO


