SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Update_TransDetail_RTStatus'
GO

CREATE PROCEDURE spu_ACT_Update_TransDetail_RTStatus
	@TransDetail_ID  integer,
	@RT_Status  integer,
	@risk_transfer_reconciliation_date datetime=null

AS
 
	Declare @Document_id as Integer
	Declare @Account_id  as Integer
	
	select @Document_id=document_id from transdetail where  TransDetail_ID=@TransDetail_ID
	select @Account_id=Account_id from transdetail where  TransDetail_ID=@TransDetail_ID
 
 -- Insurer Payments - these transdetails have just been paid - set RT status to 2 only if the 
 -- TransDetail belongs to an insurer with no RT agtreement (CurRTStatus > 0)
 
	 if @RT_Status = 2 
  		begin
			UPDATE TransDetail set Risk_Transfer =@RT_Status where TransDetail_id in 
			(Select TransDetail_id from TransDetail where Document_ID= @Document_ID and isnull(risk_transfer,0) >0 and Account_ID = @Account_ID)
 	 	end 

 -- Risk Transfer - Marking - these transaction have been marked
 	if @RT_Status = 3 
 	 	begin
			UPDATE TransDetail set Risk_Transfer =@RT_Status where TransDetail_id in 
			(Select TransDetail_id from TransDetail where Document_ID= @Document_ID and isnull(risk_transfer,0) >1 and Account_ID = @Account_ID)
		end 

 -- Risk Transfer reconciliation - these transdetails have been recieved by insurer - set RT status 
 -- to 4 only if the TransDetail belongs to an insurer with no RT agtreement (CurRTStatus > 0) and has been PAID (CurRTStatus > 1) and has been marked >2
	 if @RT_Status = 4
		begin
			 UPDATE TransDetail set Risk_Transfer =@RT_Status,risk_transfer_reconciliation_date=@risk_transfer_reconciliation_date where TransDetail_id in
			 (Select TransDetail_id from TransDetail where Document_ID= @Document_ID and isnull(risk_transfer,0) >2 and Account_ID = @Account_ID)
  		end
GO
