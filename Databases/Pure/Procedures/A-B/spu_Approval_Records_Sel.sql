SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_Approval_Records_Sel'
GO

CREATE PROCEDURE spu_Approval_Records_Sel
    @PaymentType int,
    @PaymentID int

AS

     SELECT     approval_cnt, approval_user_group, user_id, approved, approval_date
     FROM         Payment_approval
     WHERE     (payment_type = @PaymentType) AND (payment_cnt = @PaymentID)


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

