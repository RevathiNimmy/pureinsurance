
EXECUTE DDLDropProcedure 'Spu_SIR_PasswordHistory_add'
GO


CREATE PROCEDURE Spu_SIR_PasswordHistory_add
    @user_id int
AS

    DECLARE @secure_password varchar(100)
	DECLARE @date_created dateTime
	DECLARE @rowsCount Int

select @user_id = user_id,
       @secure_password = secure_password,
	   @date_created = password_change_date
	   from PMUser where 
	   user_id = @user_id
	   
	 select @rowsCount =  count(*) 
						from PMUser_Password_History 
					    where user_id = @user_id
		
if @rowscount >= 9 
	Begin
		WITH PHT AS
		(
		  SELECT *, ROW_NUMBER() OVER(ORDER BY date_added desc) AS RowNum
		  FROM PMUser_Password_History where user_id = @user_id
		)

		DELETE FROM PHT WHERE RowNum >9;
	End

insert into PMUser_Password_History
      (
		user_id,
		historic_password,
		date_added
      )

Values
      (
		@user_id,
		@secure_password,
		@date_created   
      )


GO


