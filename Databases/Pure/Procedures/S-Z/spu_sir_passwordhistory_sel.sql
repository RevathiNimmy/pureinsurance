
EXECUTE DDLDropProcedure 'spu_SIR_PasswordHistory_sel'
GO

CREATE PROCEDURE spu_SIR_PasswordHistory_sel
       @user_id	  INT 	 
AS
	DECLARE @ReuseCount INT
	
	SELECT @ReuseCount= value FROM system_options WHERE  option_number = 5105
			
	IF @ReuseCount <> 0 
		BEGIN
		Set @ReuseCount= @ReuseCount+1
			SELECT TOP (@ReuseCount) *
					FROM PMUser_Password_History
					WHERE user_id = @user_id ORDER BY date_added DESC
		END
GO


