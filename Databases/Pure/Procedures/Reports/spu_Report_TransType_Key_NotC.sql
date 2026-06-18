SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_TransType_Key_NotC'
GO


CREATE PROCEDURE spu_Report_TransType_Key_NotC
AS

/**********************************************************************************************************************************
** Created by Jude Killip
**
** 08/10/2001
**
** RSA Reports  - Register reports subreport: to provide a key description of trans type codes excluding claims
**
**
**********************************************************************************************************************************/
SELECT code, description
FROM transaction_type
WHERE is_deleted = 0
AND code NOT LIKE 'C_%'
GO


