SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_RI_Arrangement_Line_Select_RI2007'
GO

CREATE PROCEDURE   spu_Claim_RI_Arrangement_Line_Select_RI2007    
    @claim_id int,    
    @ri_arrangement_id int,    
    @Mode int,    
    @Recovery int=0    
AS    
    Select    
         Case When ral.type = 'F' Then    
             'FAC Prop'    
         When ral.type = 'FX' Then    
             'FAC XOL'    
         When ral.type = 'TX' Then    
             'Treaty XOL'    
         When ral.type = 'PX' Then    
             'Treaty Prop XOL'    
         When ral.type = 'T' Then    
             'Treaty QSH'    
  When ral.type = 'TC' Then    
             'Treaty CAT'    
         When ral.type = 'R' Then    
             'Treaty RET'    
  When ral.type = 'TFS' Then    
           'Treaty Surplus'    
         End [ri_placement],    
         Case    
         WHEN ral.type = 'FX' AND (Select count(*)    
                                               FROM ri_arrangement_line ral1    
                                               WHERE ral1.grouping = (ral.ri_arrangement_line_id)    
                                               GROUP BY ral1.grouping) > 1 Then    
             'Multi Acts'    
         WHEN ral.type = 'F' OR ral.type = 'FX' Then    
             RTRIM(p.shortname)    
         WHEN ral.type = 'R' Then    
              'Retained'    
         ELSE    
             t.description    
         End [ri_name],    
         Case    
         WHEN ral.type = 'FX'  then    
             ral.retained    
         WHEN ral.Type='R' Then    
             1    
         ELSE    
             NULL    
         END Retained,    
         Case    
  when ral.type = 'T' and ISNULL(ral.this_share_percent,0) <=0 Then    
   ISNULL(ral.default_share_percent,0) / 100    
   when ral.type ='R'then  ISNULL(ral.default_share_percent,0) / 100  
   Else    ISNULL(ral.default_share_percent,0) / 100    
  End default_share_percent,    
         Case    
     When ral.is_obligatory='1' then    
      ISNULL(ral.default_share_percent,0) / 100    
    ELSE    
               ISNULL(ral.this_share_percent,0) / 100    
    END this_share_percent,    
         ral.lower_limit lower_limit,    
         ral.line_limit line_limit,    
         ral.sum_insured,    
         Case @Mode    
                  When 0 Then  ral.reserve - ral.this_reserve    
                  Else ral.reserve    
         End as Reserve,    
         ral.this_reserve,    
         Case    
                  When   @Mode=0  Then   (ral.payment) - (ral.this_Payment)    
                  Else  ral.payment    
         End as Payment,    
          Case    
            When   @Recovery=1 Then  ral.This_recovery+ral.This_salvage    
            Else    
            ral.this_payment    
            End As   this_payment,    
          Case    
            When   @Mode=0 Then  (ral.salvage+ral.Recovery) - (ral.This_recovery+ral.This_salvage)    
            Else    
            (ral.salvage+ral.Recovery)    
            End As  RecoveredToDate,    
         Case @Mode    
                  When 0 Then (ral.reserve ) - (ral.payment)    
                  Else (ral.reserve + ral.this_reserve) - (ral.payment + ral.this_Payment)    
         End Balance, 
         ISNULL(ral.claim_incurred_to_date,0)     ,     
         Case    
         When ral.type = 'F' or ral.type='T' or ral.type='TFS' Then    
              ral.agreement_code    
         ELSE    
             ''    
         END Agreement_code,    
   CASE When ISNULL(Party_Insurer.domiciled_for_tax,0) =1 AND ral.type='F' THEN    
     'A'    
   When ISNULL(Party_Insurer.domiciled_for_tax,0)=0 AND ral.type='F' THEN    
     'U'    
   When ral.type='FX' Then    
     'U'    
   ELSE    
     ''    
   END Domiciled_For_Tax,    
         'AU' AddedMode,  
        
         ral.Grouping,    
         party_insurer.is_ri_broker,    
         ral.ri_arrangement_line_id,    
         ral.type,    
         ral.treaty_id,    
         ral.party_cnt,    
         ral.xol_arrangement_id,    
         ral.priority,    
         ral.number_of_lines,    
         ral.Line_limit,    
         xa.layer,    
         xa.catastrophe_code_id,    
         cc.code,    
         xa.ri_model_id,    
         Case When xa.catastrophe_code_id Is Null Then    
             rm.xol_clm_ri_model_id Else rm.xol_cat_ri_model_Id End xol_ri_model_id,    
         Case When xa.catastrophe_code_id Is Null Then    
             rm.xol_clm_limit Else rm.xol_cat_limit End xol_limit,    
 ral.Reserve + ral.this_reserve as Incurred,    
 ral.is_obligatory,    
        ril.cede_premium_only
          
    From    Claim_RI_Arrangement_Line ral    
    Left Join    
            Treaty t    
            On t.treaty_id = ral.treaty_id    
    Left Join    
            Reinsurance_type rt    
  On rt.reinsurance_type_id = t.reinsurance_type_id    
    Left Join    
            Party p    
            On p.party_cnt = ral.party_cnt    
    Left Join    
            claim_xol_arrangement xa    
            On xa.xol_arrangement_id = ral.xol_arrangement_id    
            And xa.claim_id = ral.claim_id    
    Left Join    
            ri_model rm    
            On rm.ri_model_id = xa.ri_model_id    
    Left Join    
            catastrophe_code cc    
            On cc.catastrophe_code_id = xa.catastrophe_code_id    
    Left Join    
            Party_insurer    
            On p.party_cnt = party_insurer.party_cnt    
    LEFT JOIN Claim_ri_arrangement cra    
            on ral.ri_arrangement_id=cra.claim_ri_arrangement_id    
    LEFT JOIN ri_arrangement_line rar    
            on rar.ri_arrangement_line_id=ral.original_ri_arrangement_line_id    
    LEFT Join    
          RI_Model_Line Ril    
          On Ril.treaty_id=ral.treaty_id and Ril.ri_model_line_id=rar.ri_model_Line_id    
    Where   ral.claim_id = @claim_id    
    And     ral.ri_arrangement_id = @ri_arrangement_id    
    order by ral.ri_arrangement_id, ral.priority, Ril.ri_model_line_id, default_share_percent 

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
