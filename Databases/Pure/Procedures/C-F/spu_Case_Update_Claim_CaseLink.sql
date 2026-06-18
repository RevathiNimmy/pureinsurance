SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Case_Update_Claim_CaseLink'
GO


CREATE PROCEDURE spu_Case_Update_Claim_CaseLink 
    @claim_id INT,
    @base_case_id INT
    
AS
  
BEGIN  
  
    UPDATE claim SET  
        base_case_id = @base_case_id
    WHERE claim_number =(SELECT claim_number FROM claim WHERE claim_id= @claim_id)
END

GO



