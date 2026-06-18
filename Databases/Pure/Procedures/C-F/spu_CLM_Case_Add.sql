SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Case_add'
GO

CREATE PROCEDURE spu_CLM_Case_add  
    @Case_ID Integer=0,  
    @case_number varchar(50),  
    @case_opened_date datetime,  
    @case_version smallint,  
    @case_progress_id int,  
    @analyst_handler_id int,  
    @admin_handler_id int,  
    @base_case_id int,  
    @user_id smallint  
  
As  
DECLARE @New_Case_Id integer  
  
If @Case_Id <>0  
BEGIN  
    SELECT @base_case_id = base_case_id  
    FROM [Case] WHERE case_id=@Case_id  
	
	if @case_version = 0
		SELECT @case_version = case_version 
		FROM [Case] WHERE case_id=@Case_ID
	 
 /*   SELECT @case_version = MAX(case_version) + 1  
    FROM [Case] WHERE base_case_id=@base_case_id          */  
  
 UPDATE [CASE]SET  
  case_number=@case_number,  
        case_opened_date=@case_opened_date,  
        case_version=@case_version,  
		case_progress_id=@case_progress_id,  
        analyst_handler_id=@analyst_handler_id,  
        admin_handler_id=@admin_handler_id,  
        base_case_id=@base_case_id,  
        user_id=@user_id,  
  is_dirty_case=0  
 WHERE case_id=@Case_id  
  
	SELECT @Case_id 'new_case_id', @base_case_id 'base_case_id'  
 RETURN  
END  
  
IF @base_case_id=0  
  SET @base_case_id=NULL  
  
     Insert INTO [CASE](  
        case_number,  
        case_opened_date,  
        case_version,  
        case_progress_id,  
        analyst_handler_id,  
        admin_handler_id,  
        base_case_id,  
        user_id,  
  is_dirty_case)  
     Values(  
        @case_number,  
        @case_opened_date,  
        @case_version,  
        @case_progress_id,  
        @analyst_handler_id,  
        @admin_handler_id,  
        @base_case_id,  
        @user_id,  
  0)  
  
SELECT @new_case_id = @@IDENTITY  
  
IF @Case_id=0  
BEGIN  
    UPDATE [Case] SET base_case_id=@new_case_id WHERE Case_id=@New_case_id  
    SET @base_case_id = @new_case_id  
END  
  
SELECT @@IDENTITY 'new_case_id', @base_case_id 'base_case_id'  
    

