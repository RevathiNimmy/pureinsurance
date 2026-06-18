SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_get_client_for_sub_agent'
GO

CREATE PROCEDURE spu_ACT_get_client_for_sub_agent
    @transdetail_id INT
AS

--Get the client details we are going to post against
SELECT t2.transdetail_id, t2.amount
FROM transdetail t1,
     transdetail t2
WHERE t2.document_id = t1.document_id
AND t1.account_id = t2.account_id
AND t1.transdetail_id = @transdetail_id
AND t1.amount = (t2.amount * -1)
