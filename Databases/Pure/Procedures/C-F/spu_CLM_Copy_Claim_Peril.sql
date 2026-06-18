SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Copy_Claim_Peril'
GO

CREATE PROCEDURE spu_CLM_Copy_Claim_Peril  
	@claim_id int,  
	@copy_claim_id int,  
	@version_id int,
	@status int OUTPUT  
AS  
  
 DECLARE @claim_peril_id int,  
  @peril_type_id int,  
  @description varchar(255),  
  @comments varchar(255),  
  @sum_insured money,  
  @ri_band int,  
  @Return_Status int,  
  @copy_claim_peril_id int,  
  @copy_recovery_id int,  
  @recovery_id int,  
  @copy_reserve_id int,  
  @reserve_id int,  
  @gis_screen_id int  
  
 -- cursor to get back all claim_peril for this claim  
 DECLARE Claim_Peril_Cursor CURSOR FAST_FORWARD FOR  
 SELECT  claim_Peril_id,  
  Peril_type_id,  
  Description,  
  Comments,  
  sum_insured,  
  ri_band,  
  gis_screen_id  
 FROM    claim_peril WITH (NOLOCK)  
 WHERE   claim_id = @claim_id  
  
 -- loop through cursor and copy claim_peril details to work  
 OPEN Claim_Peril_Cursor  
  
 FETCH NEXT FROM Claim_Peril_Cursor  
 INTO    @claim_peril_id,  
  @peril_type_id,  
  @description,  
  @comments,  
  @sum_insured,  
  @ri_band,  
  @gis_screen_id  
  
 WHILE @@FETCH_STATUS = 0  
 BEGIN  
  
  -- add claim peril details to claim peril table with copy_claim_id  
  INSERT INTO Claim_Peril (  
   Claim_id,  
   Peril_type_id,  
   Description,  
   Comments,  
   sum_insured,  
   ri_band,  
   gis_screen_id,  
   base_claim_peril_id,  
   version_id)  
  SELECT  
   @copy_claim_id,  
                 @peril_type_id,  
                 @description,  
                 @comments,  
                 @sum_insured,  
                 @ri_band,  
             @gis_screen_id,  
                 base_claim_peril_id,  
      @version_id  
         FROM    claim_peril cp WITH (NOLOCK)  
         WHERE   cp.claim_id = @claim_id  
         AND     cp.peril_type_id = @peril_type_id  
  AND   cp.claim_peril_id = @claim_peril_id  
  
  -- did we get an error copying claim_peril  
  IF @@ERROR <> 0  
   GOTO Error_Routine  
  
  -- get new work claim_peril_id  
  SELECT @copy_claim_peril_id = @@IDENTITY  
 
  -- copy recovery to work for this claim_peril_id  
  DECLARE Recovery_Cursor CURSOR FAST_FORWARD FOR  
   SELECT  recovery_id  
   FROM    recovery WITH (NOLOCK)  
   WHERE   claim_peril_id = @claim_peril_id  
  
  -- loop through cursor and copy claim_peril details to work  
  OPEN Recovery_Cursor  
  
  FETCH NEXT FROM Recovery_Cursor  
  INTO    @recovery_id  
  
  WHILE @@FETCH_STATUS = 0  
   BEGIN  
    INSERT INTO recovery (  
     claim_Peril_id,  
     Recovery_type_id,  
     Currency_id,  
     Initial_reserve,  
     revised_reserve,  
     received_to_date,  
     revision_count,  
     tax_amount,  
     base_recovery_id,  
--Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)    
     version_id,    
     recovery_party_type_id,    
    recovery_party_cnt,  
    this_receipt_net)    
--End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc) )    
    SELECT  @copy_claim_peril_id,  
     r1.recovery_type_id,  
     r1.currency_id,  
     r1.initial_reserve,  
     r1.revised_reserve,  
     r1.received_to_date,  
     r1.revision_count,  
     r1.tax_amount,  
     r1.base_recovery_id,  
   --Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)    
    @version_id,    
    r2.recovery_party_type_id,    
    r2.recovery_party_cnt,  
    0    
 --End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)    
           FROM    recovery r1    
		Inner Join recovery r2 on r2.base_recovery_id = r1.base_recovery_id 
		Inner Join claim_peril cp on cp.claim_peril_id = r2.claim_peril_id
		Inner Join 
			(Select MAX(c1.claim_id) 'claim_id' From claim c1
				Inner Join claim c2 ON c1.base_claim_id = c2.base_claim_id AND c2.claim_id = @copy_claim_id
					Where c1.is_dirty = 0 AND c1.transaction_type_id IN (1, 2)) c
			ON c.claim_id = cp.claim_id
           WHERE   r1.recovery_id = @recovery_id  
  
    -- did we get an error copying recovery to work  
    IF @@ERROR <> 0  
     GOTO Error_Routine  
  
    -- get new work claim_peril_id  
    SELECT @copy_recovery_id = @@IDENTITY  
  
    FETCH NEXT FROM Recovery_Cursor  
    INTO    @recovery_id  
  
   END  
  
  CLOSE Recovery_Cursor  
  
  DEALLOCATE Recovery_Cursor  
  
  -- copy reserve to work  
  DECLARE Reserve_Cursor CURSOR FAST_FORWARD FOR  
   SELECT  reserve_id  
   FROM    reserve WITH (NOLOCK)  
   WHERE   claim_peril_id = @claim_peril_id  
  
  -- loop through cursor and copy claim_peril details to work  
  OPEN Reserve_Cursor  
  
  FETCH NEXT FROM Reserve_Cursor  
  INTO    @reserve_id  
  
  WHILE @@FETCH_STATUS = 0  
  BEGIN  
   INSERT INTO Reserve (  
    claim_Peril_id,  
    Reserve_type_id,  
    Initial_reserve,  
    This_Revision,  
    Paid_to_date,  
    This_Payment,  
    Revised_reserve,  
    Sum_insured,  
    Revision_count,  
    Average,  
    base_reserve_id,  
    version_id,
	Gross_Reserve,
	Tax,
	Revised_Gross_Reserve,
	Revised_Tax_Reserve,
	paid_to_date_tax)  
   SELECT  @copy_claim_peril_id,  
    reserve_type_id,  
    initial_reserve,  
    0,  
    paid_to_date,  
    0,  
    revised_reserve,  
    sum_insured,  
    revision_count,  
    average,  
    base_reserve_id,  
    @version_id,
	Gross_Reserve,
	tax,
	ISNULL(Revised_Gross_Reserve,0),
	ISNULL(Revised_Tax_Reserve,0),
	paid_to_date_tax
   FROM    reserve WITH (NOLOCK)  
   WHERE   reserve_id = @reserve_id  
   
   -- did we get an error copying reserve to work  
   IF @@ERROR <> 0  
    GOTO Error_Routine  
  
   FETCH NEXT FROM Reserve_Cursor  
    INTO    @reserve_id  
  
  END  
  
  CLOSE Reserve_Cursor  
  
  DEALLOCATE Reserve_Cursor  
  
  -- copy receipt to work  
  EXEC spu_CLM_Copy_Receipt  
   @claim_id,  
   @copy_claim_id,  
   @claim_peril_id,  
   @copy_claim_peril_id,  
   @version_id,  
   @status OUTPUT  
  
     -- did we get an error copying receipt to work  
     IF @status <> 0  
         GOTO Error_Routine  
  
     -- copy payment to work  
     exec spu_CLM_Copy_Payment  
             @claim_id,  
             @copy_claim_id,  
             @claim_peril_id,  
             @copy_claim_peril_id,  
      @version_id,  
             @status OUTPUT  
  
     --  did we get an error copying payment to work  
     IF @status <> 0  
         GOTO Error_Routine  
  
     -- get next claim_peril record  
     FETCH NEXT FROM Claim_Peril_Cursor  
         INTO    @claim_peril_id,  
                 @peril_type_id,  
                 @description,  
                 @comments,  
                 @sum_insured,  
                 @ri_band,  
     @gis_screen_id  
  
 END  
  
CLOSE Claim_Peril_Cursor  
DEALLOCATE Claim_Peril_Cursor  
  
SELECT  @status = 0  
RETURN  
  
Error_Routine:  
  
    CLOSE Claim_Peril_Cursor  
    DEALLOCATE Claim_Peril_Cursor  
  
    SELECT  @status = -1  
    RETURN  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
