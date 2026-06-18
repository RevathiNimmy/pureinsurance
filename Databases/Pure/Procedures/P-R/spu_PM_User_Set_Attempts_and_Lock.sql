
EXEC DDLDROPPROCEDURE 'spu_PM_User_Set_Attempts_and_Lock'
GO

CREATE PROCEDURE spu_PM_User_Set_Attempts_and_Lock
@user_id	int,
@mode		int,
@incorrect_attempts_allowed int,
@is_locked	int OUTPUT
AS

SELECT
@is_locked = is_locked FROM pmuser
Where user_id=@user_id

If @mode=1 
BEGIN

if @is_locked <> 1
begin
Update PMUser set 
incorrect_attempt_count= isnull(incorrect_attempt_count,0)+1,	
is_locked=case when (isnull(incorrect_attempt_count,0)+1)>=@incorrect_attempts_allowed OR ISNULL(@is_locked,0) <> 0 
then 1 else 0 end,
@is_locked =case when (isnull(incorrect_attempt_count,0)+1)>=@incorrect_attempts_allowed
then 1 else 0 end
Where user_id=@user_id
END
else
begin
Update PMUser set 
incorrect_attempt_count= isnull(incorrect_attempt_count,0)+1,	
is_locked=1,
@is_locked =case when (isnull(incorrect_attempt_count,0)+1)>=@incorrect_attempts_allowed
then 1 else 0 end
Where user_id=@user_id
END

END
Else If @mode=2 AND @is_locked=0 
BEGIN

Update PMUser set 
incorrect_attempt_count= 0
Where user_id=@user_id
END
Else If @mode=3 
BEGIN
Update PMUser set 
incorrect_attempt_count= 0, is_locked=0,
@is_locked=0
Where user_id=@user_id

END

GO

