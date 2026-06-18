SET QUOTED_IDENTIFIER OFF
GO

SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Add_TransDetail_selection'
GO

Create Procedure spu_ACT_Add_TransDetail_selection
	@Transdetail_id INT,
	@user_id INT,
	@session_guid VARCHAR(40),
	@transdetail_selection_type varchar(10),
	@linked_to_transdetail_id INT = NULL,
	@allocated_base_amount numeric(19,4)=NULL,
	@write_off_base_amount numeric(19,4)= NULL,
	@write_off_account_id int = NULL,
	@write_off_reason_id int =NULL,
	@InstalmentNumber int  = NULL

AS

Declare @transdetail_selection_type_id int 

SELECT @transdetail_selection_type_id = transdetail_selection_type_id FROM TransDetail_Selection_Type 
	WHERE Code= @transdetail_selection_type

IF EXISTS( SELECT 1 FROM TransDetail_selection WHERE  Transdetail_id = @Transdetail_id AND user_id = @user_id AND InstalmentNumber = @InstalmentNumber)
	RETURN 

INSERT INTO TransDetail_selection 
(	
		transdetail_id,
		user_id,
		session_guid,
		transdetail_selection_type_id,
		linked_to_transdetail_id,
		allocated_base_amount,
		write_off_base_amount,
		write_off_account_id,
		write_off_reason_id,
		InstalmentNumber
)
values
(
	@transdetail_id,
	@user_id,
	@session_guid,
	@transdetail_selection_type_id,
	@linked_to_transdetail_id,
	@allocated_base_amount,
	@write_off_base_amount,
	@write_off_account_id,
	@write_off_reason_id,
	@InstalmentNumber
)

