SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Generate_DocumentReference'
GO

CREATE PROCEDURE spu_ACT_Generate_DocumentReference
	@document_type_code VARCHAR(20),
	@user_id INT,
	@company_id INT = 1,
	@document_reference VARCHAR(20) OUTPUT


AS
BEGIN

DECLARE @number_rangeID INT 
DECLARE @number_pool_sel INT

SELECT @number_rangeID = ACTnumber_range_id FROM ACTNumber_Range WHERE code=@document_type_code
SELECT @number_rangeID= ISNULL(@number_rangeID,1)


--EXEC spe_ACTNumber_pool_sel
SELECT  @number_pool_sel= actnumber_pool_id
		FROM ACTNumber_Pool   
WHERE 	actnumber_range_id = @number_rangeID  
	  	AND company_id = @company_id  


IF @number_pool_sel=0
	BEGIN
	
	--EXEC spe_ACTNumber_pool_del
	DELETE FROM ACTNumber_Pool  
	WHERE 	actnumber_pool_id = @number_pool_sel 
		AND actnumber_range_id = @number_rangeID  
		AND company_id = @company_id  
	
	--EXEC spe_ACTnumber_upd
	UPDATE ACTNumber  
	    SET  
	    allocated_at=GetDate(),  
	user_id=@user_id  
	WHERE actnumber_id = @number_pool_sel AND actnumber_range_id = @number_rangeID  
	AND company_id = @company_id  
	
	
	END
Else
	EXEC spe_ACTnumber_add @number_pool_sel OUTPUT ,@number_rangeID,@user_id ,@company_id


	SELECT @document_reference=RTRIM(@document_type_code) + CONVERT(VARCHAR, REPLICATE ( 0 , 10-LEN(@number_pool_sel)))+ CONVERT(VARCHAR,@number_pool_sel)
END

GO
