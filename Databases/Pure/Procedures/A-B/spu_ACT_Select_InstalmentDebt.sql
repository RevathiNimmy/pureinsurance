-- Created Kevin Grandison - 07/07/03

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_InstalmentDebt'
GO


CREATE  PROCEDURE spu_ACT_Select_InstalmentDebt
    @account_id int,
    @insurance_file_cnt int = NULL
AS

DECLARE @InstalmentDebtAmount INT

SELECT @InstalmentDebtAmount= SUM(Amount)  
FROM PFInstalments I
INNER JOIN PFPremiumFinance P
ON P.pfprem_finance_cnt=I.pfprem_finance_cnt
AND P.pfprem_finance_version=I.pfprem_finance_version
INNER JOIN Account A ON A.account_key=P.clientid
WHERE DueDate < GetDate()
AND      ((Status<> 3 AND Status <> 2) OR Status=5)

AND      P.StatusInd = '040'
AND      A.account_id=@account_id
AND     (P.insurance_file_cnt=@insurance_file_cnt OR @insurance_file_cnt IS NULL)


RETURN ISNULL(@InstalmentDebtAmount,0)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

