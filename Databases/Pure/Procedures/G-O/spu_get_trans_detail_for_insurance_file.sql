SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_trans_detail_for_insurance_file'
GO

CREATE PROCEDURE spu_get_trans_detail_for_insurance_file    
    @insurance_file_cnt_1 INT,
	@insurance_file_cnt_2 INT
AS    
BEGIN  
	SELECT Distinct account_id 
    FROM transdetail AS td
    INNER JOIN document AS d 
        ON td.document_id = d.document_id
    INNER JOIN documenttype AS dt 
        ON d.documenttype_id = dt.documenttype_id
    WHERE d.insurance_file_cnt IN (@insurance_file_cnt_1, @insurance_file_cnt_2)
	group by account_id
      
   
END    
GO

    