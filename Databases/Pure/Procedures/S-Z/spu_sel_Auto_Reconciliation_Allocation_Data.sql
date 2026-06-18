
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_sel_Auto_Reconciliation_Allocation_Data'
GO
  

CREATE PROCEDURE spu_sel_Auto_Reconciliation_Allocation_Data
    @Premium_ReconciliationRS_ID int
AS

select Acc_Entry.ACC_Reference_Number,
	   Acc_Entry.Client_Name,
	   Acc_Entry.Policy_Number,
	   Acc_Entry.Gross_Amount_Due,
	   Acc_Entry.Commission_Due,
	   Acc_Entry.Net_Amount_Due,
	   Acc_Entry.Gross_Amount_Paid,
	   Acc_Entry.Commission_Paid,
	   Acc_Entry.Net_Amount_Paid,
	   (select sum(net_amount_paid) from Account_Entry_RS where  Account_Entry_RS.Premium_ReconciliationRS_ID  = Acc_Entry.Premium_ReconciliationRS_ID ) as TotalNetPaid
from Account_Entry_RS Acc_Entry 
where Acc_Entry.Transaction_Status = 'P'
and Acc_Entry.Premium_ReconciliationRS_ID = @Premium_ReconciliationRS_ID



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO
