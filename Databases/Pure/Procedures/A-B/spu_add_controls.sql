SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_add_controls'
GO

CREATE PROCEDURE spu_add_controls  
  
    @claimid int,  
    @perildatadefnid int  
AS  
  
BEGIN  
  
	DECLARE @version_id int 
	DECLARE @user_defined_peril_data_id int 
	
	EXEC spu_CLM_Get_claim_version
		@claim_id = @claimid, 
		@version_id  = @version_id OUTPUT

	INSERT INTO user_defined_peril_data
	(  
		claim_id,  
		peril_data_defn_id, 
		version_id 
	)  
	VALUES
	(  
		@claimid,  
		@perildatadefnid,
		@version_id
	)  

	SET @user_defined_peril_data_id = @@IDENTITY

	UPDATE user_defined_peril_data   
	SET base_user_defined_peril_data_id =@user_defined_peril_data_id
	WHERE user_defined_peril_data_id= @user_defined_peril_data_id

END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
