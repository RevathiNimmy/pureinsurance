SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

DDLDropPROCEDURE 'spu_SAM_Get_Product_Events'
GO

CREATE PROCEDURE spu_SAM_Get_Product_Events 
@product_id int=0,  
@insurance_file_cnt int=0,  
@type varchar(10)  
AS  
if @product_id=0  
Select @product_id=product_id from insurance_file where insurance_file_cnt=@insurance_file_cnt
IF @type='CLAIM'  
  SELECT p.claim_Event_description_id EventKey,  
         code EventCode,  
         description EventDescription,
         e.is_default IsDefault  
  FROM  
         product_claim_events p JOIN claim_Event_description e  
  ON e.claim_Event_description_id=p.claim_Event_description_id  
  WHERE  product_id=@product_id and e.is_deleted=0
  ORDER BY [description]
ELSE  
  SELECT p.MTA_Event_description_id EventKey,  
  code EventCode,  
  description EventDescription,
  e.is_default IsDefault 
  FROM product_MTA_events p JOIN MTA_Event_description e  
  ON e.MTA_Event_description_id=p.MTA_Event_description_id  
  WHERE product_id=@product_id  and e.is_deleted=0
  ORDER BY [description]


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO