SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_apply_risk_coinsurance_to_peril'
GO


CREATE PROCEDURE spu_apply_risk_coinsurance_to_peril
    @insurance_file_cnt int,
    @risk_cnt int
AS

--*******************************************************************************************
-- Version      Author  Date            Desc
-- 1.00.0001        Tom 23 July 2001    apply coinsurance to peril for this risk
--
--*******************************************************************************************
DECLARE @Total_Coi_Share numeric(19,4),
    @Retained_Share numeric(19,4),
    @Total_Coi_Com_Share numeric(19,4)

-- TOTAL UP COINSURANCE SHARES EXCEPT RETAINED
SELECT  @Total_Coi_Share    = SUM(share_percent),
    @Total_Coi_Com_Share    = SUM(commission_percent)
FROM    coi_value
WHERE   insurance_file_cnt = @insurance_file_cnt
AND party_cnt NOT IN
(
    SELECT  party_cnt
    FROM    party
    WHERE   shortname = "RETAINED"
)

-- MAKE SURE ITS NOT NULL
IF @Total_Coi_Share IS NULL
    SELECT @Total_Coi_Share = 0

-- MAKE SURE ITS NOT NULL
IF @Total_Coi_Com_Share IS NULL
    SELECT @Total_Coi_Com_Share = 0

-- GET RETAINED SHARE
SELECT @Retained_Share = 100 - @Total_Coi_Share

UPDATE  peril
SET coinsured_this_premium = (this_premium * @Total_Coi_Share) / 100,
    coinsured_sum_insured = (sum_insured * @Total_Coi_Share) / 100,
    coinsured_commission = (sum_insured * @Total_Coi_Com_Share) / 100,
    retained_this_premium = (this_premium * @Retained_Share) / 100,
    retained_sum_insured = (sum_insured * @Retained_Share) / 100
WHERE   risk_cnt = @risk_cnt

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


