SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
EXECUTE DDLDropProcedure 'spu_clm_Process_Decline'
GO

-- Object:  Stored Procedure dbo.spu_clm_Process_Decline
-- Script Date: 12/1/2003 3:23:17 PM ******/
CREATE PROCEDURE spu_clm_Process_Decline
     @claim_id int,
     @Payment_Id int
AS
BEGIN
DECLARE @Claim_payment_item_id INT,
            @base_claim_payment_item_id INT,
            @Reserve_id int,
            @This_payment money,
            @this_reserve money ,
            @base_claim_id INT ,
            @next_claim_id INT,@Reserve money ,@Payment money , @Claim_ri_arrangement_id INT
Set @this_payment=0
Set @this_reserve=0

DECLARE CUR_CPI CURSOR FAST_FORWARD FOR 
      Select Claim_payment_item_id, Reserve_id From Claim_payment_item
      WHERE Claim_payment_id = @payment_id

OPEN CUR_CPI
FETCH NEXT FROM CUR_CPI INTO @Claim_payment_item_id, @Reserve_id

WHILE @@FETCH_STATUS=0
BEGIN
    Select @this_payment = this_payment, @this_reserve = this_revision From Reserve
    Where Reserve_id = @Reserve_id

    Update      Reserve SET
    paid_to_date = paid_to_date - @this_payment, Revised_reserve = Revised_reserve - @this_reserve
    Where Reserve_id >= @Reserve_id AND base_reserve_id = (Select base_reserve_id From Reserve Where Reserve_id = @Reserve_id)

    Update      Reserve SET
    this_payment = 0, this_revision = 0
    Where Reserve_id = @Reserve_id 
    
FETCH NEXT FROM CUR_CPI INTO @Claim_payment_item_id, @Reserve_id
END

CLOSE CUR_CPI
DEALLOCATE CUR_CPI

----------------------------------------------------------------------
Declare @base_Claim_ri_arrangement_line_id int,
            @ri_arrangement_line_id int,  
            @ri_model_line_id int  
            
Set @this_payment=0
Set @this_reserve=0

Declare cur_RILine CURSOR FAST_FORWARD FOR
      Select DISTINCT base_Claim_ri_arrangement_line_id, this_payment, this_reserve,ri_model_line_id From claim_ri_arrangement_line  
      Where Claim_id= @Claim_id

Open cur_RILine
Fetch next From cur_RILine into @base_Claim_ri_arrangement_line_id, @This_payment , @this_reserve,@ri_model_line_id  
While @@Fetch_status= 0
Begin
  IF @base_Claim_ri_arrangement_line_id  is not null  
   BEGIN  
	Update claim_ri_arrangement_line Set this_payment = 0, this_reserve = 0
		Where base_claim_ri_arrangement_line_id = @base_Claim_ri_arrangement_line_id and claim_id = @claim_id 

  Update claim_ri_arrangement_line Set Payment = Payment - @this_payment, reserve = reserve - @this_reserve,
						 Payment_to_date =Payment_to_date -@This_payment,
						 reserve_to_date = reserve_to_date -@this_reserve    
   Where base_Claim_ri_arrangement_line_id = @base_Claim_ri_arrangement_line_id And claim_id >= @claim_id  and is_pt_archive =0
 END  
  Else  
    BEGIN  
   Update claim_ri_arrangement_line Set this_payment = 0, this_reserve = 0  
   Where claim_id = @claim_id  and ri_model_line_id = @ri_model_line_id  

  Update claim_ri_arrangement_line Set Payment = Payment - @this_payment, reserve = reserve - @this_reserve,
  				       Payment_to_date =Payment_to_date -@This_payment,
				       reserve_to_date = reserve_to_date -@this_reserve    
   Where claim_id in (SELECT c2.claim_id from Claim C1 Join claim C2 on c1.base_claim_id=c2.base_claim_id  
   WHERE c1.Claim_id  = @claim_id and c2.Claim_id>=@claim_id and c2.is_dirty=0)  
   and ri_model_line_id = @ri_model_line_id  and is_pt_archive =0
END
Fetch next From cur_RILine into @base_Claim_ri_arrangement_line_id, @This_payment , @this_reserve,@ri_model_line_id  
END  
Close cur_RILine
Deallocate cur_RILine
-----------------------------------------

Declare @base_Claim_ri_arrangement_id int,
            @ri_arrangement_id int

Set @this_payment = 0
Set @this_reserve = 0

      Declare cur_RIArrangement CURSOR FAST_FORWARD FOR
            Select Distinct base_Claim_ri_arrangement_id, this_payment, this_reserve From Claim_RI_Arrangement
            Where Claim_ID = @Claim_ID

      Open cur_RIArrangement
      Fetch Next From cur_RIArrangement into @base_Claim_ri_arrangement_id, @This_payment, @this_reserve
      WHILE @@Fetch_status=0
      BEGIN
		Update Claim_RI_Arrangement Set this_payment = 0, this_reserve = 0
			Where base_Claim_ri_arrangement_id = @base_Claim_ri_arrangement_id and claim_id = @claim_id 

  Update Claim_RI_Arrangement Set Payment = Payment - @this_payment, reserve = reserve - @this_reserve ,
					    Payment_to_date =payment_to_date -@This_payment,
					    reserve_to_date = reserve_to_date -  @this_reserve								  
			Where base_Claim_ri_arrangement_id = @base_Claim_ri_arrangement_id And claim_id >= @claim_id 
			
        Fetch Next From cur_RIArrangement into @base_Claim_ri_arrangement_id, @This_payment , @this_reserve
      END
      Close cur_RIArrangement
      Deallocate cur_RIArrangement

-----------------------------------------
DELETE Tax_Calculation  Where Claim_payment_item_id in
(Select claim_payment_item_id From Claim_payment_Item Where claim_payment_id= @payment_id)

DELETE from CashListItem_claim_link
WHERE Claim_payment_Id = @Payment_ID

UPDATE claim_payment SET amount= 0, tax_amount= 0, tax_amount_wht= 0, is_referred=2 WHERE claim_payment_id IN
(SELECT claim_payment_id FROM claim_payment INNER JOIN
(SELECT base_claim_payment_id FROM claim_payment WHERE claim_payment_id = @Payment_Id) CP1
ON claim_payment.base_claim_payment_id = CP1.base_claim_payment_id)
Select @base_claim_id = base_claim_id from claim where Claim_id =@claim_id 


DECLARE @nprogress_status_id INT 
SELECT @nprogress_status_id = progress_status_id FROM Progress_Status WHERE code = 'REOPENED'

IF ISNULL(@nprogress_status_id,0)=0
BEGIN
 SELECT @nprogress_status_id = progress_status_id FROM Progress_Status WHERE code = 'OPEN'
END

DECLARE @nClaim_id INT  -- Claim ID of last claim version
DECLARE @ClaimNumber VARCHAR(20)
Select @ClaimNumber = Claim_Number FROM Claim WHERE Claim_ID = @claim_id 
SELECT @nClaim_id= MAX(Claim_ID) FROM Claim WHERE Claim_Number = @ClaimNumber 

UPDATE Claim
SET Claim_Status_ID = 4 , progress_status_id = ISNULL(@nprogress_status_id,progress_status_id) ,
Claims_status_date=GETDATE()
WHERE Claim_ID = @nClaim_id AND Claim_Status_ID IN (3,5)


Declare cur_RecalculateRI CURSOR FAST_FORWARD FOR  
    SELECT Claim_id
	FROM     Claim	
	WHERE claim.base_claim_id =@base_claim_id
	AND claim_id > @claim_id And is_dirty =0
Open cur_RecalculateRI  
Fetch next From cur_RecalculateRI into @next_claim_id
While @@Fetch_status= 0  
Begin  
exec spu_Copy_Reinsurance_Details_To_Claim_RI2007  @claim_id = @next_claim_id   
exec spu_CLM_Finalise_Claim_Details @claim_id = @next_claim_id , @claim_version_description = 'RI recalculated after payment declined',@finalPayment = 0	
Fetch next From cur_RecalculateRI into @next_claim_id	
End
Close cur_RecalculateRI
Deallocate cur_RecalculateRI

END


