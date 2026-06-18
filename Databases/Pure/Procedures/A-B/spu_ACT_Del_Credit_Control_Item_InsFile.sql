SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Del_Credit_Control_Item_InsFile'
GO

CREATE PROCEDURE spu_ACT_Del_Credit_Control_Item_InsFile  
 @insurance_file_cnt INT,
 @nDeleteNonInstalment TINYINT = 0    
AS  

BEGIN    
IF @nDeleteNonInstalment =0
		UPDATE Credit_Control_Item  WITH (ROWLOCK)
		SET is_deleted  =1  
		WHERE insurance_file_cnt = @insurance_File_cnt
    ELSE If (@nDeleteNonInstalment=2)
	BEGIN
		DECLARE @insurance_folder_cnt int
		
		SELECT @insurance_folder_cnt = insurance_folder_cnt FROM insurance_file WITH (nolock) WHERE insurance_file_cnt =@insurance_file_cnt

	    UPDATE Credit_Control_Item  WITH (ROWLOCK)
		SET is_deleted  =1  
		WHERE insurance_file_cnt in( SELECT insurance_File_cnt  FROM insurance_file  WITH (nolock)  WHERE insurance_folder_cnt =@insurance_folder_cnt)

	END
	ELSE
		UPDATE Credit_Control_Item  WITH (ROWLOCK)
		SET is_deleted  =1  
		WHERE insurance_file_cnt = @insurance_File_cnt AND ISNULL(pfprem_finance_cnt ,0)=0 
END


GO
