SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_RI_Model_Line_saa'
GO


CREATE PROCEDURE spu_RI_Model_Line_saa  
    @ri_model_id int,  
    @FilterType INT = 0,  
    @TreatyTypeCode VARCHAR(20)='',
    @ri_arrangement_id INT = NULL
  
AS  

IF IsNull(@FilterType,0) = 0
	 SELECT  rml.ri_model_line_id,  
	 rml.ri_model_id,  
	 rml.priority,  
	 rml.number_of_lines,  
	 rml.line_limit,  
	 rml.treaty_id,  
	 t.description,  
	 rml.share_percent,  
	 rml.lower_limit,  
	 rml.ceding_rate,  
	 rml.Treaty_type_id,  
	 t.reinsurance_type_id,  
	 rml.is_obligatory,  
	 rml.cede_premium_only,  
	 t.code TreatyCode,  
	 rt.code ReinsuranceTypeCode,  
	 tt.code TreatyTypeCode,
	 t.effective_date,  
	 t.expiry_date,
	 rml.premium_calculation_basis_id,
	 rml.Is_VariableQuotaShare ,
	 0 as ManuallyAddedTreaty
	 FROM    RI_Model_Line rml  
	 JOIN  treaty t ON t.treaty_id = rml.treaty_id  
	 JOIN reinsurance_type rt ON rt.reinsurance_type_id=t.reinsurance_type_id  
	 JOIN treaty_type tt ON tt.Treaty_type_id=rml.Treaty_type_id  
	 WHERE   rml.ri_model_id = @ri_model_id  
	  AND (
			 (@TreatyTypeCode = '')
			 OR
			 (Rtrim(tt.code) = @TreatyTypeCode)
	      )
	  ORDER BY  
	  priority,  
	  share_percent  

ELSE IF(IsNull(@FilterType,0) = 1)
			SELECT 
			ral.ri_model_line_id,
			ra.ri_model_id,
			ral.priority,
			ral.number_of_lines,
			ral.line_limit,
			ral.treaty_id,
			t.description,
			(Case WHEN ral.type = 'TX' OR ral.type= 'TC' Then 100
			 ELSE ral.default_share_percent
			 END)  share_percent,
			ral.lower_limit,
			(Case WHEN ral.type = 'TX' OR ral.type= 'TC' OR ral.type='R' Then ral.default_share_percent
				  WHEN ral.type = 'T' OR RAL.type='TFS' Then 0
			  ELSE NULL
			  END				  
			 ) ceding_rate,													
			(select Treaty_Type_id from Treaty_Type  where code = (CASE WHEN ral.type = 'TX' OR ral.type = 'TC' Then 'XOL'
																		 WHEN ral.type = 'T' OR ral.type = 'TFS' OR ral.type = 'R' Then 'PROP' 
																	ELSE NULL
																	END
																	)
			) Treaty_Type_id,
			t.reinsurance_type_id,
			ral.Is_Obligatory,
			ral.Is_Obligatory,			 
			t.code TreatyCode,
			rt.code ReinsuranceTypeCode,
			(CASE WHEN ral.type = 'TX' OR ral.type = 'TC' Then 'XOL'
				  WHEN ral.type = 'T' OR ral.type = 'TFS' OR ral.type = 'R' Then 'PROP' 
			 ELSE NULL
			 END
			) TreatyTypeCode,

			t.effective_date,
			t.expiry_date,
			0 premium_calculation_basis_id,
			0 AS Is_VariableQuotaShare,
			ISNULL(ral.manually_added,0) ManuallyAddedTreaty
			FROM  RI_Arrangement_Line ral 
			JOIN RI_Arrangement ra ON ral.ri_arrangement_id = ra.ri_arrangement_id 
			JOIN Treaty t ON t.treaty_id = ral.treaty_id
			JOIN Reinsurance_type rt ON rt.reinsurance_type_id = t.reinsurance_type_id 			
			WHERE ra.ri_arrangement_id = @ri_arrangement_id 
			AND ral.type <> 'FX'
			AND ral.type <> 'F'
			AND 
				(
					(@TreatyTypeCode = '')
				or 
					@TreatyTypeCode = (CASE WHEN ral.type = 'TX' OR ral.type = 'TC' Then 'XOL'
										WHEN ral.type = 'T' OR ral.type = 'TFS' OR ral.type = 'R' Then 'PROP' 
										ELSE NULL
										END
					)
				)
			ORDER BY 
			ral.priority, 
			ral.default_share_percent 

	
	-- Claim
ELSE IF(IsNull(@FilterType,0) = 2)
			SELECT 
			--c_ral.ri_arrangement_id ,
			(0) ri_model_line_id, 
			c_ra.ri_model_id,
			c_ral.priority,
			c_ral.number_of_lines,
			c_ral.line_limit,
			c_ral.treaty_id,
			t.description,
			(Case WHEN c_ral.type = 'TX' OR c_ral.type= 'TC' Then 100
			 ELSE c_ral.default_share_percent
			 END)  share_percent,
			c_ral.lower_limit,
			(Case WHEN c_ral.type = 'TX' OR c_ral.type= 'TC' Then (select ral.default_share_percent from RI_Arrangement_Line ral
			                                                       where ral.ri_arrangement_id = c_ra.original_ri_arrangement_id 
			                                                       AND ral.type = c_ral.type 
			                                                       AND ral.treaty_id = c_ral.treaty_id 
			                                                        )
			      WHEN c_ral.type='R' Then c_ral.default_share_percent
				  WHEN c_ral.type = 'T' OR c_ral.type='TFS' Then 0
			  ELSE NULL
			  END				  
			 ) ceding_rate,	
			(select Treaty_Type_id from Treaty_Type  where code = (CASE WHEN c_ral.type = 'TX' OR c_ral.type = 'TC' Then 'XOL'
																		 WHEN c_ral.type = 'T' OR c_ral.type = 'TFS' OR c_ral.type = 'R' Then 'PROP' 
																	ELSE NULL
																	END
																	)
			) Treaty_Type_id,
			t.reinsurance_type_id,
			c_ral.Is_Obligatory,
			c_ral.Is_Obligatory cede_premium_only,			
			t.code TreatyCode,
			rt.code ReinsuranceTypeCode,
			(CASE WHEN c_ral.type = 'TX' OR c_ral.type = 'TC' Then 'XOL'
				  WHEN c_ral.type = 'T' OR c_ral.type = 'TFS' OR c_ral.type = 'R' Then 'PROP' 
			 ELSE NULL
			 END
			) TreatyTypeCode,

			t.effective_date,
			t.expiry_date,
			0 premium_calculation_basis_id,
			0 AS Is_VariableQuotaShare,
			ISNULL(c_ral.manually_added,0) ManuallyAddedTreaty
			FROM  Claim_RI_Arrangement_Line c_ral 
			JOIN Claim_RI_Arrangement c_ra ON c_ral.ri_arrangement_id = c_ra.ri_arrangement_id 
			JOIN Treaty t ON t.treaty_id = c_ral.treaty_id
			JOIN Reinsurance_type rt ON rt.reinsurance_type_id = t.reinsurance_type_id 			
			WHERE c_ra.ri_arrangement_id = @ri_arrangement_id 
			AND c_ral.type <> 'FX'
			AND c_ral.type <> 'F'
			AND 
				(
					(@TreatyTypeCode = '')
				or 
					@TreatyTypeCode = (CASE WHEN c_ral.type = 'TX' OR c_ral.type = 'TC' Then 'XOL'
										WHEN c_ral.type = 'T' OR c_ral.type = 'TFS' OR c_ral.type = 'R' Then 'PROP' 
										ELSE NULL
										END
					)
				)
			ORDER BY 
			c_ral.priority, 
			c_ral.default_share_percent 


GO
