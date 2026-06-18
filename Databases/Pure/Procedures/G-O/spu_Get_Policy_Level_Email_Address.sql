EXECUTE DDLDROPPROCEDURE 'spu_Get_Policy_Level_Email_Address'

GO

CREATE PROCEDURE spu_Get_Policy_Level_Email_Address
	@Insurance_File_Cnt INT,
	@Logged_In_User INT,
	@Branch_ID INT,
	@IsNonBatchProcess TINYINT
AS

BEGIN

DECLARE @DefaultEmailAddress varchar(255)
DECLARE @SenderOptionEnabled char(1)
DECLARE @SenderEmail varchar(255)

SELECT @DefaultEmailAddress = value from SYSTEM_OPTIONS where option_number = 5047 and branch_id = @Branch_ID
SELECT @SenderOptionEnabled = ISNULL(value,'') from SYSTEM_OPTIONS where option_number = 5209 and branch_id = @Branch_ID

IF @SenderOptionEnabled = 1
BEGIN
	IF @IsNonBatchProcess = 1
	BEGIN
		SELECT ISNULL(email_address,@DefaultEmailAddress), '' FROM PMUser WHERE user_id = @Logged_In_User
	END
	ELSE
	BEGIN
		SELECT ISNULL(sender_email,@DefaultEmailAddress), ISNULL(receiver_email,'') FROM Insurance_File WHERE Insurance_File_Cnt = @Insurance_File_Cnt
	END
END
ELSE
BEGIN
	IF @IsNonBatchProcess = 1
	BEGIN
		SELECT @DefaultEmailAddress, '' FROM PMUser WHERE user_id = @Logged_In_User
	END
	ELSE
	BEGIN
		SELECT @DefaultEmailAddress, ISNULL(receiver_email,'') FROM Insurance_File WHERE Insurance_File_Cnt = @Insurance_File_Cnt
	END
END

END