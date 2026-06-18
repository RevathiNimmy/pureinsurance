SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Update_Instalment_As_Posted'
GO

CREATE PROCEDURE spu_ACT_Update_Instalment_As_Posted
    @transdetailid int,
    @instalmentid int,
    @transactiondate datetime,
	@writeoffreasonid as int = Null,
	@writeofftransdetailid as int = Null 
AS
update pfinstalments
set
pftransaction_id = @transdetailid,
pfinstalments_result_id = (select pfinstalments_result_id from  pfinstalments_result where code = 'C'),
status = (select pfinstalments_status_id from pfinstalments_status where code = 'C'),
posteddate = @transactiondate,
batch_id =  Null,
write_off_reason_id = CASE WHEN @writeoffreasonid = 0 THEN Null ELSE @writeoffreasonid END,
write_off_transdetail_id = case when @writeofftransdetailid = 0 then Null else @writeofftransdetailid end
where
pfinstalments_id = @instalmentid
GO