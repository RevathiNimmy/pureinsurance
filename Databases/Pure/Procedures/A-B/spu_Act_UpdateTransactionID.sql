

/* Created by : Vidya Rangdale
Creation Date : 26/02/2014
Description   : This is used to update transaction details into Transdetail table
Test Code     : Exec spu_Act_UpdateTransactionID  
 */

SET QUOTED_IDENTIFIER ON
GO

Execute DDLDropProcedure 'spu_Act_UpdateTransactionID'
GO

CREATE PROCEDURE spu_Act_UpdateTransactionID  
	@nDocument_id  INT,  
	@nTransdetail_id  INT
AS  
BEGIN 

	UPDATE transdetail SET insurance_ref = NULL
	WHERE  transdetail_id = @nTransdetail_id  AND document_id = @nDocument_id

END
 
GO


