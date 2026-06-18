SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PartyOther_Update'
GO

CREATE  PROCEDURE spu_PartyOther_Update  
    @party_cnt int,  
       @Active_Indicator bit = null ,   
    @After_Hours_Indicator bit = null,  
    @Priority_Indicator tinyint = null,
    @is_TPA_settle_directly tinyint = null
AS  
  
BEGIN  
  
UPDATE Party_other   
  
Set Active_Indicator=@Active_Indicator,  
After_Hours_Indicator=@After_Hours_Indicator,   
Priority_Indicator= @Priority_Indicator,
is_TPA_settle_directly= @is_TPA_settle_directly      
where party_cnt= @party_cnt 
  
END  


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO