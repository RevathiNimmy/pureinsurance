SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_RI_Arrangement_Line_MultiActs_RI2007'
GO


CREATE PROCEDURE spu_RI_Arrangement_Line_MultiActs_RI2007    
@ri_arrangement_id INT    
    
AS    
    
    (  Select  -- RI Grid fields    
   Case When ral.type = 'F' Then    
       'FAC Prop'    
   When ral.type = 'FX' Then    
       'FAC XOL'    
   When ral.type = 'TX' Then  --E005 Part2    
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
    --JOIN ri_arrangement_line ral2    
    --ON ral1.grouping = ral2.grouping    
    where ral1.grouping    
    = (ral.ri_arrangement_line_id) group by ral1.grouping) > 1 Then    
    'Multi Acts'    
   When ral.type = 'F' OR ral.type = 'FX' Then    
       RTRIM(p.shortname)    
  When ral.type = 'R' Then    
       'Retained'    
   Else    
       t.description    
   End [ri_name],    
    
    Case When ral.type = 'FX' then    
            ral.retained    
        When ral.type='R' Then    
            '1'    
        ELSE    
            NULL    
        end Retained,    
    
    Case When ral.type='R' Then    
             ral.default_share_percent / 100    
        ELSE    
            ral.default_share_percent / 100    
        END   default_share_percent,    
    
      Case When ral.type = 'F'  Then    
            ral.this_share_percent / 100    
           When ral.type='FX' OR ral.type='TX' OR ral.type='R' or ral.type = 'T' Or ral.type = 'TFS'Then    
                 ral.this_share_percent / 100    
           end this_share_percent,    
    
            ral.lower_limit,    
            ral.line_limit,    
            ral.sum_insured,    
            ral.premium_value,    
            ral.premium_tax,    
            ral.commission_percent / 100,    
            ral.commission_value,    
            ral.commission_tax,    
            Case When ral.type = 'F' or ral.type='T' Or ral.type = 'TFS' or ral.type ='TX' or ral.type ='PX'  Then    
                        ral.agreement_code    
            ELSE    
                        ''    
            end Agreement_code,    
     --E016    
     CASE When ISNULL(Party_Insurer.domiciled_for_tax,0)=1 AND ral.type='F' THEN    
  'A'    
     WHEN ISNULL(Party_Insurer.domiciled_for_tax,0)= 0 AND ral.type='F' THEN    
  'U'    
     WHEN ral.type='FX' THEN    
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
     --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)    
     ral.is_commission_modified,    
     ral.is_obligatory,    
     --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)    
     ISNULL(Ril.cede_premium_only,0) cede_premium_only,          --Added By E005    
     case when ral.type='FX' or ral.type='F' then '' else ISNULL(rt.code,'') end code,  --E005 Part 2  
	 case when ral.type='FX' or ral.type='F' then '' else ISNULL(t.code,'') end 'TreatyCode',
  case when ral.manually_added = 1 and type = 'T'then 1 
    when ral.manually_added = 1 and type = 'TX'then 2
	when ISNULL(Ril.Treaty_Type_id,0) = 0 then 
			case when type in ('T','TFS','R') then 1 else 2 end else ISNULL(Ril.Treaty_Type_id,0)  end 'treaty_type_id',  
	CASE WHEN ISNULL(ra.version_id,0) > 1 then 1 else 0 end AS 'is_portfolio_transferred',
    ISNULL(RI.treaty_premium_type, 0) 'treaty_premium_type',
    ISNULL(Ril.premium_calculation_basis_id, 0) 'premium_calculation_basis_id',
    ISNULL(pcb.calculation_factors, '') 'calculation_factors',
	ISNULL(ral.FACPropPremiumPerc,0)/100 'fac_prop_premium_perc',
	ISNULL(RI.fac_premium_type,0) 'fac_premium_type',
	ISNULL(ral.manually_added,0) 'manually_added',
	ISNULL(ral.is_edited,0) 'is_edited',
	ISNULL(ral.is_premium_edited,0) 'is_premium_edited'
    From    RI_Arrangement_Line ral    
    Left Join  Treaty t    On t.treaty_id = ral.treaty_id    
    Left Join    Reinsurance_type rt    On rt.reinsurance_type_id = t.reinsurance_type_id    
    Left Join    Party p    On p.party_cnt = ral.party_cnt    
    Left Join    Party_insurer    On p.party_cnt = party_insurer.party_cnt    
    LEFT JOIN ri_arrangement ra  on ral.ri_arrangement_id=ra.ri_arrangement_id    
    LEFT Join  --E005  
	RI_Model_Line Ril    
     On Ril.treaty_id=ral.treaty_id and Ril.ri_model_line_id=ral.ri_model_line_id
	LEFT JOIN RI_Model RI ON RI.ri_model_id = RiL.ri_model_id
    LEFT JOIN Premium_Calculation_Basis pcb ON pcb.premium_calculation_basis_id = ril.premium_calculation_basis_id
    Where   ral.ri_arrangement_id = @ri_arrangement_id AND ral.type<>'FX' )    
    
UNION ALL    
    
(    
    Select    
        'FAC XOL' [ri_placement],    
        RTRIM(MIN(p.shortname)) [ri_name],    
        (SELECT SUM(participation_percent)/100    
  FROM ri_arrangement_line WHERE    
  ri_arrangement_id = @ri_arrangement_id    
  AND  type='FX'    
  AND  grouping = ral.grouping    
  AND  retained =1),    
        SUM(ral.default_share_percent) / 100 default_share_percent,    
        NULL this_share_percent,    
        MIN(convert(numeric(14,2),ral.lower_limit)) lower_limit,    
        MIN(convert(numeric(14,2),ral.line_limit)) line_limit,    
         SUM(ral.sum_insured)- SUM(ral.sum_insured) * SUM( ISNULL(ral.retained,0) * ISNULL(ral.participation_percent,0))/100 SI,    
    
        SUM(ral.premium_value),    
        SUM(ral.premium_tax),    
        SUM(ral.commission_percent) / 100,    
        SUM(ral.commission_value),    
        SUM(ral.commission_tax),    
   '' Agreement_code,    
 --E016    
 'U' Domiciled_For_Tax,    
        'AU' AddedMode,       --Default 'AU' for automatic populated Rows    
        MIN(ral.grouping),    
        '',    
        -- Supporting fields       14    
        MIN(ral.grouping),    
        'FX',    
        NULL,    
        '',    
        1,    
        1,    
        SUM(ral.premium_percent) / 100,    
 --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)    
        ''  ,    
  0,    
 --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)    
 MIN(Ril.cede_premium_only),          --Added By E005    
 'FAX',     --E005 Part 2  
 '' , 2 'treaty_type_id' ,
  1 'is_portfolio_transferred',
  ISNULL(RI.treaty_premium_type, 0) 'treaty_premium_type',
  ISNULL(Ril.premium_calculation_basis_id, 0) 'premium_calculation_basis_id',
  ISNULL(pcb.calculation_factors, '') 'calculation_factors',
  ISNULL(ral.FACPropPremiumPerc,0)/100 'fac_prop_premium_perc',
  ISNULL(RI.fac_premium_type,0) 'fac_premium_type',
  ISNULL(ral.manually_added,0) 'manually_added',
  0 'is_edited',
  0 'is_premium_edited'
    From    RI_Arrangement_Line ral    
    Left Join    Treaty t    On t.treaty_id = ral.treaty_id    
    Left Join    Reinsurance_type rt On rt.reinsurance_type_id = t.reinsurance_type_id    
    Left Join    Party p    On p.party_cnt = ral.party_cnt    
    Left Join    Party_insurer    On p.party_cnt = party_insurer.party_cnt    
    LEFT JOIN ri_arrangement ra on ral.ri_arrangement_id=ra.ri_arrangement_id    
    LEFT Join  --E005    
     RI_Model_Line Ril    
     On Ril.treaty_id=ral.treaty_id and Ril.ri_model_line_id=ral.ri_model_line_id
	LEFT JOIN RI_Model RI ON RI.ri_model_id = RiL.ri_model_id
    LEFT JOIN Premium_Calculation_Basis pcb ON pcb.premium_calculation_basis_id = ril.premium_calculation_basis_id
    Where   ral.ri_arrangement_id = @ri_arrangement_id and ral.type='FX'    
        Group By Grouping , Ril.Treaty_Type_id,RI.treaty_premium_type,ril.premium_calculation_basis_id ,pcb.calculation_factors,ral.FACPropPremiumPerc,RI.fac_premium_type,ral.manually_added
        Having Grouping in(select grouping from RI_Arrangement_Line    
        group by grouping having count(grouping)=1)    
)    
    
UNION ALL    
    
(    
    Select    
        'FAC XOL' [ri_placement],    
        'Multiple Acts' [ri_name],    
        (SELECT SUM(participation_percent)/100    
  FROM ri_arrangement_line WHERE    
  ri_arrangement_id = @ri_arrangement_id    
  AND  type='FX'    
  AND  grouping = ral.grouping    
  AND  retained =1),    
        SUM(ral.default_share_percent) / 100 default_share_percent,    
        NULL this_share_percent,    
        MIN(convert(numeric(14,2),ral.lower_limit)) lower_limit,    
        MIN(convert(numeric(14,2),ral.line_limit)) line_limit,    
        Round(SUM(ral.sum_insured),0),    
        SUM(ral.premium_value),    
        SUM(ral.premium_tax),    
        SUM(ral.commission_percent) / 100,    
        SUM(ral.commission_value),    
        SUM(ral.commission_tax),    
        '' Agreement_code,    
 --E016    
 'U' Domiciled_For_Tax,    
        'AU' AddedMode,       --Default 'AU' for automatic populated Rows    
        MIN(ral.grouping),    
        '',    
        -- Supporting fields       14    
        MIN(ral.grouping),    
        'FX',    
        NULL,    
        '',    
        1,    
        1,    
        SUM(ral.premium_percent) / 100,    
 --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)    
        ''  ,    
  0,    
 --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)    
 MIN(Ril.cede_premium_only),          --Added By E005    
 'FAX',  --E005 Part 2    
 '', 2 'treaty_type_id',
 1 'is_portfolio_transferred',
    ISNULL(RI.treaty_premium_type, 0) 'treaty_premium_type',
    ISNULL(ril.premium_calculation_basis_id, 0) 'premium_calculation_basis_id',
    ISNULL(pcb.calculation_factors, '') 'calculation_factors',
	ISNULL(ral.FACPropPremiumPerc,0)/100 'fac_prop_premium_perc',
	ISNULL(ISNULL(RI.fac_premium_type,0),0) 'fac_premium_type',
	ISNULL(ral.manually_added,0) 'manually_added',
	0 'is_edited',
	0 'is_premium_edited'
    From    RI_Arrangement_Line ral    
    Left Join    Treaty t    On t.treaty_id = ral.treaty_id    
    Left Join    Reinsurance_type rt    On rt.reinsurance_type_id = t.reinsurance_type_id    
    Left Join    Party p    On p.party_cnt = ral.party_cnt    
    Left Join    Party_insurer    On p.party_cnt = party_insurer.party_cnt    
    LEFT JOIN ri_arrangement ra   on ral.ri_arrangement_id=ra.ri_arrangement_id    
    LEFT Join  --E005    
      RI_Model_Line Ril  On Ril.treaty_id=ral.treaty_id and Ril.ri_model_line_id=ral.ri_model_line_id   
    LEFT JOIN RI_Model RI ON RI.ri_model_id = Ril.ri_model_id
    LEFT JOIN Premium_Calculation_Basis pcb ON pcb.premium_calculation_basis_id = ril.premium_calculation_basis_id
                Where   ral.ri_arrangement_id = @ri_arrangement_id and ral.type='FX'    
                Group By Grouping, Ril.Treaty_Type_id,RI.treaty_premium_type,ril.premium_calculation_basis_id ,pcb.calculation_factors,ral.FACPropPremiumPerc,ISNULL(RI.fac_premium_type,0),ISNULL(ral.manually_added,0)
) 
  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


 
