SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_Batch_Rejection'
GO

CREATE PROCEDURE spu_ACT_Add_Batch_Rejection
@batchid int,
@cashlistitemreversereasonid int,
@rejectiondate datetime,
@pmuserid smallint,
@cashlistitemid int,
@pfinstalmentsid int,
@amount numeric
As

DECLARE @batchrejectionid INT

BEGIN
	INSERT INTO Batch_Rejection (
		batch_id,
		cashlistitem_reverse_reason_id,
		rejection_date,
		pmuser_id,
		cashlistitem_id,
		pfinstalments_id,
		amount)
	VALUES (
		@batchid,
		@cashlistitemreversereasonid,
		@rejectiondate,
		@pmuserid,
		@cashlistitemid,
		@pfinstalmentsid,
		@amount)
END
SELECT @batchrejectionid=@@IDENTITY
GO
