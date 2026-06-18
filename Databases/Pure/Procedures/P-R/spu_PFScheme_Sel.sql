SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

DDLDropProcedure 'spu_PFScheme_sel'
GO
-- Object:  Stored Procedure dbo.spu_PFScheme_sel 
-- Script Date: 10/15/2003 2:38:36 PM 
-- 06/07/2004 Add new business_code_mandatory options.
CREATE PROCEDURE spu_PFScheme_sel
    @CompanyNo int,  
    @SchemeNo int,  
    @SchemeVersion int = NULL,
    @SchemeType tinyint = 1  
AS BEGIN  
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
 ISNULL((SELECT MAX(ISNULL(PFS.EDIMessageCount,0))+1 --PN12383 Get max edi number  

    FROM PFScheme pfs,  --for Provider  
         Party p  
    WHERE p.party_cnt = pfs.party_cnt  
    AND pfs.schemeno = @SchemeNo),0) 'EdiMessageCount',  
        s.ImmediateBankDetails,  
        p.shortname,  
        p.name,  
        s.mediatype_id,  
        s.currency_id,  
        s.pfscheme_printtype_id,  
        ISNULL(s.spread_commission,0) as spread_commission,  
        s.suspense_account_id,  
        s.interest_account_id,  
        s.admin_account_id,  
        s.protection_account_id,  
        s.tax_group_id,  
        s.tax_suspense_account_id,  
        s.commission_suspense_account_id,  
        s.pfscheme_type_id,  
        ISNULL(s.bank_name_mandatory,0) AS bank_name_mandatory,  
        ISNULL(s.bank_address_mandatory,0) AS bank_address_mandatory,  
        ISNULL(s.branch_name_mandatory,0) AS branch_name_mandatory,  
        ISNULL(s.branch_code_mandatory,0) AS branch_code_mandatory,  
        s.bankaccount_id,  
        s.confirmationdocid,  
        ISNULL(s.ri_suspense_account_id,0) as ri_suspense_account_id,  
        ISNULL(s.spread_ri,0) as spread_ri,  
        ISNULL(s.spread_taxes,0) as spread_taxes,  
        ISNULL(s.deposit_as_instalment,0) as deposit_as_instalment,  
        ISNULL(s.deposit_on_other_media_type,0) as deposit_on_other_media_type,  
        ISNULL(s.agent_ref_mandatory,0) as agent_ref_mandatory,  
        s.pf_message,  
 ISNULL(s.business_code_mandatory,0) as business_code_mandatory,  
 s.receipt_difference_option as receipt_difference,  
 provider_website,  
 plbrokerid,  
 clbrokerid,  
 provider_username,  
 provider_password,  
 provider_timeout,  
 provider_brokerid,  
 financial_institution_code,  
 direct_debit_supplier_name,  
 direct_debit_supplier_id,  
 remitter,  
 processing_days,  
        s.allow_client_fees,  
        s.rates_are_for_information_only,  --'(RC) PLICO 9-10  
--Start(Saurabh Agrawal) Tech Spec PGR005 Automated Emails
	s.ColNotDocID,
	s.ColNotNumDays,
	ISNULL(s.is_plan_reference_editable,0) as is_plan_reference_editable,
	ISNULL(s.business_identifier_code_mandatory,0) AS business_identifier_code_mandatory,
		ISNULL(s.international_bank_account_number_mandatory,0) AS international_bank_account_number_mandatory,
		 ISNULL(spread_subagent_commission,0) as spread_subagent_commission,  
        s.commission_subagent_suspense_account_id
--End(Saurabh Agrawal) Tech Spec PGR005 Automated Emails
	
    FROM  
        PFScheme s LEFT JOIN party p on s.party_cnt = p.party_cnt  
    WHERE  
        CompanyNo = @CompanyNo  
        AND SchemeNo = @SchemeNo  
        AND (@SchemeVersion IS NULL OR SchemeVersion = @SchemeVersion)  
        AND (s.scheme_type = @SchemeType OR s.scheme_type IS NULL)  
    ORDER BY  
        schemeno ASC,  
        schemeversion ASC  
END  

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

