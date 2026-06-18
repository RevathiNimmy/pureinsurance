SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_RI_Arrangement_Archive'
GO

  
CREATE PROCEDURE spu_Claim_RI_Arrangement_Archive  
  @nClaim_id INT    
AS    
  
INSERT INTO Claim_RI_Arrangement_line_Broker_Participants_Archive   
(claim_ri_arrangement_line_id,  
 ri_party_cnt,  
 participation_percent)  
SELECT claim_ri_arrangement_line_id,  
 ri_party_cnt,  
 participation_percent  
FROM Claim_RI_Arrangement_line_Broker_Participants   
WHERE claim_ri_arrangement_line_id IN(SELECT claim_ri_arrangement_line_id  
         FROM Claim_RI_Arrangement_line WHERE   
         claim_id = @nClaim_id)  
         
INSERT INTO Claim_RI_Arrangement_Archive(
	claim_id ,
	ri_arrangement_id,
	risk_cnt,
	ri_band_id,
	ri_model_id,
	claim_allocation_type,
	sum_insured ,
	reserve,
	payment ,
	salvage,
	recovery,
	is_modified,
	this_reserve,
	this_payment ,
	this_salvage,
	this_recovery ,
	claim_ri_arrangement_id ,
	base_claim_ri_arrangement_id ,
	version_id,
	original_ri_arrangement_id,
	ri_arrangement_version,
	Cloned,
 xol_ri_model_id,incurred_to_date,reserve_to_date,payment_to_date,salvage_to_date,recovery_to_date,extended_limit_amount    
) 
SELECT claim_id ,
	ri_arrangement_id,
	risk_cnt,
	ri_band_id,
	ri_model_id,
	claim_allocation_type,
	sum_insured ,
	reserve,
	payment ,
	salvage,
	recovery,
	is_modified,
	this_reserve,
	this_payment ,
	this_salvage,
	this_recovery ,
	claim_ri_arrangement_id ,
	base_claim_ri_arrangement_id ,
	version_id,
	original_ri_arrangement_id,
	ri_arrangement_version,
	Cloned,
 xol_ri_model_id,incurred_to_date,reserve_to_date,payment_to_date,salvage_to_date,recovery_to_date,extended_limit_amount  
	FROM Claim_RI_Arrangement where claim_id = @nClaim_id 
  
INSERT INTO Claim_RI_Arrangement_Line_Archive (  
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
 claim_ri_arrangement_line_id,  
 base_claim_ri_arrangement_line_id,  
 version_id,  
 original_ri_arrangement_line_id,  
 retained,  
 lower_limit,  
 participation_percent,  
 grouping,  
 Is_Obligatory,ri_model_line_id,reserve_to_date,payment_to_date,salvage_to_date,recovery_to_date,claim_incurred_to_date,is_pt_archive)  
 SELECT claim_id,  
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
 claim_ri_arrangement_line_id,  
 base_claim_ri_arrangement_line_id,  
 version_id,  
 original_ri_arrangement_line_id,  
 retained,  
 lower_limit,  
 participation_percent,  
 grouping,  
 Is_Obligatory,ri_model_line_id,reserve_to_date,payment_to_date,salvage_to_date,recovery_to_date,claim_incurred_to_date,is_pt_archive FROM Claim_RI_Arrangement_Line  
      WHERE claim_id =@nClaim_id  
  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO	 