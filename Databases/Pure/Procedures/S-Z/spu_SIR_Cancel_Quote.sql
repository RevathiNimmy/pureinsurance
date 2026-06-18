SET QUOTED_IDENTIFIER OFF 
GO

EXECUTE DDLDropProcedure 'spu_SIR_Cancel_Quote'
GO
/*
	Created By		: Krishan Kumar Gorav
	Creation Date	: 05 Aug 2015
	Parameters		: @nInsuranceFileCnt - Insurance file cnt
	Description		: To change the status of quote to be cancelled for a given insurance file
	Test Code		: EXEC spu_SIR_Cancel_Quote 123456
*/
CREATE PROCEDURE spu_SIR_Cancel_Quote
    @nInsuranceFileCnt int  
AS  
BEGIN  
    DECLARE @nInsuranceFileStatusIdForCancelled INT
	SELECT @nInsuranceFileStatusIdForCancelled = insurance_file_status_id FROM insurance_file_status where code='CAN'

	UPDATE Insurance_File  
	SET insurance_file_status_id = @nInsuranceFileStatusIdForCancelled
	WHERE insurance_file_cnt = @nInsuranceFileCnt OR base_insurance_file_cnt= @nInsuranceFileCnt
END
SET QUOTED_IDENTIFIER OFF 
GO
