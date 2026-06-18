SET QUOTED_IDENTIFIER OFF 

SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Claim_Copy_RIArrangementLines'
GO

CREATE Procedure spu_Claim_Copy_RIArrangementLines      
    @Claim_id INT,      
    @Copy_Claim_ID INT,      
    @ri_version_id INT      
AS      
   

    DECLARE @Claim_RI_Arrangement_Id INT,      
        @Reserve Numeric(10,4),      
        @Payment Numeric(10,4),      
        @ri_arrangement_line_id    INT,  
        @max_ri_arrangement_line_id    INT  
    
    INSERT INTO Claim_Ri_Arrangement      
    (   claim_id,      
        ri_arrangement_id,      
        risk_cnt,      
        ri_band_id,      
        ri_model_id,      
        claim_allocation_type,      
        sum_insured,      
        reserve,      
        payment,      
        salvage,      
        recovery,      
        is_modified,      
        this_reserve,      
        this_payment,      
        this_salvage,      
        this_recovery,      
        base_claim_ri_arrangement_id,      
        version_id,      
        original_ri_arrangement_id,      
        ri_arrangement_version)      
    SELECT       
        claim_id,      
        (SELECT MAX(ri_arrangement_id)+1 FROM Claim_Ri_Arrangement),      
        risk_cnt,      
        NULL,      
        ri_model_id,      
        claim_allocation_type,      
        sum_insured,      
        reserve,      
        payment,      
        salvage,      
        recovery,      
        is_modified,      
        this_reserve,      
        this_payment,      
        this_salvage,      
        this_recovery,      
        base_claim_ri_arrangement_id,      
        version_id,      
        original_ri_arrangement_id,      
        @ri_version_id      
    FROM Claim_Ri_Arrangement      
    WHERE claim_id=@Claim_Id      
    And ri_arrangement_version=@RI_Version_id-1      
          
    SELECT  @Claim_RI_Arrangement_Id =RI_Arrangement_Id       
    FROM    Claim_RI_Arrangement      
    WHERE   claim_id=@Claim_Id      
    And     ri_arrangement_version=@RI_Version_id      
          
  
    DECLARE CRA_insert CURSOR  
    STATIC  
    FOR  
        SELECT ri_arrangement_line_id  
        FROM   claim_ri_arrangement_line  
        WHERE  claim_id=@Copy_Claim_Id      
      
    OPEN CRA_insert  
      
    FETCH NEXT FROM CRA_insert INTO      
                @ri_arrangement_line_id  
          
    WHILE @@Fetch_status=0    
    BEGIN    
  
                SELECT @max_ri_arrangement_line_id = MAX(ri_arrangement_line_id)+1  
                FROM    claim_ri_arrangement_line  
  
                Insert Into Claim_Ri_arrangement_Line      
                (      
                    claim_id,  
                    ri_arrangement_line_id,       
                    ri_arrangement_id,      
                    type,      
                    treaty_id,      
                    party_cnt,      
                    xol_arrangement_id,      
                    default_share_percent,      
                    this_share_percent,      
                    agreement_code,      
                    priority,      
                    number_of_lines,      
                    line_limit,      
                    sum_insured,      
                    reserve,      
                    payment,      
                    salvage,      
                    recovery,      
                    this_reserve,      
                    this_payment,      
                    this_salvage,      
                    this_recovery,      
                    base_claim_ri_arrangement_line_id,      
                    version_id,      
                    original_ri_arrangement_line_id,      
                    retained,      
                    lower_limit,      
                    participation_percent,      
                    grouping      
                )      
                Select            
                    @claim_id,      
                    @max_ri_arrangement_line_id,  
                    @Claim_ri_arrangement_id,      
                    type,      
                    treaty_id,      
                    party_cnt,      
                    xol_arrangement_id,      
                    default_share_percent,      
                    this_share_percent,      
                    agreement_code,      
                    priority,      
                    number_of_lines,      
                    line_limit,      
                    sum_insured,      
                    reserve,      
                    payment,      
                    salvage,      
                    recovery,      
                    this_reserve,      
                    this_payment,      
                    this_salvage,      
                    this_recovery,      
                    base_claim_ri_arrangement_line_id,      
                    version_id,      
                    original_ri_arrangement_line_id,      
                    retained,      
                    lower_limit,      
                    participation_percent,      
                    grouping            
                From Claim_Ri_Arrangement_Line      
                Where claim_id=@Copy_Claim_Id      
    and ri_arrangement_line_id=@ri_arrangement_line_id  
      
  FETCH NEXT FROM CRA_insert INTO      
                @ri_arrangement_line_id  
      
   END  
     
   CLOSE CRA_insert    
   DEALLOCATE CRA_insert    
   
   SELECT  @Reserve=Reserve,@Payment=Payment 
   FROM  Claim_ri_Arrangement      
   WHERE  Claim_Id = @claim_id      
    
    update   Claim_Ri_Arrangement_Line    
    set default_share_percent = this_share_percent    
    Where claim_id=@claim_id  and type='F'    
    
      
    Exec spu_calculate_claims_ri_method_2_full      
            @Claim_id =@Claim_Id,      
            @Ri_arrangement_id =@Claim_ri_arrangement_id ,      
            @total_reserve=@Reserve ,        
            @total_payment= @Payment       
      
      
SET QUOTED_IDENTIFIER OFF 

SET ANSI_NULLS ON
GO
      
    
  
