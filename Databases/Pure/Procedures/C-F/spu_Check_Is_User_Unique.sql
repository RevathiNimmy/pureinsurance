SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_Check_Is_User_Unique'
GO

CREATE PROCEDURE spu_Check_Is_User_Unique
    @UserID   int,
    @PaymentID int

AS

SELECT      approval_date, approved
FROM         Payment_approval
WHERE     (user_id = @UserID) AND (payment_cnt =@PaymentID)
               
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
