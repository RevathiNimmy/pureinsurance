SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_DOC_Add_folder'
GO
CREATE PROCEDURE spu_DOC_Add_folder        
    @folder_num integer OUTPUT,        
    @folder_name varchar(255),        
    @parent_num integer,        
    @ex_code varchar(20),        
    @folder_level tinyint,        
    @access_level tinyint,        
    @password varchar(12),        
    @create_date datetime      
       
       
AS        
BEGIN        
	        
	DECLARE @Prev_Policy_No VARCHAR(100)         
	      
	DECLARE @AddDocfolderPerInsRef_Hidden_Opt INT 
    DECLARE @InsuranceFileCnt INT
    DECLARE @PolicyRefVer1 VARCHAR(100)
    DECLARE @tmp_parent_num INT
     
	SELECT @AddDocfolderPerInsRef_Hidden_Opt=Isnull(Option_Number,0) from Hidden_Options  where Option_Number=99    
	
    --PN 66472
	IF @AddDocfolderPerInsRef_Hidden_Opt > 0
	BEGIN
		--NOW CHECK IS AUTO NUMBERING IS VALID FOR POLICY
		SELECT @InsuranceFileCnt = ISNULL(insurance_file_cnt,0)  FROM Insurance_file INF(nolock)
               INNER JOIN product P ON P.product_id = INF.product_id
			   WHERE INF.insurance_ref = @folder_name
               AND ISNULL(P.Policy_Auto_numbering_id,0)>0
               AND ISNULL(INF.Policy_Version,0)>0
		
		IF @InsuranceFileCnt > 0 BEGIN
			SELECT @PolicyRefVer1 = insurance_ref FROM insurance_file 
            WHERE insurance_folder_cnt =  
           (SELECT insurance_folder_cnt FROM insurance_file 
            WHERE insurance_file_cnt = @InsuranceFileCnt)
            AND Policy_Version = 1
			
			SELECT @tmp_parent_num = ISNULL(folder_num,0) FROM doc_folder WHERE folder_name = @PolicyRefVer1
			
			IF @tmp_parent_num > 0 BEGIN
				SET @parent_num = @tmp_parent_num
				SET @folder_level=3			
			END
		END
	END
	   
	IF EXISTS(Select * From doc_Folder where Folder_Name = @folder_name and parent_num = @parent_num and Ex_code = @ex_code)
	Begin
		Select @folder_num = @parent_num
		RETURN
	End	
	INSERT INTO DOC_folder (
	folder_name,
	parent_num,
	ex_code,
	folder_level,
	access_level,
	password,
	create_date)
	VALUES (
	@folder_name,
	@parent_num,
	@ex_code,
	@folder_level,
	@access_level,
	@password,
	@create_date)

	SELECT @folder_num = @@IDENTITY
	IF @ex_code = 'GENERAL'
		BEGIN
		EXECUTE SPU_MERGE_GENERAL_DOCUMENT @folder_num,@parent_num
	END  
	 
END 
GO

