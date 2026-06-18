SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_quote_version_deletion_report'
GO
CREATE PROCEDURE spu_get_quote_version_deletion_report
 @DateFrom DATE = NULL,
 @DateTo DATE = NULL
AS
BEGIN

 SET @DateFrom = ISNULL(@DateFrom, CAST(GETDATE() AS DATE))
 SET @DateTo = ISNULL(@DateTo, CAST(GETDATE() AS DATE))

   SELECT
   ISNULL(DL.insurance_ref, '') + '/' + CAST(ISNULL(DL.quote_version,0) AS VARCHAR) AS QuoteNumber,
   ISNULL(PA.agency_account_number, '') AS AgentCode,
   ISNULL(PA.trading_name, '') AS AgentName,
   ISNULL(P.description, '') AS Product,
   ISNULL(PTY.name, '') AS NameOfPolicyholder,
   CONVERT(VARCHAR(10), DL.deletion_date, 101) AS [Date]
 FROM INSURANCE_FILE_DELETE_LOG DL
 LEFT JOIN Party_Agent PA ON PA.party_cnt = DL.lead_agent_cnt
 LEFT JOIN Product P ON P.product_id = DL.product_id
 LEFT JOIN Party PTY ON PTY.party_cnt = DL.insured_cnt
 WHERE DL.status = 1
   AND DL.deletion_date >= @DateFrom
   AND DL.deletion_date < DATEADD(DAY, 1, @DateTo)
 ORDER BY DL.deletion_date

END