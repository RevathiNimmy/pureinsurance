SET QUOTED_IDENTIFIER ON
GO
/*
    Created By		: George Harris
    Creation Date	: 4th April 2014
    Desacription	: Replace inline SQL appearing in Function DeletePolicyFromRenewal in bSIRRenewalProcess 

    Test Code		: EXEC spu_Get_Insurance_File_Details 2527

*/
Execute DDLDropProcedure 'spu_Get_Insurance_File_Details'
GO

CREATE PROCEDURE spu_Get_Insurance_File_Details
(
    @InsuranceFileCnt INT
)AS 


SET NOCOUNT ON;

BEGIN

    SELECT ifi2.insurance_file_cnt
		  ,rs.renewal_status_cnt
		  ,rs.renewal_insurance_file_cnt
		  ,ifi2.insurance_folder_cnt
		  ,ifi2.insured_cnt
		  ,ifi.insurance_ref 
    FROM Insurance_File ifi 
	   INNER JOIN Insurance_file ifi2 ON ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt
	   INNER JOIN Renewal_Status rs ON ifi2.insurance_file_cnt = rs.insurance_file_cnt
    WHERE ifi.insurance_file_cnt = @InsuranceFileCnt

END
   SET QUOTED_IDENTIFIER OFF
GO

