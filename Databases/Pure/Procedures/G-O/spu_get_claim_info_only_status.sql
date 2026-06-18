
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
--Start(Sriram)PN 55475
EXECUTE DDLDropProcedure 'spu_get_claim_info_only_status'  
GO

CREATE PROCEDURE spu_get_claim_info_only_status  
    @Claim_id int  
AS  
  
BEGIN  
  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001        JMK 25/05/2001  Find out whether Claim was Info Only  
--  
--*******************************************************************************************  
SELECT Info_only  
    FROM claim  
    WHERE Claim_id = @claim_id
  
END  
--End(Sriram)PN 55475

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
