SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_TransType_Key'
GO


CREATE PROCEDURE spu_Report_TransType_Key
AS
/**********************************************************************************************************************************
** Created by Jude Killip
** 30/08/2001
** RSA Reports  - Register reports subreport: to provide a key description of the trans type codes
**
**
**********************************************************************************************************************************/
SELECT code, description
FROM transaction_type
WHERE is_deleted = 0

GO

