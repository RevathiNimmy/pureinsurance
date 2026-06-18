--Start (Arul Stephen) - (Tech Spec WR34 - Claims Recovery Party Link.doc)


SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_CLM_process_recovery'
GO

CREATE PROCEDURE spu_SAM_CLM_process_recovery
    	@recovery_id integer OUTPUT,
    	@recovery_type_id integer,
    	@claim_peril_id integer,
    	@revision_amount money,
    	@currency_id integer,
    	@version_id integer,
    	@recovery_party_type_id INT= NULL,
    	@recovery_party_cnt INT= NULL,
		@is_deleted_recovery BIT=NULL,
		@base_recovery_key INT=NULL,    
        @OverllapExisting  TINYINT = 0,
		@Initial_amount money=0 
    
--@OverllapExisting 0 For No Overllap, 1 For Open i.e Initial Reserve,  2 for Maintain i.e Revised Reserve  

AS
 	SELECT	@recovery_id  = recovery_id
 	FROM	recovery
 	WHERE	claim_peril_id = @claim_peril_id
 		AND recovery_type_id = @recovery_type_id
 		AND recovery_party_cnt = @recovery_party_cnt

	IF @is_deleted_recovery=NULL
		SELECT @is_deleted_recovery=0

	IF @base_recovery_key=NULL
		SELECT @base_recovery_key=0

 IF @base_recovery_key<>0 AND @is_deleted_recovery<>0    
 	BEGIN
 DELETE FROM recovery WHERE Base_Recovery_id=@base_recovery_key    
 END    
 ELSE IF ISNULL(@recovery_id, 0) = 0    
BEGIN    
  		INSERT INTO recovery 
		(
   			claim_peril_id ,
   			recovery_type_id,
   			currency_id,
   			initial_reserve ,
   			revised_reserve,
   			received_to_date,
   			revision_count,
   			tax_amount,
   			version_id,
   			recovery_party_type_id,
   			recovery_party_cnt
  		)
  		VALUES 
		(
   			@claim_peril_id,
   			@recovery_type_id,
   			@currency_id,
			@Initial_amount,
   			@revision_amount,
   			0,
   			0,
   			0,
   			@version_id,
   			@recovery_party_type_id,
   			@recovery_party_cnt

		)

   		SELECT @recovery_id = @@IDENTITY

   		UPDATE recovery SET base_recovery_id = @recovery_id WHERE recovery_id = @recovery_id

 	END
    
 	ELSE
	BEGIN
  IF @OverllapExisting = 0 -- No Overllap  
		UPDATE  recovery
		SET 	revised_reserve =ISNULL(revised_reserve,0) + @revision_amount,
      			revision_count =ISNULL(revision_count,0) + 1,
			recovery_party_type_id = @recovery_party_type_id,
			recovery_party_cnt = @recovery_party_cnt
 		WHERE   recovery_id = @recovery_id
                  
ELSE IF @OverllapExisting = 1 -- Overllap Initial  (Open Claim)
                  UPDATE  recovery    
                  SET revised_reserve = revised_reserve + @revision_amount
                                WHERE   recovery_id = @recovery_id    
ELSE IF (@OverllapExisting = 2) -- Overllap Revised  (Maintain Claim)
				  UPDATE recovery
				  SET revised_reserve = revised_reserve + @revision_amount 
				  WHERE recovery_id = @recovery_id   
ELSE IF (@OverllapExisting = 4) -- Overllap Revised  
                  UPDATE  recovery    
                                SET  revised_reserve =  CASE WHEN @revision_amount = 0 THEN  (SELECT revised_reserve FROM Recovery WHERE Recovery_id = @recovery_id) ELSE @revision_amount END 
                                WHERE   recovery_id = @recovery_id    
 	END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--End (Arul Stephen) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
