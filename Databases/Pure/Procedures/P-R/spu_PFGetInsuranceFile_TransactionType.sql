EXECUTE DDLDropProcedure 'spu_PFGetInsuranceFile_TransactionType'
GO

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_PFGetInsuranceFile_TransactionType
     @nInsurance_file_cnt INT
AS

DECLARE @nInsuranceFolderCnt INT
DECLARE @dtStartDate DATETIME

SELECT @nInsuranceFolderCnt=insurance_folder_cnt,@dtStartDate=cover_start_date FROM Insurance_File WHERE insurance_file_cnt=@nInsurance_file_cnt

IF Exists( SELECT ifi.insurance_file_cnt
FROM    Insurance_File ifi WITH(NOLOCK)
		JOIN Insurance_File_System IFS on ifi.insurance_file_cnt=IFS.insurance_file_cnt 
		INNER JOIN Transaction_Type tt WITH(NOLOCK) ON IFS.last_trans_type_id=tt.transaction_type_id
      	JOIN Insurance_File_Type IFTT on ifi.insurance_file_type_id=IFTT.insurance_file_type_id 
		WHERE	ifi.insurance_folder_cnt = @nInsuranceFolderCnt AND ifi.cover_start_date <= @dtStartDate AND tt.code='REN' )
BEGIN
	SELECT 'REN'
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
