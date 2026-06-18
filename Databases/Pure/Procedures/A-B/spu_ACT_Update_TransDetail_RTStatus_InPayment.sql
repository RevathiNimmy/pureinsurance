SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Update_TransDetail_RTStatus_InPayment'
GO

CREATE PROCEDURE spu_ACT_Update_TransDetail_RTStatus_InPayment
	@Document_ID  integer,
	@Account_ID integer,
	@RT_Status  integer
AS

 -- Insurer Payments - these transdetails have just been paid - set RT status to 2 only if the
 -- TransDetail belongs to an insurer with no RT agtreement (CurRTStatus > 0)

   	UPDATE TransDetail set Risk_Transfer =@RT_Status where TransDetail_id in
	(Select TransDetail_id from TransDetail where Document_ID= @Document_ID and isnull(risk_transfer,0) >0 and Account_ID = @Account_ID)

GO
