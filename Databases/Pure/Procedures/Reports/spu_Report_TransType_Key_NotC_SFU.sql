
EXECUTE DDLDropProcedure 'spu_Report_TransType_Key_NotC_SFU'
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO

/**********************************************************************************************************************************
** Created by Jude Killip
**
** 08/10/2001
**
** RSA Reports  - Register reports subreport: to provide a key description of trans type codes excluding claims
**
**
**********************************************************************************************************************************/
CREATE PROCEDURE spu_Report_TransType_Key_NotC_SFU  AS

SELECT code, description
FROM transaction_type
WHERE is_deleted = 0
AND code NOT LIKE 'C_%'



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO

