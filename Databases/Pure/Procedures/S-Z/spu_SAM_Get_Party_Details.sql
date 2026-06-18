SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Party_Details'
GO

CREATE PROCEDURE spu_SAM_Get_Party_Details  
  
@party_cnt integer  
  
AS  
  
  
SELECT   
  
party.shortname as shortname,   
party.resolved_name as name,  
party.tax_number,   
party.domiciled_for_tax,   
party.tax_exempt,   
party.tax_percentage,  
party.shortname,   
party.resolved_name,   
tp_user_code,   
tp_introducer_code,  
pt.code as party_type_code,   
ae.resolved_name as account_executive,  
c.code as currency_code,  
lt.code as license_type_code,   
ds.code as driver_status_code,   
po.license_number,   
po.date_of_birth,   
po.gender,   
po.reg_number,   
po.active_indicator,   
po.after_hours_indicator,   
po.priority_indicator, 
source.code as source_code  ,
po.is_TPA_settle_directly
  
FROM Party  
  
LEFT OUTER JOIN source ON  
 source.source_id = party.source_id  
  
  
LEFT OUTER JOIN Party ae ON  
 ae.party_cnt = party.consultant_cnt  
  
  
  
  
LEFT OUTER JOIN Party_Net_Data pnd ON  
 pnd.party_cnt = party.party_cnt  
   
  
LEFT OUTER JOIN Party_Other po ON  
 po.party_cnt = party.party_cnt  
  
 LEFT OUTER JOIN Driver_Status ds ON  
  ds.driver_status_id = po.party_status  
  
 LEFT OUTER JOIN License_Type lt ON  
  lt.license_type_id = po.license_type_id    
  
LEFT OUTER JOIN Currency c ON   
 c.currency_id = party.currency_id  
  
INNER JOIN Party_Type pt ON  
 pt.party_type_id = party.party_type_id  
  
WHERE party.Party_Cnt = @party_cnt
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
