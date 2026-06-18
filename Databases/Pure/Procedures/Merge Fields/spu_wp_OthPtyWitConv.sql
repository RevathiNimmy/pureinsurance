SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_OthPtyWitConv'
GO


CREATE PROCEDURE spu_wp_OthPtyWitConv
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT       pc.code Oth_Pty_CnvTyp,
         pc.fine_amt Oth_Pty_CnvFine,
         pc.status_code Oth_Pty_CnvStat,
         pc.conviction_date Oth_Pty_CnvDte,
         pc.description Oth_Pty_CnvDesc,
         pc.sentence_code Oth_Pty_SenTyp,
         pc.sentence_effective_date Oth_Pty_SenDte,
         pc.sentence_description Oth_Pty_SenDesc,
         pc.sentence_duration Oth_Pty_SenDur,
         pc.sentence_duration_qualifier Oth_Pty_SenUnit,
         pc.alcohol_measurement_method Oth_Pty_MotMeth,
         pc.alcohol_level Oth_Pty_MotLev,
         pc.driving_licence_penalty_pts Oth_Pty_MotPoints

FROM         party_conviction pc,
         claim_party_link cpl

WHERE        cpl.claim_id = @ClaimCnt
AND      cpl.party_cnt = @instance2
AND      pc.party_cnt = @instance2
AND      pc.party_conviction_id = @instance3
GO


