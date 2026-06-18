SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pm_Add_or_Update_Registry'
GO
CREATE PROC spu_pm_Add_or_Update_Registry  
@KeyPath VARCHAR(255),  
@KeyName VARCHAR(255),  
@KeyData VARCHAR(255) ,
@UserName VARCHAR(100),
@MachineName VARCHAR(100)
AS  

IF @UserName<>''
BEGIN
	IF EXISTS(SELECT NULL FROM Registry_Setting WHERE KeyPath= @KeyPath AND KeyName = @KeyName AND System_Logged_in_User=@UserName)  
		UPDATE Registry_Setting set KeyData =@KeyData WHERE KeyPath=@KeyPath and KeyName = @KeyName AND System_Logged_in_User=@UserName 
	ELSE
	INSERT INTO Registry_Setting (KeyPath,KeyName,KeyType,keydata,System_Logged_in_User) VALUES(@KeyPath,@KeyName,'String',@KeyData,@UserName)  
END
ELSE 
BEGIN
	DECLARE @system_id INT  
	SELECT @system_id = system_id FROM PMSystem WHERE system_name=@MachineName
	IF EXISTS(SELECT NULL FROM Registry_Setting WHERE KeyPath= @KeyPath AND KeyName = @KeyName AND System_id=@system_id)  
		UPDATE Registry_Setting  SET KeyData =@KeyData FROM Registry_Setting WHERE KeyPath= @KeyPath AND KeyName = @KeyName AND System_id=@system_id
	ELSE
	INSERT INTO Registry_Setting (KeyPath,KeyName,KeyType,keydata,System_id) VALUES(@KeyPath,@KeyName,'String',@KeyData,@system_id)  
END

GO