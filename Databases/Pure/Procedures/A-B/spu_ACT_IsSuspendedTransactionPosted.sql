SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_ACT_IsSuspendedTransactionPosted'
GO

CREATE PROCEDURE spu_ACT_IsSuspendedTransactionPosted  
    @SuspendedTransdetailId int,
    @ReturnValue   int  Output  
AS  
       
  Set @ReturnValue = 0
  
  IF EXISTS(select t.transdetail_id from transdetail t with(nolock) 
  --This is to check if two suspended transactions are not linked with one transaction(two Sub Agent case) Sumit K
  inner join suspended_accounts_transactions s on s.destination_account_id = t.account_id
            where t.insurance_ref = (select top 1 insurance_ref from transdetail with(nolock) 
                                   where transdetail_id  = @SuspendedTransdetailId)   
            and t.spare = (select top 1 b.document_ref from transdetail a  with(nolock) 
                         inner join Document b with(nolock) on a.document_id = b.document_id 
                         where transdetail_id  = @SuspendedTransdetailId AND b.document_ref NOT LIKE 'INC%')
                         and s.suspended_transdetail_id = @SuspendedTransdetailId)
	Set @ReturnValue = 1        
  Else
  	Set @ReturnValue = 0  
  	     
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO                         
