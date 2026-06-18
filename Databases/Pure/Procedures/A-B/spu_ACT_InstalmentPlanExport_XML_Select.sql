SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_InstalmentPlanExport_XML_Select'
GO

CREATE PROCEDURE spu_ACT_InstalmentPlanExport_XML_Select    
 @batch_id INT,    
 @pfscheme_type_code VARCHAR(60) = '',    
 @new_batch  SMALLINT = 0,  
 @transactiontype  VARCHAR(60) = NULL    
  
AS    
    
    If (Select export_date From batch Where batch_id = @batch_id) Is Null Begin    
        Update  batch    
        Set     export_date = GetDate()    
        Where   batch_id = @batch_id    
    End Else Begin    
        Update  batch    
        Set     reexport_date = GetDate()    
        Where   batch_id = @batch_id    
    End    

DECLARE @pfscheme_type_id INT

SELECT @pfscheme_type_id = 0
    
SELECT @pfscheme_type_id = isnull(pfscheme_type_id,0) FROM PFScheme_Type WHERE PFScheme_Type.code = @pfscheme_type_code
    
IF (@transactiontype IS NOT NULL)  
SELECT @transactiontype= UPPER(@transactiontype)  
  
IF @new_batch = 1    
BEGIN    
     
  UPDATE pfpremiumfinance 
  SET  batch_id = @batch_id    
  FROM pfpremiumfinance pf
  INNER JOIN 
	pfscheme pfs ON 
	  pfs.SchemeNo = pf.SchemeNo 
	  AND pfs.SchemeVersion = pf.SchemeVersion 
	  AND pfs.CompanyNo = pf.CompanyNo  
   WHERE (batch_id IS NULL OR batch_id = 0)
	AND (pfs.pfscheme_type_id = @pfscheme_type_id OR @pfscheme_type_id = 0)
   AND ((@pfscheme_type_code='TP' AND (((statusind = '040'or   statusind='990') AND (UPPER(PF.TransType) =UPPER(@transactiontype))) OR  (statusind='999' AND @transactiontype='MTC' ))) OR (pfs.pfscheme_type_id<>1 AND statusind = '040'))

END    

DECLARE @sSqlQuery VARCHAR(MAX)  

SELECT @sSqlQuery ='DECLARE @parameters varchar (300)    
SET @parameters = ''batch_id=' +  Cast(@batch_id as varchar(10)) + ' scheme_type_code=' +  @pfscheme_type_code  + ''''  
  
SELECT @sSqlQuery=  @sSqlQuery + ' SELECT  1         As Tag,    
 NULL             As Parent,    
 ''http://www.siriusfs.com/SFI/Export/instalment_plan_Export/20070816''    
				As [EXPORT_HEADER!1!xmlns],    
 ''http://www.w3.org/2001/XMLSchema-instance''    
				As [EXPORT_HEADER!1!xmlns:xsi],    
 ''http://www.siriusfs.com/SFI/Export/instalment_plan_Export/20070816    
Instalment_Plan_Export.xsd''    
				As [EXPORT_HEADER!1!xsi:schemaLocation],    
 GetDate()        As [EXPORT_HEADER!1!date_exported],    
 ''INSTALMENT_PLAN_EXPORT''    
				As [EXPORT_HEADER!1!interface_name],    
 @parameters      As [EXPORT_HEADER!1!parameters_used],    
 ' +  Cast(@batch_id as varchar(10)) + '   As [EXPORT_HEADER!1!batch_id],    
 b.batch_ref      As [EXPORT_HEADER!1!batch_reference],  
 NULL             As [instalment_plan!2!insurance_file_cnt],    
 NULL             As [instalment_plan!2!insurance_ref],    
 NULL             As [instalment_plan!2!cover_start_date],    
 NULL             As [instalment_plan!2!first_instalment_date],    
 NULL             As [instalment_plan!2!next_instalment_date],    
 NULL             As [instalment_plan!2!last_instalment_date],    
 NULL             As [instalment_plan!2!total_premium],    
 NULL             As [instalment_plan!2!amount_financed],    
 NULL             As [instalment_plan!2!instalment_deposit],    
 NULL             As [instalment_plan!2!net_amount],    
 NULL             As [instalment_plan!2!finance_fee],    
 NULL             As [instalment_plan!2!interest_free],    
 NULL             As [instalment_plan!2!interest_rate],    
 NULL             As [instalment_plan!2!interest_cost],    
 NULL             As [instalment_plan!2!tax_cost],    
 NULL             As [instalment_plan!2!number_of_instalments],    
 NULL             As [instalment_plan!2!apr],    
 NULL             As [instalment_plan!2!first_instalment],    
 NULL             As [instalment_plan!2!other_instalment],    
 NULL             As [instalment_plan!2!last_instalment],    
 NULL             As [instalment_plan!2!pay_protection],    
 NULL             As [instalment_plan!2!cost_of_protection],    
 NULL             As [instalment_plan!2!total_cost],    
 NULL             As [instalment_plan!2!contact_name],    
 NULL             As [instalment_plan!2!surname],    
 NULL             As [instalment_plan!2!address1],    
 NULL             As [instalment_plan!2!address2],    
 NULL             As [instalment_plan!2!address3],    
 NULL             As [instalment_plan!2!address4],    
 NULL             As [instalment_plan!2!area_code],    
 NULL             As [instalment_plan!2!postal_code],    
 NULL             As [instalment_plan!2!country_code],    
 NULL             As [instalment_plan!2!phone_number],    
 NULL             As [instalment_plan!2!bank_sort_code],    
 NULL             As [instalment_plan!2!account_number],    
 NULL             As [instalment_plan!2!sepa_bic],    
 NULL             As [instalment_plan!2!sepa_iban],    
 NULL             As [instalment_plan!2!pfrf_id],    
 NULL             As [instalment_plan!2!StartDate],    
 NULL             As [instalment_plan!2!EndDate],    
 NULL             As [instalment_plan!2!BankName],    
 NULL             As [instalment_plan!2!BankBranch],    
 NULL             As [instalment_plan!2!BankAddr1],    
 NULL             As [instalment_plan!2!BankAddr2],    
 NULL             As [instalment_plan!2!BankAddr3],    
 NULL             As [instalment_plan!2!BankTown],    
 NULL             As [instalment_plan!2!BankPCode],    
 NULL             As [instalment_plan!2!BankRegion],    
 NULL             As [instalment_plan!2!BankCountry],    
 NULL             As [instalment_plan!2!BankAreaCode],    
 NULL             As [instalment_plan!2!BankPhoneNo],    
 NULL             As [instalment_plan!2!BankExtension],    
 NULL             As [instalment_plan!2!BankFaxAreaCode],    
 NULL             As [instalment_plan!2!BankFaxNo],    
 NULL             As [instalment_plan!2!BankAccountName],    
 NULL             As [instalment_plan!2!cc_number],    
 NULL             As [instalment_plan!2!cc_expiry_date],    
 NULL             As [instalment_plan!2!cc_start_date],    
 NULL             As [instalment_plan!2!cc_issue],    
 NULL             As [instalment_plan!2!cc_pin],    
 NULL             As [instalment_plan!2!agent_cnt],    
 NULL             As [instalment_plan!2!agent_ref],    
 NULL             As [instalment_plan!2!date_created],    
 NULL             As [instalment_plan!2!date_modified],    
 NULL             As [instalment_plan!2!date_confirmed],    
 NULL             As [instalment_plan!2!date_review],    
 NULL             As [instalment_plan!2!date_laststatement],    
 NULL             As [instalment_plan!2!date_lastgeneration],    
 NULL             As [instalment_plan!2!final_statement_set],    
 NULL             As [instalment_plan!2!no_statements],    
 NULL             As [instalment_plan!2!is_cardholder],    
 NULL             As [instalment_plan!2!cardholder_name],    
 NULL             As [instalment_plan!2!cardholder_address1],    
 NULL             As [instalment_plan!2!cardholder_address2],    
 NULL             As [instalment_plan!2!cardholder_address3],    
 NULL             As [instalment_plan!2!cardholder_address4],    
 NULL             As [instalment_plan!2!cardholder_postcode],    
 NULL             As [instalment_plan!2!card_type],    
 NULL             As [instalment_plan!2!datebankdetailschanged],    
 NULL     AS [instalment_plan!2!scheme_type],    
 NULL             AS [instalment_plan!2!email_address]'  
   
 IF( @pfscheme_type_code='TP')  
BEGIN  
 SELECT @sSqlQuery= @sSqlQuery+ ' , NULL             AS [instalment_plan!2!title],    
         NULL             AS [instalment_plan!2!business_type],   
         NULL             AS [instalment_plan!2!cust_charge_percent],   
         NULL              AS [instalment_plan!2!renewal_date],  
         NULL              AS [instalment_plan!2!ppd],   
         NULL              AS [instalment_plan!2!dob],  
         NULL              AS [instalment_plan!2!new_insurance_ref],  
		 NULL              AS [instalment_plan!2!forename],
		 NULL              AS [instalment_plan!2!dob],
		 NULL              AS [instalment_plan!2!broker],
		 NULL              AS [instalment_plan!2!instalment_plan_ref]'
  
  IF (@transactiontype= 'MTA')    
   SELECT @sSqlQuery= @sSqlQuery+ ', NULL              AS [instalment_plan!2!mta_type],   
            NULL              AS [instalment_plan!2!mta_amount],   
            NULL              AS [instalment_plan!2!entry_type]'  
    
  IF (@transactiontype= 'MTC')    
   SELECT @sSqlQuery= @sSqlQuery+ ',NULL              AS [instalment_plan!2!cancellation_code]'  
END  

SELECT @sSqlQuery= @sSqlQuery+ ' FROM batch b WHERE b.batch_id =  ' +  Cast(@batch_id as varchar(10)) + '  

    UNION ALL    
    
         SELECT 2         As [Tag],    
           1                   As [Parent],    
         ''http://www.siriusfs.com/SFI/Export/instalment_plan_Export/20070816''    
				As [EXPORT_HEADER!1!xmlns],    
         ''http://www.w3.org/2001/XMLSchema-instance''    
				As [EXPORT_HEADER!1!xmlns:xsi],    
         ''http://www.siriusfs.com/SFI/Export/instalment_plan_Export/20070816    
        Instalment_Plan_Export.xsd''    
				As [EXPORT_HEADER!1!xsi:schemaLocation],    
         GetDate()        As [EXPORT_HEADER!1!date_exported],    
         ''INSTALMENT_PLAN_EXPORT''    
				As [EXPORT_HEADER!1!interface_name],    
         @parameters        As [EXPORT_HEADER!1!parameters_used],    
          ' +  Cast(@batch_id as varchar(10)) + '   As [EXPORT_HEADER!1!batch_id],    
         null                     As [EXPORT_HEADER!1!batch_reference],   
         pf.Insurance_File_Cnt   As [instalment_plan!2!insurance_file_cnt],    
         insf.insurance_ref      AS [instalment_plan!2!insurance_ref],    
         insf.cover_start_date   As [instalment_plan!2!cover_start_date],    
         pf.first_instalment_date  As [instalment_plan!2!first_instalment_date],    
         pf.next_instalment_date  As [instalment_plan!2!next_instalment_date],    
         pf.last_instalment_date  As [instalment_plan!2!last_instalment_date],    
 insf.this_premium + isnull(tax_item1.total,0) + isnull(tax_item2.total,0) + isnull(policy_fee.total, 0)           As [instalment_plan!2!total_premium],
         AmountToFinance           As [instalment_plan!2!amount_financed],    
         pf.Deposit            As [instalment_plan!2!instalment_deposit],    
         pf.NetAmount            As [instalment_plan!2!net_amount],    
         pf.financefee           As [instalment_plan!2!finance_fee],    
         pf.InterestFree           As [instalment_plan!2!interest_free],    
 convert(varchar,pf.InterestRate)    As [instalment_plan!2!interest_rate],
         pf.InterestCost           As [instalment_plan!2!interest_cost],    
         pf.tax_cost            As [instalment_plan!2!tax_cost],    
         pf.NoOfInstallments           As [instalment_plan!2!number_of_instalments],    
 convert(varchar,pf.APR)        As [instalment_plan!2!apr],
         pf.FirstInstallment           As [instalment_plan!2!first_instalment],    
         pf.OthInstallments           As [instalment_plan!2!other_instalment],    
         pf.last_instalment          As [instalment_plan!2!last_instalment],    
         pf.PayProtection          As [instalment_plan!2!pay_protection],    
         pf.CostOfProtection           As [instalment_plan!2!cost_of_protection],    
         pf.TotalCost            As [instalment_plan!2!total_cost],    
 CASE p.party_type_id WHEN 4 THEN contact.description ELSE  pf.ClientName END  As [instalment_plan!2!contact_name],
         CASE p.party_type_id WHEN 4 THEN '''' ELSE  p.name END             As [instalment_plan!2!surname],    
         pf.ClientAddr1           As [instalment_plan!2!address1],    
         pf.ClientAddr2           As [instalment_plan!2!address2],    
         pf.ClientAddr3           As [instalment_plan!2!address3],    
         pf.ClientAddr4           As [instalment_plan!2!address4],    
         pf.ClientAreaCode           As [instalment_plan!2!area_code],    
         pf.ClientPCode           As [instalment_plan!2!postal_code],    
         pf.ClientCountry           As [instalment_plan!2!country_code],    
         pf.ClientPhoneNo           As [instalment_plan!2!phone_number],    
         pf.BankSortCode           As [instalment_plan!2!bank_sort_code],    
         pf.BankAccountNo           As [instalment_plan!2!account_number],    
         ISNULL(pf.business_identifier_code,'''')           As [instalment_plan!2!sepa_bic],    
         ISNULL(pf.international_bank_account_number,'''')           As [instalment_plan!2!sepa_iban],    
         pf.pfrf_id            As [instalment_plan!2!pfrf_id],    
         pf.StartDate            As [instalment_plan!2!StartDate],    
         pf.EndDate            As [instalment_plan!2!EndDate],    
         pf.BankName            As [instalment_plan!2!BankName],    
         pf.BankBranch           As [instalment_plan!2!BankBranch],    
         pf.BankAddr1            As [instalment_plan!2!BankAddr1],    
         pf.BankAddr2            As [instalment_plan!2!BankAddr2],    
         pf.BankAddr3            As [instalment_plan!2!BankAddr3],    
         pf.BankTown            As [instalment_plan!2!BankTown],    
         pf.BankPCode            As [instalment_plan!2!BankPCode],    
         pf.BankRegion           As [instalment_plan!2!BankRegion],    
         pf.BankCountry           As [instalment_plan!2!BankCountry],    
         pf.BankAreaCode           As [instalment_plan!2!BankAreaCode],    
         pf.BankPhoneNo           As [instalment_plan!2!BankPhoneNo],    
         pf.BankExtension           As [instalment_plan!2!BankExtension],    
         pf.BankFaxAreaCode           As [instalment_plan!2!BankFaxAreaCode],    
         pf.BankFaxNo            As [instalment_plan!2!BankFaxNo],    
         pf.BankAccountName          As [instalment_plan!2!BankAccountName],    
         pf.cc_number            As [instalment_plan!2!cc_number],    
         pf.cc_expiry_date           As [instalment_plan!2!cc_expiry_date],    
         pf.cc_start_date           As [instalment_plan!2!cc_start_date],    
         pf.cc_issue           As [instalment_plan!2!cc_issue],    
         pf.cc_pin            As [instalment_plan!2!cc_pin],    
         pf.agent_cnt            As [instalment_plan!2!agent_cnt],    
         pf.agent_ref            As [instalment_plan!2!agent_ref],    
         pf.date_created           As [instalment_plan!2!date_created],    
         pf.date_modified           As [instalment_plan!2!date_modified],    
         pf.date_confirmed           As [instalment_plan!2!date_confirmed],    
         pf.date_review           As [instalment_plan!2!date_review],    
 pf.date_laststatement          As [instalment_plan!2!date_laststatement],
 pf.date_lastgeneration         As [instalment_plan!2!date_lastgeneration],
 pf.final_statement_set         As [instalment_plan!2!final_statement_set],
         pf.no_statements           As [instalment_plan!2!no_statements],    
         pf.is_cardholder           As [instalment_plan!2!is_cardholder],    
         pf.cardholder_name           As [instalment_plan!2!cardholder_name],    
 pf.cardholder_address1         As [instalment_plan!2!cardholder_address1],
 pf.cardholder_address2         As [instalment_plan!2!cardholder_address2],
 pf.cardholder_address3         As [instalment_plan!2!cardholder_address3],
 pf.cardholder_address4         As [instalment_plan!2!cardholder_address4],
 pf.cardholder_postcode         As [instalment_plan!2!cardholder_postcode],
         pf.card_type            As [instalment_plan!2!card_type],    
         pf.datebankdetailschanged      As [instalment_plan!2!datebankdetailschanged],    
         pfs.SchemeName                 AS [instalment_plan!2!scheme_type],    
         emailaddress.number            AS [instalment_plan!2!email_address]'  
  
          IF( @pfscheme_type_code='TP')  
          BEGIN  
          SELECT @sSqlQuery= @sSqlQuery+ ' , CASE p.party_type_id WHEN 4 THEN '''' ELSE  ppc.party_title_code END   AS [instalment_plan!2!title],    
                   ''' +  Cast(@transactiontype as varchar(10)) + '''                   AS [instalment_plan!2!business_type],   
                   null                AS [instalment_plan!2!cust_charge_percent],   
                   insf.renewal_date                   AS [instalment_plan!2!renewal_date],   
                   pf.first_instalment_date                   AS [instalment_plan!2!ppd],   
                   NULL                   AS [instalment_plan!2!dob],   
                   NULL                   AS [instalment_plan!2!new_insurance_ref],
				   CASE p.party_type_id WHEN 4 THEN '''' ELSE  ppc.forename END              AS [instalment_plan!2!forename],
		           ''01/01/1900''						AS [instalment_plan!2!dob],
		           AG.resolved_name              AS [instalment_plan!2!broker],
				   pf.pfprem_finance_cnt              AS [instalment_plan!2!instalment_plan_ref]' 
  
          IF (@transactiontype= 'MTA')    
            SELECT @sSqlQuery= @sSqlQuery+ ',CASE WHEN (insf.this_premium + isnull(tax_item1.total,0) + isnull(tax_item2.total,0) + isnull(policy_fee.total, 0) ) >=0 THEN ''AA'' ELSE ''RA''   END    AS [instalment_plan!2!mta_type],   
                    NULL                   AS [instalment_plan!2!mta_amount],   
                    NULL                   AS [instalment_plan!2!entry_type]'  
              
          IF (@transactiontype= 'MTC')    
            SELECT @sSqlQuery= @sSqlQuery+ ',''ZZ''                   AS [instalment_plan!2!cancellation_code]'  
          END  

        SELECT @sSqlQuery= @sSqlQuery+ ' FROM PFPremiumFinance pf    
INNER JOIN 
	insurance_file insf ON 
	  insf.insurance_file_cnt = pf.Insurance_File_Cnt
INNER JOIN 
	Party p ON 
	  pf.clientid = p.party_cnt
INNER JOIN 
	pfscheme pfs ON 
	  pfs.SchemeNo = pf.SchemeNo 
	  AND pfs.SchemeVersion = pf.SchemeVersion 
	  AND pfs.CompanyNo = pf.CompanyNo
LEFT OUTER JOIN (
  SELECT  insurance_file_cnt, SUM(value) total 
      FROM    Tax_Calculation  
      where  risk_cnt IS NULL  
                    AND  transtype in (''TTR'',''TTF'',''TTIF'')    
      GROUP BY insurance_file_cnt )  tax_item1 ON
	tax_item1.insurance_file_cnt = pf.insurance_file_cnt 
LEFT OUTER JOIN (
  SELECT  ifrl.insurance_file_cnt, SUM(value) total
      FROM    Tax_Calculation rt  
      JOIN    insurance_file_risk_link ifrl      ON ifrl.risk_cnt = rt.risk_cnt  
      JOIN    risk r                             ON r.risk_cnt = rt.risk_cnt  
                    WHERE   ifrl.status_flag <> ''U''    
      AND     r.is_risk_selected = 1  
      AND     rt.risk_cnt IS NOT NULL  
                    AND     transtype in (''TTR'',''TTF'',''TTIF'')    
      GROUP BY ifrl.insurance_file_cnt) tax_item2 ON 
	tax_item2.insurance_file_cnt = pf.insurance_file_cnt 
LEFT OUTER JOIN (
  SELECT  insurance_file_cnt, SUM(currency_amount) total 
      FROM    policy_fee_u  
      GROUP BY insurance_file_cnt) policy_fee ON 
	  policy_fee.insurance_file_cnt = pf.Insurance_File_Cnt

LEFT OUTER JOIN (SELECT party_cnt, c.description FROM Party_Contact_Usage pcu  
              INNER JOIN contact c ON c.contact_cnt=pcu.contact_cnt AND contact_type_id=7) contact
ON contact.party_cnt=p.party_cnt	
                 LEFT OUTER JOIN (SELECT party_cnt, c.number FROM Party_Contact_Usage pcu    
                      INNER JOIN contact c ON c.contact_cnt=pcu.contact_cnt AND contact_type_id=3) emailaddress    
                 ON emailaddress.party_cnt=p.party_cnt'  
                     
                 IF( @pfscheme_type_code='TP')  
                  SELECT @sSqlQuery= @sSqlQuery+ ' LEFT OUTER JOIN Party_Personal_Client ppc ON ppc.party_cnt =   p.party_cnt 
												   LEFT OUTER JOIN Party AG ON AG.party_cnt =   insf.lead_agent_cnt'  
  
        SELECT @sSqlQuery= @sSqlQuery+ ' WHERE  isnull(pf.batch_id,0) = ' +  Cast(@batch_id as varchar(10)) +    
		 ' AND (pfs.pfscheme_type_id = ' +  Cast(@pfscheme_type_id as varchar(10)) + ' OR ' +  Cast( @pfscheme_type_id as varchar(10)) + '= 0) '

		IF( @pfscheme_type_code='TP') 
			BEGIN
				SELECT @sSqlQuery= @sSqlQuery+ ' AND (((pf.statusind = ''040'' or   pf.statusind=''990'') AND (UPPER(PF.TransType) =UPPER(''' +  Cast(@transactiontype as varchar(10)) + ''')))' + 
				' OR    (''' +  Cast(@transactiontype as varchar(10)) + '''=''MTC'' AND  pf.statusind=''999''))'
							
			END 
      	ELSE
		 SELECT @sSqlQuery= @sSqlQuery+ ' AND (pf.statusind = ''040'' AND pfs.pfscheme_type_id<>1)'

		 SELECT @sSqlQuery= @sSqlQuery+ '  FOR XML EXPLICIT'
     
  EXEC(@sSqlQuery)  

SET ANSI_NULLS ON    
SET QUOTED_IDENTIFIER ON    

 
GO
