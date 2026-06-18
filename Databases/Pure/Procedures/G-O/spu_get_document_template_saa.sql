SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF

GO

EXECUTE DDLDropProcedure 'spu_get_document_template_saa'

GO

CREATE PROCEDURE spu_get_document_template_saa
    @code char(20),
    @document_template_id int OUTPUT,
    @document_type_id int OUTPUT,
    @description char(255) OUTPUT,
    @effective_date datetime=NULL,
	@insurance_file_cnt integer= 0 ,
	@claim_cnt integer =0
	
AS  
  DECLARE @claim_doc_option int = 5030,
  @option_value int
IF @effective_date IS NOT NULL OR @effective_date ='1899-12-30' 
BEGIN
	IF @insurance_file_cnt <> 0 AND @claim_cnt = 0
		SELECT @effective_date = inception_date_tpi FROM Insurance_File WHERE Insurance_file_cnt = @insurance_file_cnt
	ELSE IF @claim_cnt <> 0 
		BEGIN 	 
		 SELECT @option_value = value from System_Options where option_number = @claim_doc_option
			IF @option_value = 0
				SELECT @effective_date = Reported_date FROM Claim WHERE claim_id= @claim_cnt 
			ELSE IF @option_value =2
				SELECT @effective_date = Loss_from_date  FROM Claim WHERE claim_id= @claim_cnt 
			ELSE  
				SET @effective_date = GETDATE() 
		END
	ELSE  
		SET @effective_date = GETDATE() 
		
END 

--Use @document_template_id in Variation to fetch effective template_id
IF ISNULL(@code, '') = '' AND ISNULL(@document_template_id, 0) <> 0
	Select @code = code From document_template 
		Where document_template_id = @document_template_id
			AND (LEFT( CONVERT(varchar,document_template.effective_date, 120), 10)  <= @effective_date
			or copy_of_original=1)

SET @document_template_id = 0
SET @document_type_id = 0
SET @description = ''

SELECT @document_template_id = document_template_id,
       @document_type_id = document_type_id,
       @description = description
FROM  document_template
WHERE (effective_date = ( SELECT MAX(effective_date )  
                          FROM document_template   
                          WHERE code = @code   
                          AND is_deleted = 0   
                          AND (LEFT( CONVERT(varchar,document_template.effective_date, 120), 10)  <= @effective_date))
						   or copy_of_original=1)
AND code = @code
AND is_deleted=0  
GO