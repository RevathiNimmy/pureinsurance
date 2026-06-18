SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_OS_Loss_Reserves'
GO


CREATE PROCEDURE spu_Report_OS_Loss_Reserves
    @Treaty varchar(255)
AS
/**********************************************************************************************************************************
** Created by Kerry Butler
** 31/08/2001
** AUA Reports - Outstanding_Loss_Reserves.rpt
**
**********************************************************************************************************************************
**  The contents of field EXP are still to be clarified.
**
** 1.1      29/11/2001  JMK Change route for getting Treaty Information
**
***********************************************************************************************************************************/
/*
    Claim status id constants
    1 = Provisional Open Claim
    2 = Live Open Claim
    3 = Closed
    4 = ReOpen
    5 = ReClosed
*/

/*
-- test
declare @Treaty varchar (100)
select @Treaty = 'all'
--select @Treaty = 'Property Quota Share 01'
*/
CREATE TABLE #tempOSLossReserves
(
        TreatyCode varchar (10) NULL,
        TreatyDesc varchar (255) NULL,
        ReserveType varchar (255) NULL,
        ClaimNumber varchar (30) NULL,
        AgentCode varchar (10) NULL,
        InsuranceRef varchar (30) NULL,
        ClientCode varchar (10) NULL,
        ClientName varchar (60) NULL,
        LossFromDate datetime NULL,
        LossYear int NULL,
        ClaimDesc varchar (255) NULL,
        EXP varchar (10) NULL,
        CausationCode varchar (10) NULL,
        CatastropheCode varchar (10) NULL,
        InitialReserve money NULL,
        RevisedReserve money NULL,
        Payments money NULL
)
IF @Treaty = 'ALL'
BEGIN
    -- print 'get outstanding claims with Reserves '
    INSERT INTO #tempOSLossReserves
        SELECT t.code,
            t.description,
            (select description from reserve_type where Reserve_type_id = res.Reserve_type_id),
            c.claim_number,
            c.insurer_short_name,
            c.policy_number,
            c.client_short_name,
            c.client_name,
            c.loss_from_date,
            datepart(year,c.loss_from_date),
            LEFT(c.description,255),
            NULL,
            (SELECT code FROM Primary_Cause WHERE primary_cause_id = c.Primary_Cause_id),
            (SELECT code FROM Catastrophe_Code WHERE Catastrophe_code_id = c.Catastrophe_code_id),
            res.initial_reserve,
            res.revised_reserve,
            res.paid_to_date
        FROM claim c
        JOIN Claim_Peril cp     ON c.claim_id = cp.claim_id                                         -- Claim_Peril link
        JOIN Reserve res        ON cp.claim_peril_id = res.claim_peril_id
        JOIN RI_Arrangement ria ON ria.risk_cnt = c.risk_type_id                               -- Claim -> Treaty links
        JOIN RI_Arrangement_Line ril ON ril.ri_arrangement_id = ria.ri_arrangement_id
        JOIN Treaty t ON t.treaty_id = ril.treaty_id
        WHERE c.claim_status_id NOT IN (3,5)
END
ELSE
BEGIN
    INSERT INTO #tempOSLossReserves
        SELECT t.code,
            t.description,
            (select description from reserve_type where Reserve_type_id = res.Reserve_type_id),
            c.claim_number,
            c.insurer_short_name,
            c.policy_number,
            c.client_short_name,
            c.client_name,
            c.loss_from_date,
            datepart(year,c.loss_from_date),
            LEFT(c.description,255),
            NULL,
            (SELECT code FROM Primary_Cause WHERE primary_cause_id = c.Primary_Cause_id),
            (SELECT code FROM Catastrophe_Code WHERE Catastrophe_code_id = c.Catastrophe_code_id),
            res.initial_reserve,
            res.revised_reserve,
            res.paid_to_date
        FROM claim c
        JOIN Claim_Peril cp     ON c.claim_id = cp.claim_id                                         -- Claim_Peril link
        JOIN Reserve res        ON cp.claim_peril_id = res.claim_peril_id
        JOIN RI_Arrangement ria ON ria.risk_cnt = c.risk_type_id                               -- Claim -> Treaty links
        JOIN RI_Arrangement_Line ril ON ril.ri_arrangement_id = ria.ri_arrangement_id
        JOIN Treaty t ON t.treaty_id = ril.treaty_id
        WHERE c.claim_status_id NOT IN (3,5)
        AND t.description = @Treaty
END

SELECT * FROM #tempOSLossReserves
DROP TABLE #tempOSLossReserves

GO

