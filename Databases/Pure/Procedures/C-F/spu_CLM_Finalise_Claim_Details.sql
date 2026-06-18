SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Finalise_Claim_Details'
GO

CREATE PROCEDURE spu_CLM_Finalise_Claim_Details  
  
@claim_id int, 
@claim_version_description varchar(1000),  
@finalPayment int = NULL  

AS  
  
BEGIN  
IF @claim_version_description LIKE 'Claim Clone Batch Processed%'
BEGIN
	UPDATE claim  
	SET claim_version_description = claim_version_description + ' - ' + @claim_version_description,
    is_final_payment = @finalPayment  
	WHERE claim_id = @claim_id  
	END
ELSE
BEGIN
	UPDATE claim 
	SET claim_version_description = @claim_version_description
	WHERE claim_id = @claim_id
END
UPDATE claim_ri_arrangement  
SET  
 reserve = ISNULL(reserve,0) + ISNULL(this_reserve,0),  
 payment = ISNULL(payment,0) + ISNULL(this_payment,0),  
 salvage = ISNULL(salvage,0) + ISNULL(this_salvage,0),  
 recovery = ISNULL(recovery,0) + ISNULL(this_recovery,0),
 reserve_to_date =ISNULL(reserve_to_date,0) + ISNULL(this_reserve,0),
 payment_to_date = ISNULL(payment_to_date,0) + ISNULL(this_payment,0), 
 salvage_to_date =ISNULL(salvage_to_date,0) + ISNULL(this_salvage,0), 
 recovery_to_date = ISNULL(recovery_to_date,0) + ISNULL(this_recovery,0)
WHERE claim_id = @claim_id  
  
UPDATE claim_ri_arrangement_line  
SET  
 reserve = ISNULL(reserve,0) + ISNULL(this_reserve,0),  
 payment = ISNULL(payment,0) + ISNULL(this_payment,0),  
 salvage = ISNULL(salvage,0) + ISNULL(this_salvage,0),  
 recovery = ISNULL(recovery,0) + ISNULL(this_recovery,0),
 reserve_to_date =ISNULL(reserve_to_date,0) + ISNULL(this_reserve,0),
 payment_to_date = ISNULL(payment_to_date,0) + ISNULL(this_payment,0), 
 salvage_to_date =ISNULL(salvage_to_date,0) + ISNULL(this_salvage,0), 
 recovery_to_date = ISNULL(recovery_to_date,0) + ISNULL(this_recovery,0)
WHERE claim_id = @claim_id

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
