SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Outstanding_Recoveries'
GO


CREATE PROCEDURE spu_Report_Outstanding_Recoveries
                    @Treaty varchar(255)
AS
/**********************************************************************************************************************************
** Created by Kerry Butler
** 14/11/2001
** AUA Bespoke Reports - Outstanding_Recoveries_Report
**
**  
** 13/06/2002 SJP Underwriting Type (UWType) is selected by option number and branch (ensuring unique record retrieved)
**********************************************************************************************************************************
**
***********************************************************************************************************************************/
SET NOCOUNT ON
CREATE TABLE #tempOS_Recoveries
(
    Treaty      varchar(255)    NULL,
    ClaimNo     varchar(30) NULL,
    ClientName  varchar(60) NULL,
    LossDate    datetime    NULL,
    Causation   varchar(50) NULL,
    Catastrophe varchar(50) NULL,
    LossDesc    varchar(255)    NULL,
    PaidToDate  decimal(19,4)   NULL,
    OS_Recovery decimal(19,4)   NULL,
    UWType      char(1)     NULL)

-- Decide if underwriting or Agency
DECLARE  @UWType char(1)

--SJP (13/06/2002) UW_Type selected by branch_id  = 1 and option_number = 1 to ensure unique record
SELECT @UWtype = UW_type FROM hidden_options where branch_id = 1 and option_number = 1

IF isnull(@Treaty,'') = ''
    SELECT @Treaty = 'ALL'

IF @Treaty = 'ALL'
BEGIN
INSERT INTO #tempOS_Recoveries
-- PW090402 - make select distinct to avoid results being multiplied by nth degree
SELECT DISTINCT
    sd.ri_shortname,
    sf.loss_code,
        insurance_holder_shortname,
    loss_date,
    pc.description,
    cc.description,
    LEFT(c.description,255),
    r.received_to_date,
    (initial_reserve + revised_reserve - received_to_date),
    @UWType
FROM Stats_Folder sf

JOIN
        stats_detail sd ON sd.stats_folder_cnt = sf.stats_folder_cnt
JOIN
        claim c ON c.claim_number = sf.loss_code
JOIN
        primary_cause pc ON pc.primary_cause_id = c.primary_cause_id
LEFT OUTER JOIN
    catastrophe_code cc ON cc.catastrophe_code_id = c.catastrophe_code_id
JOIN
    claim_peril cp ON cp.claim_id = c.claim_id
JOIN
    recovery r ON r.claim_peril_id = cp.claim_peril_id

WHERE   (transaction_type_code LIKE 'C_S%' OR
        transaction_type_code LIKE 'C_R%')
    AND sd.stats_detail_type = 'TTY'  -- do we need COI?

END
ELSE
BEGIN
INSERT INTO #tempOS_Recoveries
-- PW090402 - make select distinct to avoid results being multiplied by nth degree
SELECT DISTINCT
    sd.ri_shortname,
    sf.loss_code,
        insurance_holder_shortname,
    loss_date,
    pc.description,
    cc.description,
    LEFT(c.description,255),
    r.received_to_date,
    (initial_reserve + revised_reserve - received_to_date),
    @UWType

FROM Stats_Folder sf

JOIN
        stats_detail sd ON sd.stats_folder_cnt = sf.stats_folder_cnt
JOIN
        claim c ON c.claim_number = sf.loss_code
JOIN
        primary_cause pc ON pc.primary_cause_id = c.primary_cause_id
LEFT OUTER JOIN
    catastrophe_code cc ON cc.catastrophe_code_id = c.catastrophe_code_id
JOIN
    claim_peril cp ON cp.claim_id = c.claim_id
JOIN
    recovery r ON r.claim_peril_id = cp.claim_peril_id

WHERE   (transaction_type_code LIKE 'C_S%' OR
        transaction_type_code LIKE 'C_R%')
    AND sd.stats_detail_type = 'TTY'  -- do we need COI?
    AND sd.ri_shortname = @Treaty
END

SET NOCOUNT OFF

SELECT * FROM #tempOS_Recoveries

DROP TABLE  #tempOS_Recoveries


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO
