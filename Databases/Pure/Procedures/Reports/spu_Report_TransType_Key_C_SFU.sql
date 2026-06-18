
EXECUTE DDLDropProcedure 'spu_Report_TransType_Key_C_SFU'
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

/**********************************************************************************************************************************
** Created by Jude Killip
** Modified P Haynes
** 30/08/2001
** 20/09/2001
**
** RSA Reports  - Register reports subreport: to provide a key description of the trans type codes like c%
**                
**
**********************************************************************************************************************************/
CREATE PROCEDURE spu_Report_TransType_Key_C_SFU  AS

SELECT code, description  
FROM transaction_type 
WHERE is_deleted = 0
and code like 'C%'



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

