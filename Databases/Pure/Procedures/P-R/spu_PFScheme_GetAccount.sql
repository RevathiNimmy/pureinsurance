EXECUTE DDLDropProcedure 'spu_PFScheme_GetAccount'
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_PFScheme_GetAccount
    @PremiumFinanceCnt int,
    @PremiumFinanceversion int,
    @WhichAccount varchar(20)

AS BEGIN

    IF LEFT(UPPER(@WhichAccount),3) = 'TAX' BEGIN
        SELECT 
            a.account_id,
            a.short_code AS Account, 
            l.ledger_short_name LedgerCode,
            at.code AccountType
        FROM 
            pfScheme s, 
            Account a, 
            pfPremiumFinance pf, 
            Ledger l,
            AccountType at
        WHERE 
            pf.pfprem_finance_cnt = @PremiumFinanceCnt
            AND pf.pfprem_finance_version = @PremiumFinanceVersion
            AND s.tax_suspense_account_id = a.account_id
            AND pf.CompanyNo = s.CompanyNo
            AND pf.SchemeNo = s.SchemeNo
            AND pf.SchemeVersion = s.SchemeVersion
            AND a.ledger_id = l.ledger_id 
            AND a.accounttype_id = at.accounttype_id
    END
    ELSE 
    IF LEFT(UPPER(@WhichAccount),3) = 'FEE' BEGIN
        SELECT 
            a.account_id,
            a.short_code AS Account, 
            l.ledger_short_name LedgerCode,
            at.code AccountType
        FROM 
            pfScheme s, 
            Account a, 
            pfPremiumFinance pf, 
            Ledger l,
            AccountType at
        WHERE 
            pf.pfprem_finance_cnt = @PremiumFinanceCnt
            AND pf.pfprem_finance_version = @PremiumFinanceVersion
            AND s.interest_account_id = a.account_id
            AND pf.CompanyNo = s.CompanyNo
            AND pf.SchemeNo = s.SchemeNo
            AND pf.SchemeVersion = s.SchemeVersion
            AND a.ledger_id = l.ledger_id 
            AND a.accounttype_id = at.accounttype_id

    END
    ELSE 
    IF LEFT(UPPER(@WhichAccount),4) = 'SUSP' BEGIN
        SELECT 
            a.account_id,
            a.short_code AS Account, 
            l.ledger_short_name LedgerCode,
            at.code AccountType
        FROM 
            pfScheme s, 
            Account a, 
            pfPremiumFinance pf, 
            Ledger l,
            AccountType at
        WHERE 
            pf.pfprem_finance_cnt = @PremiumFinanceCnt
            AND pf.pfprem_finance_version = @PremiumFinanceVersion
            AND s.suspense_account_id = a.account_id
            AND pf.CompanyNo = s.CompanyNo
            AND pf.SchemeNo = s.SchemeNo
            AND pf.SchemeVersion = s.SchemeVersion
            AND a.ledger_id = l.ledger_id 
            AND a.accounttype_id = at.accounttype_id

    END
    ELSE 
    IF LEFT(UPPER(@WhichAccount),3) = 'COM' BEGIN
        SELECT 
            a.account_id,
            a.short_code AS Account, 
            l.ledger_short_name LedgerCode,
            at.code AccountType
        FROM 
            pfScheme s, 
            Account a, 
            pfPremiumFinance pf, 
            Ledger l,
            AccountType at
        WHERE 
            pf.pfprem_finance_cnt = @PremiumFinanceCnt
            AND pf.pfprem_finance_version = @PremiumFinanceVersion
            AND s.commission_suspense_account_id = a.account_id
            AND pf.CompanyNo = s.CompanyNo
            AND pf.SchemeNo = s.SchemeNo
            AND pf.SchemeVersion = s.SchemeVersion
            AND a.ledger_id = l.ledger_id 
            AND a.accounttype_id = at.accounttype_id
    END
    ELSE 
    IF LEFT(UPPER(@WhichAccount),4) = 'BANK' BEGIN
        SELECT 
            a.account_id,
            a.short_code AS Account, 
            l.ledger_short_name LedgerCode,
            at.code AccountType
        FROM 
            pfScheme s, 
            Account a, 
            pfPremiumFinance pf, 
            Ledger l,
            AccountType at,
            BankAccount ba
        WHERE 
            pf.pfprem_finance_cnt = @PremiumFinanceCnt
            AND pf.pfprem_finance_version = @PremiumFinanceVersion
            --PSL 18/02/2003 This should be joined on account_id not bankAccount_id
            --AND s.bankaccount_id = a.account_id
            AND ba.account_id = a.account_id
            AND s.bankaccount_id = ba.bankaccount_id
            AND pf.CompanyNo = s.CompanyNo
            AND pf.SchemeNo = s.SchemeNo
            AND pf.SchemeVersion = s.SchemeVersion
            AND a.ledger_id = l.ledger_id 
            AND a.accounttype_id = at.accounttype_id

    END
	 ELSE    
		IF LEFT(UPPER(@WhichAccount),6) = 'SUBCOM' BEGIN    
			SELECT    
				a.account_id,    
				a.short_code AS Account,    
				l.ledger_short_name LedgerCode,    
				at.code AccountType    
			FROM    
				pfScheme s,    
				Account a,    
				pfPremiumFinance pf,    
				Ledger l,    
				AccountType at    
			WHERE    
				pf.pfprem_finance_cnt = @PremiumFinanceCnt    
				AND pf.pfprem_finance_version = @PremiumFinanceVersion    
				AND s.commission_subagent_suspense_account_id = a.account_id    
				AND pf.CompanyNo = s.CompanyNo    
				AND pf.SchemeNo = s.SchemeNo    
				AND pf.SchemeVersion = s.SchemeVersion    
				AND a.ledger_id = l.ledger_id    
				AND a.accounttype_id = at.accounttype_id    
    END
END
GO
    
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
   