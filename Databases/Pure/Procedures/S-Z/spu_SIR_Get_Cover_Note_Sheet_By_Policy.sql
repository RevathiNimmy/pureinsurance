SET QUOTED_IDENTIFIER ON    
Go
SET ANSI_NULLS ON  
GO
--**********************************************************************************************
-- Author : Prabodh Mishra
-- History: 23/08/2007 REL001 - Created
--**********************************************************************************************

EXECUTE DDLDropProcedure 'spu_SIR_Get_Cover_Note_Sheet_By_Policy'
GO

CREATE PROCEDURE spu_SIR_Get_Cover_Note_Sheet_By_Policy 
	@insurance_file_cnt int
AS
Declare
	@policy_ref varchar(50)
BEGIN

SELECT @policy_ref = insurance_ref From insurance_file 
Where insurance_file_cnt = @insurance_file_cnt

	--Check if a sheet is attached
	SELECT CNB.Book_Number,
		CNS.Cover_Sheet_Number, 
		CNSS.Code,
		CNS.is_deleted
	From Cover_Note_Sheet CNS
		INNER JOIN Cover_Note_Sheet_Status CNSS 
			ON CNSS.Cover_Note_Sheet_Status_Id = CNS.Cover_Note_Sheet_Status_Id
		INNER JOIN Cover_Note_Book CNB
			ON CNB.Cover_Note_Book_Id = CNS.Cover_Note_Book_Id
		INNER JOIN Insurance_File IFI on IFI.insurance_file_cnt = CNS.insurance_file_cnt
		Where CNS.insurance_file_cnt In 
		(Select insurance_file_cnt From insurance_file Where insurance_ref like @policy_ref) 



END

SET QUOTED_IDENTIFIER OFF    
Go
SET ANSI_NULLS OFF  
GO

