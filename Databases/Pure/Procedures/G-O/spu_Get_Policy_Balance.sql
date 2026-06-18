SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_Get_Policy_Balance'
GO

CREATE PROCEDURE spu_Get_Policy_Balance    
    @lAccount_ID INTEGER,    
    @lInsurance_File_cnt INTEGER = NULL,  
    @daccounting_date DATETIME,  
    @sInsurance_ref VARCHAR(255) = NULL,
    @mOutstandingamount MONEY = 0 OUTPUT   
AS   
BEGIN

	DECLARE @lInsurance_Folder_cnt AS INTEGER
	
	IF LEN(RTRIM(LTRIM(@sInsurance_ref))) > 3
		SELECT @lInsurance_Folder_cnt = Insurance_Folder_cnt 
		FROM Insurance_Folder 
		WHERE RTRIM(LTRIM(code)) =  @sInsurance_ref
	
	IF ISNULL(@lInsurance_Folder_cnt,0) <> 0   
	    SELECT @mOutStandingAmount = ISNULL(Sum(td.outstanding_account_amount),0)   
	    FROM transdetail td JOIN Document Docs   
	        ON td.document_id = docs.document_id   
	    JOIN Insurance_file Ifs ON    
	        Docs.Insurance_File_Cnt = Ifs.Insurance_file_cnt   
	    WHERE  
	        Ifs.Insurance_folder_cnt = @lInsurance_Folder_Cnt  
	        AND account_id = @lAccount_ID   
	        AND td.accounting_date <= @dAccounting_Date
	  
      SELECT @lAccount_ID AS Account_ID,  
	    @mOutStandingAmount AS OutStanding_Amount
END

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

