ddldropprocedure 'spu_SIR_ReCalculate_Policy_Fee'
go

CREATE PROCEDURE spu_SIR_ReCalculate_Policy_Fee
 	@insurance_file_cnt int
AS

BEGIN
declare @policy_fee_id integer
declare @premium numeric(19,4)
declare @fee_amount numeric(19,4)
declare @Tax_Amount numeric(19,4)
declare @Total_Fee numeric(19,4)
declare @fee_percentage numeric(19,4)
declare @old_tax_amount numeric(19,4)
declare @old_fee_amount numeric(19,4)
declare @tax_percentage numeric(19,4)

--Get Premium Value (exc tax)
select @Premium = sum(premium_excluding_tax) from insurance_cob_section where insurance_file_cnt = @insurance_file_cnt

DECLARE CURSOR_Policy_Fee CURSOR STATIC FOR
	select policy_fee_id, fee_percentage, tax_amount, fee_amount from policy_fee where insurance_file_cnt=@insurance_file_cnt and fee_percentage <> 0

OPEN CURSOR_Policy_Fee

FETCH NEXT FROM CURSOR_Policy_Fee INTO
  	@policy_fee_id, @fee_percentage, @old_tax_amount, @old_fee_amount

WHILE @@FETCH_STATUS = 0
BEGIN
	--Apply to Policy_Fees where percentage is populated
	select @tax_percentage=0
	if @old_fee_amount <> 0 
	Begin
		select @tax_percentage = @old_tax_amount / @old_fee_amount
	End

	select @fee_amount = @premium * @fee_percentage
	select @tax_amount = @fee_amount * @tax_percentage
	select @total_fee = @tax_amount + @fee_amount

	update 
		policy_fee 
	set 
		fee_amount = round(@fee_amount,2), 
		tax_amount = round(@tax_amount,2), 
		total_fee = round(@total_fee,2)
	where 
		policy_fee_id = @policy_fee_id

	FETCH NEXT FROM CURSOR_Policy_Fee INTO
		@policy_fee_id, @fee_percentage, @old_tax_amount, @old_fee_amount
END

CLOSE CURSOR_Policy_Fee
DEALLOCATE CURSOR_Policy_Fee

END