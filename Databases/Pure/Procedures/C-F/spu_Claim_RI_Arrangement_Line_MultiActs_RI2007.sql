SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Claim_RI_Arrangement_Line_MultiActs_RI2007'

GO

CREATE PROCEDURE   spu_Claim_RI_Arrangement_Line_MultiActs_RI2007  
    @claim_id int,  
    @ri_arrangement_id int,  
    @Mode int ,  
   @Recovery int=0  
AS  
    Select  
         Case When ral.type = 'F' Then  
             'FAC Prop'  
         When ral.type = 'FX' Then  
             'FAC XOL'  
         When ral.type = 'TX'  Then  
             'Treaty XOL'  
         When ral.type = 'PX'  Then  
             'Treaty Prop XOL'  
         When ral.type = 'TC' Then
	     'Treaty CAT'  
         When ral.type = 'T' Then  
             'Treaty QSH'  
  When ral.type = 'TFS' Then  
             'Treaty Surplus'  
         When ral.type = 'R' Then  
             'Treaty RET'  
         End [ri_placement],  
         Case  
         WHEN ral.type = 'FX' AND (Select count(*)  
                                               FROM ri_arrangement_line ral1  
                                               WHERE ral1.grouping = (ral.ri_arrangement_line_id)  
                                               GROUP BY ral1.grouping) > 1 Then  
             'Multi Acts'  
         WHEN ral.type = 'F' Then  
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
         ISNULL(ral.this_share_percent,0) / 100 default_share_percent,   
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
          When   @Mode=0   Then   ral.payment - ral.this_Payment  
         When   @Mode=0 AND @Recovery=1 Then   ral.payment + (ral.this_Salvage + ral.this_recovery + ral.salvage+ral.Recovery )  
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
            End As   RecoveredToDate,  
         Case @Mode  
                  When 0 Then (ral.reserve ) - (ral.payment )  
                  Else (ral.reserve + ral.this_reserve) - (ral.payment + ral.this_payment)  
         End Balance,
         ISNULL(ral.claim_incurred_to_date,0)  ,  
         Case  
         When ral.type = 'F' or ral.type='T' Then  
              ral.agreement_code  
         ELSE  
             ''  
         END Agreement_code,  
   CASE When ISNULL(Party_insurer.domiciled_for_tax,0) =1 AND ral.Type='F' THEN  
     'A'  
   WHEN ISNULL(Party_insurer.domiciled_for_tax,0)=0 and ral.type='F' THEN  
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
        Case  
            When   @Recovery=1 or @Recovery=0  Then  ral.Reserve + ral.this_reserve  
            End As   Incurred,  
         ral.is_obligatory  ,  
  Ril.cede_premium_only,  
  rt.code  
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
    AND     ral.type<>'FX'  
UNION ALL  
    Select  
             'FAC XOL' [ri_placement],  
             'Multiple Acts' [ri_name],  
              SUM(ral.retained * ral.participation_percent)/100 Retained,  
           SUM(ral.default_share_percent) / 100    default_share_percent,  
           SUM(ral.this_share_percent) / 100,  
           MIN(CONVERT(numeric(14,2),ral.lower_limit)) lower_limit,  
           MIN(CONVERT(numeric(14,2),ral.line_limit)) line_limit,  
           SUM(ral.sum_insured)- SUM(ral.sum_insured) * SUM(ral.retained * ral.participation_percent)/100 ,  
       Case @Mode  
                  When 0 Then  (SUM(ral.reserve) - SUM(ral.this_reserve))  
                  Else SUM(ral.reserve)  
         End as Reserve,  
        Case @Mode  
                  When 0 Then   SUM(ral.this_reserve)  
                  Else SUM(ral.this_reserve)* SUM((1-ral.retained) * ral.participation_percent)/100  
         End as this_Reserve,  
 Case  
              When @Mode=0 Then  (SUM(ral.payment) - SUM(ral.this_Payment))  
              Else  SUM(ral.payment)* SUM((1-ral.retained) * ral.participation_percent)/100  
          End as Payment,  
 Case  
             When   @Recovery=1 Then  sum(ral.This_recovery+ral.This_salvage)  
             Else  
              Case  
              When @Mode=0 Then  sum(ral.this_payment)  
              Else  sum(ral.this_payment)  
              END  
             End As   this_payment,  
       Case  
            When   @Mode=0 Then  SUM(ral.salvage+ral.Recovery) - sum(ral.This_recovery+ral.This_salvage)  
            Else  
            sum((ral.salvage+ral.Recovery)  )  
            End As   RecoveredToDate,  
         Case @Mode  
                  When 0 Then SUM(ral.reserve ) - SUM(ral.payment )  
                  Else (SUM(ral.reserve) + SUM(ral.this_reserve)) - (SUM(ral.payment) + SUM(ral.this_payment))  
         End Balance,
         SUM(ral.claim_incurred_to_date)  ,  
       '' Agreement_code,  
    '' Domiciled_For_Tax,  
         'AU' AddedMode,  
   MIN(ral.Grouping),  
            NULL,  
            MIN(ral.grouping),  
            'FX',  
            NULL,  
            NULL,  
            NULL,  
            NULL,  
            Null,  
            NULL,  
            NULL,  
            NULL,  
            NULL,  
            NULL,  
            NULL,  
            NULL,  
     sum(ral.Reserve + ral.this_reserve) Incurred,  
     0,  
      '' cede_premium_only,  
   'FAX' code  
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
    LEFT JOIN Claim_ri_arrangement cra  
            on ral.ri_arrangement_id=cra.claim_ri_arrangement_id  
    LEFT JOIN ri_arrangement_line rar  
            on rar.ri_arrangement_line_id=ral.original_ri_arrangement_line_id  
    LEFT Join  
          RI_Model_Line Ril  
          On Ril.treaty_id=ral.treaty_id and Ril.ri_model_line_id=rar.ri_model_Line_id  
    Where   ral.claim_id = @claim_id  
    And     ral.ri_arrangement_id = @ri_arrangement_id  
 AND     ral.type='FX'  
    GROUP BY ral.grouping  
    Having ral.Grouping in(select grouping from Claim_RI_Arrangement_Line Where Claim_id=@claim_id and ri_arrangement_id=@ri_arrangement_id  
    group by grouping having count(grouping)>1)  
 UNION ALL  
    Select  
         'FAC XOL' [ri_placement],  
          min(p.Shortname) ,  
         SUM(ral.retained) Retained,  
      SUM(ral.default_share_percent) / 100    default_share_percent,  
       SUM(ral.this_share_percent) / 100,  
      MIN(CONVERT(numeric(14,2),ral.lower_limit)) lower_limit,  
      MIN(CONVERT(numeric(14,2),ral.line_limit)) line_limit,  
  SUM(ral.sum_insured),  
  Case @Mode  
                  When 0 Then  SUM(ral.reserve) - SUM(ral.this_reserve)  
                  Else SUM(ral.reserve)  
         End as Reserve,  
         SUM(ral.this_reserve),  
 Case  
              When @Mode=0 Then   SUM(ral.payment) - SUM(ral.this_Payment)  
                Else  SUM(ral.payment)  
          End as Payment,  
 Case  
             When   @Recovery=1 Then  sum(ral.This_recovery+ral.This_salvage)  
             Else  
              sum(ral.this_payment)  
             End As   this_payment,  
     Case  
            When   @Mode=0 Then  SUM(ral.salvage+ral.Recovery) - sum(ral.This_recovery+ral.This_salvage)  
            Else  
            sum((ral.salvage+ral.Recovery)  )  
            End As   RecoveredToDate,  
       Case @Mode  
                  When 0 Then SUM(ral.reserve )* SUM((1-ral.retained) * ral.participation_percent)/100  - SUM(ral.payment )* SUM((1-ral.retained) * ral.participation_percent)/100  
               Else (SUM(ral.reserve) + SUM(ral.this_reserve)) - (SUM(ral.payment) + SUM(ral.this_payment))  
         End Balance,
         SUM(ral.claim_incurred_to_date)  ,  
      '' Agreement_code,  
   '' Domiciled_For_Tax,  
         'AU' AddedMode,  
  MIN(ral.Grouping),  
            NULL,  
            MIN(ral.grouping),  
            'FX',  
            NULL,  
            NULL,  
            NULL,  
            NULL,  
            Null,  
            NULL,  
            NULL,  
            NULL,  
            NULL,  
            NULL,  
            NULL,  
            NULL,  
             Case @Mode  
        When 0 Then sum(ISNULL(ral.Reserve,0) + ISNULL(ral.salvage,0)+ISNULL(ral.Recovery,0))  
  Else sum((ISNULL(ral.Reserve,0) + ISNULL(ral.this_reserve,0)))  
  End Incurred,  
      0,  
      '' cede_premium_only,  
   'FAX' code  
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
    LEFT JOIN Claim_ri_arrangement cra  
            on ral.ri_arrangement_id=cra.claim_ri_arrangement_id  
    LEFT JOIN ri_arrangement_line rar  
            on rar.ri_arrangement_line_id=ral.original_ri_arrangement_line_id  
    LEFT Join  
          RI_Model_Line Ril  
          On Ril.treaty_id=ral.treaty_id and Ril.ri_model_line_id=rar.ri_model_Line_id  
    Where   ral.claim_id = @claim_id  
    And     ral.ri_arrangement_id = @ri_arrangement_id  
    AND     ral.type='FX'  
    GROUP BY ral.grouping  
    Having ral.Grouping in(select grouping from Claim_RI_Arrangement_Line Where Claim_id=@claim_id and ri_arrangement_id=@ri_arrangement_id  
    group by grouping having count(grouping)=1)  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
