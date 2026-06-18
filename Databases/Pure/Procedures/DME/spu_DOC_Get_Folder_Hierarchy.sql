/* stored procedure to fetch all the folder details in array  */
/* Sirius Process No 	   : 189  							  */
/* NRMA Project Process No : 231							  */
/* Desc : This stoerd procedure give all the folder hierarchy */
/*			for the supplied document number, or folder_num   */
/*			based on whether it is a document or folder	      */

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_DOC_Get_Folder_Hierarchy'
GO

CREATE PROCEDURE spu_DOC_Get_Folder_Hierarchy
    @param_doc_folder_num 	int,
    @param_doc_or_folder	CHAR(10)
AS
BEGIN

	DECLARE	@folder_num	INT
	DECLARE	@folder_name	VARCHAR(70)
	DECLARE	@parent_num	INT
	DECLARE	@ex_code	VARCHAR(20) 
	DECLARE	@folder_level	TINYINT 
	DECLARE	@access_level	TINYINT 
	DECLARE	@password	VARCHAR(12) 
	DECLARE	@create_date	DATETIME	
	DECLARE @tempfolder_num INT
	
	/* Drop the temporary Table */	
	IF OBJECT_ID('tempdb..#tempDocFolderDetails') IS NOT NULL 
	    Drop Table #tempDocFolderDetails
		
	/* Create a Temporary Table */
	CREATE TABLE #tempDocFolderDetails    
    	(	
		folder_num	INT,
		folder_name	VARCHAR(70),
		parent_num	INT,
		ex_code		VARCHAR(20) NULL,
		folder_level	TINYINT NULL,
		access_level	TINYINT NULL,
		password	VARCHAR(12) NULL,
		create_date	DATETIME
	)	

	SELECT @tempfolder_num = 
      	   CASE 
      		WHEN RTRIM(@param_doc_or_folder)  = 'DOCUMENT' THEN ( SELECT folder_num  FROM   doc_document  WHERE  doc_num = @param_doc_folder_num )
		WHEN RTRIM(@param_doc_or_folder)  = 'FOLDER'   THEN  @param_doc_folder_num			
	END

	WHILE  @tempfolder_num > -1
	BEGIN
	
		SELECT @folder_num = folder_num,
			@folder_name =folder_name,	
			@parent_num = parent_num,	
			@ex_code = ex_code,		
			@folder_level = folder_level,
			@access_level = access_level,
			@password = password,	
			@create_date = create_date
		FROM   doc_folder  
		WHERE  folder_num = @tempfolder_num
		INSERT INTO #tempDocFolderDetails VALUES (@folder_num, 
	   						  @folder_name,	
	   						  @parent_num,	
	   						  @ex_code,
	   						  @folder_level,
	   						  @access_level,
	   						  @password,	
	   						  @create_date)  

	    	SELECT @tempfolder_num = @parent_num
	    
	   	IF @tempfolder_num = 0
		  	BREAK
	   	ELSE
			cONTINUE
	END
	
	/* Return the Data */
	SELECT * FROM #tempDocFolderDetails ORDER BY folder_level ASC

	/* Drop the temporary Table */	
	IF OBJECT_ID('tempdb..#tempDocFolderDetails') IS NOT NULL 
	    Drop Table #tempDocFolderDetails
		
END
GO