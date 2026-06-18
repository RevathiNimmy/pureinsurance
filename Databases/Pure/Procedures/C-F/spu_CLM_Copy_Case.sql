SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_CLM_Copy_Case'
GO

CREATE PROCEDURE spu_CLM_Copy_Case  
    @case_ID int ,  
    @user_id smallint,
	@copy_case_id int OUTPUT,  
	@version_id int = NULL OUTPUT    
  
As  

DECLARE @case_version smallint
Declare @base_case_id smallint
  
If ISNULL(@Case_Id,0) <>0  
BEGIN  
  
    SELECT @case_version = MAX(case_version) + 1
    FROM [Case] WHERE base_case_id= (SELECT base_case_id from [CASE] where case_id=@case_id)  
  
END  
	SELECT @base_case_id from [CASE] WHERE case_id=@case_id
    IF @base_case_id=0  
  	SET @base_case_id=NULL  
  
     INSERT INTO [CASE](  
        case_number,  
        case_opened_date,  
        case_version,  
        case_progress_id,  
        analyst_handler_id,  
        admin_handler_id,  
        base_case_id,  
        user_id,
		is_dirty_case)  
   SELECT  
        case_number,  
        case_opened_date,  
        @case_version,  
        case_progress_id,  
        analyst_handler_id,  
        admin_handler_id,  
        base_case_id,  
        @user_id,
		1
 	FROM [case]  
 	WHERE case_id = @case_id    
  
SELECT @copy_case_id = @@IDENTITY  
  
IF @Case_id=0  
BEGIN  
    UPDATE [Case] SET base_case_id=@copy_case_id WHERE Case_id=@copy_case_id  
    SET @base_case_id = @copy_case_id  
END  
  
SELECT @@IDENTITY 'copy_case_id', @base_case_id 'base_case_id'  
