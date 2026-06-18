EXECUTE DDLDropProcedure 'spu_PFScheme_saa'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_PFScheme_saa
AS  BEGIN
    SELECT
        s.CompanyNo,
        s.SchemeNo,
        s.SchemeVersion,
        s.Party_Cnt,
        s.DataModelCode,
        s.StartDate,
        s.EndDate,
        s.PaymentMethod_Cnt,
        s.SystemTag,
        s.SchemeName,
        s.SchemeDescription,
        s.QuoteableInd,
        s.QuoteDocid,
        s.BankDocid,
        s.CreditDocid,
        s.InsrMailBoxNo,
        --s.EdiMessageCount,
	(SELECT MAX(ISNULL(PFS.EDIMessageCount,0))+1 --PN12383 Get max edi number 
				FROM PFScheme	pfs,	 --for Provider
				     Party p
				WHERE p.party_cnt = pfs.party_cnt
				AND pfs.schemeno = s.schemeno) 'EdiMessageCount',
        s.ImmediateBankDetails,
        p.shortname,
        p.name,
        s.suspense_account_id,
        s.interest_account_id,
        s.admin_account_id,
        s.protection_account_id,
        s.tax_suspense_account_id,
        s.commission_suspense_account_id,
        s.mediatype_id,
        s.currency_id,
        s.pfscheme_printtype_id,
        s.pfscheme_type_id,
        ISNULL(spread_commission,0) as spread_commission,
        tax_group_id,
        ISNULL(bank_name_mandatory,0) AS bank_name_mandatory,
        ISNULL(bank_address_mandatory,0) AS bank_address_mandatory,
        ISNULL(branch_name_mandatory,0) AS branch_name_mandatory,
        ISNULL(branch_code_mandatory,0) AS branch_code_mandatory,
        bankaccount_id

    FROM
        PFScheme s, party p
    WHERE
        s.party_cnt = p.party_cnt
    ORDER BY
        schemeno ASC,
        schemeversion ASC
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

