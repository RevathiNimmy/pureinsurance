if exists (select * from sysobjects where id = object_id(N'[dbo].[spu_Act_UpdTransferTransDetail]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spu_Act_UpdTransferTransDetail]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

---------------------------------------------------------------------------------------------------------------------------
-- Name : Thinh Nguyen
--
-- Desc : change 'allocated' to 'allocatedx' after its been settle in InsurerPayment program
--
-- Note : 'allocated' records are over allocated amounts from PMU (data transfer)
----------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE spu_Act_UpdTransferTransDetail
	@TransDetailID int
AS

BEGIN
	UPDATE TransDetail SET spare = 'ALLOCATEDX'
	WHERE transdetail_id IN
	(
	SELECT td2.transdetail_id FROM TransDetail td INNER JOIN TransDetail td2 
	ON td.document_id = td2.document_id
	WHERE td.transdetail_id = @TransDetailID
	AND td2.account_id = td.account_id
	AND td2.spare = 'ALLOCATED'
	)
END

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO
