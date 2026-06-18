SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_GetAccountFromPartyCnt'
GO


/*************************************************************/
-- RAW 14/02/2003 : ISS2153 : Created
/*************************************************************/
--PN6169 use company _d if necessary
CREATE PROCEDURE spu_ACT_GetAccountFromPartyCnt
    @v_iCompanyId	integer,
    @v_iPartyCnt        integer
AS
-- PN6169 Get the Product Option for multi-branch core accounts
 
DECLARE @Value VARCHAR(20)
SELECT  @Value = Value
FROM    Hidden_options
WHERE   option_number = 16	 

IF ISNULL(@Value, 0) = 0
BEGIN
    SELECT 
          a.account_id,
          RTRIM(at.code)                as accounttype_code,
          l.ledger_id                   as ledger_id,
          RTRIM(l.ledger_short_name)    as ledger_code,
          RTRIM(lt.code)                as ledgertype_code
    FROM  Account a, 
          AccountType at,
          Ledger l,
          LedgerType lt
    WHERE a.ledger_id = l.ledger_id
      AND l.ledgertype_id = lt.ledgertype_id
      AND a.accounttype_id = at.accounttype_id
      AND a.account_key = @v_iPartyCnt
END
ELSE
BEGIN
    SELECT 
          a.account_id,
          RTRIM(at.code)                as accounttype_code,
          l.ledger_id                   as ledger_id,
          RTRIM(l.ledger_short_name)    as ledger_code,
          RTRIM(lt.code)                as ledgertype_code
    FROM  Account a, 
          AccountType at,
          Ledger l,
          LedgerType lt
    WHERE a.ledger_id = l.ledger_id
      AND l.ledgertype_id = lt.ledgertype_id
      AND a.accounttype_id = at.accounttype_id
      AND a.account_key = @v_iPartyCnt
      AND a.company_id = @v_iCompanyId
END
    
GO


