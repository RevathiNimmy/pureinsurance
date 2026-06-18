SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


Execute DDLDropProcedure 'spu_Payment_Approval_Add'
GO

CREATE PROCEDURE spu_Payment_Approval_Add
     @PaymentType int,
     @PaymentCnt int,
     @Payment_Amount numeric (19,4), 
     @ApprovalGroup int, 
     @UserID int,
     @Approved int

AS

INSERT INTO Payment_approval (
                         payment_type, 
                         payment_cnt, 
                         Payment_Amount,
                         approval_user_group, 
                         user_id, 
                         approved, 
                         approval_date)
VALUES         (
                         @PaymentType,
                         @PaymentCnt,
                         @Payment_Amount, 
                         @ApprovalGroup,
                         @UserID,
                         @Approved, 
                         getdate())

