SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_TransType_Key_C'
GO

/**********************************************************************************************************************************
** Created by Jude Killip
** Modified P Haynes
** 30/08/2001
** 20/09/2001
**
** RSA Reports  - Register reports subreport: to provide a key description of the trans type codes like c%
**
**********************************************************************************************************************************/
CREATE PROCEDURE spu_Report_TransType_Key_C
AS

SELECT code, description
FROM transaction_type
WHERE is_deleted = 0
AND code LIKE 'C%'

GO

