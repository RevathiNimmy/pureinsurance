SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON

GO

EXECUTE DDLDropProcedure 'spu_get_exception_records_for_reversal'

GO

create procedure spu_get_exception_records_for_reversal
@Premium_ReconciliationRS_ID int,
@batch_id int

As

BEGIN

select ACC_Reference_Number,Net_Amount_Paid, 
(select sum(Net_Amount_Paid)  from Account_Entry_RS ACC2 where ACC2.Premium_ReconciliationRS_ID = ACC1.Premium_ReconciliationRS_ID AND ACC2. batch_id = ACC1.BATCH_ID AND ACC2.Transaction_Status = 'E' ) , 
Comments 
from Account_Entry_RS ACC1
where Premium_ReconciliationRS_ID = @Premium_ReconciliationRS_ID
and batch_id = @batch_id
and Transaction_Status = 'E'


END

GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON

GO