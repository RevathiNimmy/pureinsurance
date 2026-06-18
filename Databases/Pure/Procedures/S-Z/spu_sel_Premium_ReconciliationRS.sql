SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_sel_Premium_ReconciliationRS'
GO
  

CREATE PROCEDURE spu_sel_Premium_ReconciliationRS
    @Auto_ReconciliationRS_ID int
AS

Select TNP.Premium_ReconciliationRS_ID, Agent_Account_Ref, TNP.TotalNetPaid 
FROM
(Select 
Premium_ReconciliationRS_ID, sum(net_amount_paid) as TotalNetPaid
From Account_Entry_RS 
Where Account_Entry_RS.Transaction_Status = 'P'
Group by Premium_ReconciliationRS_ID) TNP
Inner Join Premium_ReconciliationRS PRRS ON TNP.Premium_ReconciliationRS_ID = PRRS.Premium_ReconciliationRS_ID
where Auto_ReconciliationRS_ID = @Auto_ReconciliationRS_ID



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO
