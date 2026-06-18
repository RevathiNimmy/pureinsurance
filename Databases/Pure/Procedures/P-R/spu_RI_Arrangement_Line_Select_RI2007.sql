	SET QUOTED_IDENTIFIER OFF
	GO
	SET ANSI_NULLS ON
	GO
	Execute DDLDropProcedure 'spu_RI_Arrangement_Line_Select_RI2007'
	GO
	CREATE PROCEDURE spu_RI_Arrangement_Line_Select_RI2007    
	@ri_arrangement_id INT    
	AS    
		Select  -- RI Grid fields  
	   Case When ral.type = 'F' Then  
		   'FAC Prop'  
	   When ral.type = 'FX' Then  
		   'FAC XOL'  
	   When ral.type = 'TX' Then  
		   'Treaty XOL'  
	   When ral.type = 'TC' Then  
		   'Treaty CAT'  
	   When ral.type = 'T' Then  
		   'Treaty QSH'  
	   When ral.type = 'R' Then  
		   'Treaty RET'  
	   When ral.type = 'PX' Then  
		   'Treaty Prop XOL'  
	   When ral.type = 'TFS' Then  
		   'Treaty Surplus'  
	   End [ri_placement],  
				Case  
	   When ral.type = 'FX' AND (Select count(*) from ri_arrangement_line ral1  
		where ral1.grouping  
		   = ral.grouping group by ral1.grouping) > 1 Then  
		'Multi Acts'  
	   When ral.type = 'F' OR ral.type = 'FX' Then  
		   RTRIM(p.shortname)  
	  When ral.type = 'R' Then  
		   'Retained'  
	   Else  
		   t.description  
	   End [ri_name],  
  
		Case When ral.type = 'FX' then  
			(SELECT SUM(participation_percent)/100  
	  FROM ri_arrangement_line WHERE  
	  ri_arrangement_id = @ri_arrangement_id  
	  AND  type='FX'  
	  AND  grouping = ral.grouping  
	  AND  retained =1)  
			When ral.type='R' Then  
				'1'  
			ELSE  
				NULL  
			end Retained,  
  
		Case When ral.type='R' Then  
				ral.default_share_percent /100  
			ELSE  
				ral.default_share_percent /100  
			END   default_share_percent,  
  
		Case When ral.type = 'F'  Then  
			ral.this_share_percent    /100  
			   When ral.type='FX' OR ral.type='TX' OR ral.type='R' or ral.type = 'T' or ral.type = 'TC' or ral.type = 'TFS' Then  
					 ral.this_share_percent /100  
			   end this_share_percent,  
  
			ral.lower_limit,  
			ral.line_limit,  
			case when ral.type ='FX' Then  
				 ral.sum_insured- ral.sum_insured * isnull(ral.retained,0) * ISNULL(ral.participation_percent,0)/100  
			else  
		  ral.sum_insured  
			end Sum_insured,  
  
				ral.premium_value,  
				ral.premium_tax,  
				ral.commission_percent / 100,  
				ral.commission_value,  
				isnull(ral.commission_tax,0) commission_tax,  
				Case When ral.type = 'F' or ral.type='T' or ral.type = 'FX' or ral.type = 'TFS' or  ral.type='TX' or ral.type = 'PX' Then  -- Sankar - PN 50348  
							ral.agreement_code  
				ELSE  
							''  
				end Agreement_code,  
		 --E016  
		 CASE When ISNULL(Party_Insurer.domiciled_for_tax,0) =1 AND ral.type='F' THEN  
	  'A'  
		 When ISNULL(Party_Insurer.domiciled_for_tax,0)=0 AND ral.type='F' THEN  
	  'U'  
		 When ral.type='FX' Then  
	  'U'  
		 ELSE  
	  ''  
		 END Domiciled_For_Tax,  
  
				'AU' AddedMode,       --Default 'AU' for automatic populated Rows  
		 ral.grouping,  
		 party_insurer.is_ri_broker,  
				-- Supporting fields       14  
				ral.ri_arrangement_line_id,  
				ral.type,  
				ISNULL(ral.treaty_id,0) treaty_id,  
				ral.party_cnt,  
				ISNULL(ral.priority,0) priority,  
				ISNULL(ral.number_of_lines,0) number_of_lines,  
				ral.premium_percent / 100,  
				ral.is_commission_modified,  
	 ral.is_obligatory,  
	 ISNULL(Ril.cede_premium_only,0) cede_premium_only,  
	 case when ral.type='FX' or ral.type='F' then '' else ISNULL(rt.code,'') end code,  
	 case when ral.type='FX' or ral.type='F' then '' else ISNULL(t.code,'') end 'TreatyCode'  ,  
	   case when ral.manually_added = 1 and type = 'T'then 1 
		when ral.manually_added = 1 and type = 'TX'then 2
		when ISNULL(Ril.Treaty_Type_id,0) = 0 then 
				case when type in ('T','TFS','R') then 1 else 2 end else ISNULL(Ril.Treaty_Type_id,0)  end 'treaty_type_id',  

		CASE WHEN ISNULL(ra.version_id,0) > 1 then 1 else 0 end AS 'is_portfolio_transferred', 
		ISNULL(ri.treaty_premium_type, 0) 'treaty_premium_type',
		ISNULL(ril.premium_calculation_basis_id, 0) 'premium_calculation_basis_id',
		ISNULL(pcb.calculation_factors, '') 'calculation_factors',
		ISNULL(ral.FACPropPremiumPerc,0)/100 'fac_prop_premium_perc',
		ISNULL(ri.fac_premium_type,0) 'fac_premium_type',
		ISNULL(ral.manually_added,0) 'manually_added',
		ISNULL(ral.is_edited,0) 'is_edited',
		ISNULL(ral.is_premium_edited,0) 'is_premium_edited'
		From    RI_Arrangement_Line ral  
		Left Join    Treaty t   On t.treaty_id = ral.treaty_id  
		Left Join    Reinsurance_type rt    On rt.reinsurance_type_id = t.reinsurance_type_id  
		Left Join    Party p    On p.party_cnt = ral.party_cnt  
		Left Join    Party_insurer    On p.party_cnt = party_insurer.party_cnt  
		LEFT JOIN ri_arrangement ra  on ral.ri_arrangement_id=ra.ri_arrangement_id  
		LEFT Join  RI_Model_Line Ril  On Ril.treaty_id=ral.treaty_id and Ril.ri_model_line_id=ral.ri_model_Line_id  
		LEFT JOIN RI_Model RI ON RI.ri_model_id = RiL.ri_model_id
		LEFT JOIN Premium_Calculation_Basis pcb ON pcb.premium_calculation_basis_id = ril.premium_calculation_basis_id
		Where   ral.ri_arrangement_id = @ri_arrangement_id 
	   Order By    ral.Is_Obligatory DESC,  ral.priority ASC,ril.ri_model_line_id, ral.line_limit DESC
   
	GO
	SET QUOTED_IDENTIFIER OFF
	GO
	SET ANSI_NULLS ON
	GO
