SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXEC DDLDropProcedure 'spu_ACT_Add_InsuranceFilePaymentDetails'
GO

CREATE PROCEDURE spu_ACT_Add_InsuranceFilePaymentDetails
    @Insurance_file_cnt int,
    @CashListItem_Id int,
    @Document_id int,
    @Transdetail_id int,
    @Amount numeric(19,4)
AS

INSERT INTO Insurance_File_Payment_Details
(
	Insurance_file_cnt,
	CashListItem_Id,
	Document_id,
	Transdetail_id,
	Amount
)
VALUES
(
	@Insurance_file_cnt,
	@CashListItem_Id,
	@Document_id,
	@Transdetail_id,
	@Amount
)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

