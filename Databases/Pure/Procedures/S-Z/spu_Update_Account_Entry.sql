SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON

GO

EXECUTE DDLDropProcedure 'spu_Update_Account_Entry'

GO

create procedure spu_Update_Account_Entry
@Premium_ReconciliationRS_ID int,
@batch_id int,
@ACC_Reference_Number varchar(80),
@TransactionStatus VARCHAR(2),
@Allocation_ID int,
@Comments VARCHAR(255)

As

BEGIN

if  @ACC_Reference_Number IS NULL OR LEN(@ACC_Reference_Number) = 0 
BEGIN 
	Update Account_Entry_RS 
	set  Transaction_Status = @TransactionStatus, 
		 Allocation_ID = @Allocation_ID,
		 Comments = @Comments
	where Premium_ReconciliationRS_ID = @Premium_ReconciliationRS_ID
	and batch_id = @batch_id

END 

ELSE
BEGIN
Update Account_Entry_RS 
set  Transaction_Status = @TransactionStatus, 
	 Allocation_ID = @Allocation_ID,
	 Comments = @Comments
where Premium_ReconciliationRS_ID = @Premium_ReconciliationRS_ID
and batch_id = @batch_id
and ACC_Reference_Number = @ACC_Reference_Number
END


END

GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON

GO