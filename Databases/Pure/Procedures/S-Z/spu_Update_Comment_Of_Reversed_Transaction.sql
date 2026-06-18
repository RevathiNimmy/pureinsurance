SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Update_Comment_Of_Reversed_Transaction'
GO

CREATE PROCEDURE spu_Update_Comment_Of_Reversed_Transaction
    @transaction_id int,
    @user_name varchar(255)
    
AS

BEGIN
    DECLARE @dtdate DATETIME
    SET @dtdate = GETDATE()

    UPDATE transdetail 
    SET comment = 'Original allocation reversed as part of Backdated Endorsement processed by user ' + '[' + @user_name + '] on ' + cast(@dtdate as char) 
    WHERE transdetail_id = @transaction_id
    
END 
GO

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS ON 
GO

