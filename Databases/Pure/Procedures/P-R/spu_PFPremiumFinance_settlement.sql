EXECUTE DDLDropProcedure 'spu_PFPremiumFinance_settlement'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

-- RAW 05/11/2003 : CQ2976 : added instalment date parameters 
 -- RAW 13/11/2003 : CQ1765 : added @LastPaidInstalmentDate parameter  
  
Create PROCEDURE spu_PFPremiumFinance_settlement  
    @pfprem_finance_cnt INT,  
    @pfprem_finance_version INT,  
    @SettleAmount NUMERIC(19,4) OUTPUT,  
    @RefundFee NUMERIC(19,4) OUTPUT,  
    @RefundTax NUMERIC(19,4) OUTPUT,  
    @NextInstalmentDate DATETIME = NULL OUTPUT,  
    @NextInstalmentDatePlus1 DATETIME = NULL OUTPUT,  
    @LastInstalmentDate DATETIME = NULL OUTPUT,  
    @LastPaidInstalmentDate DATETIME = NULL OUTPUT, 
	@CostOfProtection NUMERIC(19,4) =0  OUTPUT
AS

DECLARE @MaxCollectedPFInstalmentsId INT
SELECT @MaxCollectedPFInstalmentsId = ISNULL(MAX(pfinstalments_id),0)
FROM  
    PFInstalments 
JOIN  
 PFPremiumFinance ON PFPremiumFinance.pfprem_finance_cnt=@pfprem_finance_cnt  
 AND PFPremiumFinance.pfprem_finance_version=@pfprem_finance_version  
WHERE  
    PFInstalments.pfprem_finance_cnt = PFPremiumFinance.pfprem_finance_cnt  
AND PFInstalments.pfprem_finance_version = PFPremiumFinance.pfprem_finance_version  
AND InstalmentNumber > 0  
AND (PFInstalments.Status =3)  
AND PFPremiumFinance.statusind IN ('040','999','140')  

SELECT  
    @NextInstalmentDate = MIN(PFInstalments.DueDate)
FROM  
    PFInstalments  
JOIN  
 PFPremiumFinance ON PFPremiumFinance.pfprem_finance_cnt=@pfprem_finance_cnt  
 AND PFPremiumFinance.pfprem_finance_version=@pfprem_finance_version  
WHERE  
    PFInstalments.pfprem_finance_cnt = PFPremiumFinance.pfprem_finance_cnt  
AND PFInstalments.pfprem_finance_version = PFPremiumFinance.pfprem_finance_version  
AND InstalmentNumber > 0  
AND (PFInstalments.Status <>3)  AND pfinstalments_id > @MaxCollectedPFInstalmentsId
AND PFPremiumFinance.statusind IN ('040','999','140')
  
--PSL 18/03/2003 added Refund Tax amount  
-- RAW 05/11/2003 : CQ2976 : added instalment dates  
SELECT
    @SettleAmount = SUM(PFInstalments.Amount) - SUM(PFInstalments.Fee) - SUM(PFInstalments.Tax),
    @RefundFee = SUM(PFInstalments.Fee),
    @RefundTax = SUM(PFInstalments.Tax),
    @LastInstalmentDate = MAX(PFInstalments.DueDate)
FROM
    PFInstalments
JOIN
 PFPremiumFinance ON PFPremiumFinance.pfprem_finance_cnt=@pfprem_finance_cnt
 AND PFPremiumFinance.pfprem_finance_version=@pfprem_finance_version
WHERE
    PFInstalments.pfprem_finance_cnt = PFPremiumFinance.pfprem_finance_cnt
AND PFInstalments.pfprem_finance_version = PFPremiumFinance.pfprem_finance_version
AND InstalmentNumber > 0
AND (PFInstalments.Status <>3)
AND PFPremiumFinance.statusind IN ('040','140')

SELECT @CostOfProtection=PFPremiumFinance.CostOfProtection
FROM PFPremiumFinance 
WHERE pfprem_finance_cnt = @pfprem_finance_cnt
AND pfprem_finance_version=@pfprem_finance_version

-- RAW 05/11/2003 : CQ2976 : added
SELECT
    @NextInstalmentDatePlus1 = MIN(PFInstalments.DueDate)
FROM
    PFInstalments
JOIN
 PFPremiumFinance ON PFPremiumFinance.pfprem_finance_cnt=@pfprem_finance_cnt
 AND PFPremiumFinance.pfprem_finance_version=@pfprem_finance_version
WHERE
    PFInstalments.pfprem_finance_cnt = PFPremiumFinance.pfprem_finance_cnt
AND PFInstalments.pfprem_finance_version = PFPremiumFinance.pfprem_finance_version
AND InstalmentNumber > 0
--AND (PFInstalments.Status = 3)
AND PFInstalments.DueDate > @NextInstalmentDate
AND PFPremiumFinance.statusind IN ('040','140')

-- RAW 05/11/2003 : CQ2976 : end

-- RAW 13/11/2003 : CQ1765 : added
SELECT
    @LastPaidInstalmentDate = MAX(PFInstalments.DueDate)
FROM
    PFInstalments
JOIN
 PFPremiumFinance ON PFPremiumFinance.pfprem_finance_cnt=@pfprem_finance_cnt
 AND PFPremiumFinance.pfprem_finance_version=@pfprem_finance_version
WHERE
    PFInstalments.pfprem_finance_cnt = PFInstalments.pfprem_finance_cnt
AND PFInstalments.pfprem_finance_version = PFInstalments.pfprem_finance_version
AND InstalmentNumber > 0
AND NOT (PFInstalments.Status <> 3)
AND PFPremiumFinance.statusind IN ('040','140')

If @NextInstalmentDatePlus1 IS NULL
   SELECT @NextInstalmentDatePlus1=@NextInstalmentDate
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
