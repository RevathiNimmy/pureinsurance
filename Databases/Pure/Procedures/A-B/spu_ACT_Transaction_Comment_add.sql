SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Transaction_Comment_add'
GO


CREATE PROCEDURE spu_ACT_Transaction_Comment_add
    @transaction_comment_id int OUTPUT,
    @transdetail_id int,
    @cashlistitem_id int,
    @description varchar(500),
    @comment_date datetime,
    @user_id int
AS

BEGIN
IF @transdetail_id = 0 
   SELECT @transdetail_id = NULL
END

BEGIN
IF @cashlistitem_id = 0 
   SELECT @cashlistitem_id = NULL
END

BEGIN

INSERT INTO transaction_comment (
    transdetail_id,
    cashlistitem_id,
    description,
    comment_date,
    user_id)
VALUES (
    @transdetail_id,
    @cashlistitem_id,
    @description,
    @comment_date,
    @user_id)
END

BEGIN
SELECT @transaction_comment_id = @@IDENTITY
END
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF 
GO