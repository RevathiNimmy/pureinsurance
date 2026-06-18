SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_cli_name_claim_chklist'
GO

CREATE PROCEDURE spu_get_cli_name_claim_chklist  
    @Claim_Number varchar(30)  
AS  
  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
--  
--*******************************************************************************************  
    select  client_short_name  
    from    claim  
    where   claim_number = @Claim_Number  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
