SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_GLExport_XML_Select'
GO


CREATE PROCEDURE spu_ACT_GLExport_XML_Select  
    @batch_id int  

AS  
DECLARE @BatchStatus_Batch_Exported INT
-- get required id constants
SELECT @BatchStatus_Batch_Exported = batchstatus_id FROM BatchStatus Where code = 'BE'

	--****************************************
	-- create class of business -> account link table
	CREATE TABLE #class_of_business_account_link (
		account_id int NULL , 
		account_short_code varchar(20) NULL , 
		class_of_business_code varchar(20) NULL , 
		class_of_business_id int NULL)
		
	INSERT INTO #class_of_business_account_link
		SELECT account_id, short_code, REPLACE(short_code, 'CLMRES', ''), null
		FROM account
		WHERE SHORT_CODE LIKE 'CLMRES%'

	UPDATE #class_of_business_account_link
		SET #class_of_business_account_link.class_of_business_id = class_of_business.class_of_business_id 
	FROM #class_of_business_account_link 
	
		INNER JOIN class_of_business ON 
			class_of_business.code =#class_of_business_account_link.class_of_business_code 

	--****************************************
	--****************************************
  
    -- Update batch table  
    If (Select export_date From batch WITH (NOLOCK) Where batch_id = @batch_id) Is Null Begin  
        Update  batch  
        Set     export_date = GetDate()  
        Where   batch_id = @batch_id  
    End Else Begin  
        Update  batch  
        Set     reexport_date = GetDate()  
        Where   batch_id = @batch_id  
    End    
  
  UPDATE Batch
  SET batchstatus_id = @BatchStatus_Batch_Exported
  WHERE batch_id = @batch_id
  
--********************************************************************
-- FIRST TIER - EXPORT HEADER INFO ONLY
--********************************************************************

    -- Define header portion of XML  
    -- Note:  
    --     This is done for maintainability: it is simpler to define data  
    --     columns against the data rather than empty columns in the header.  
    Select  1                   As Tag,  
            Null                  As Parent,  
            'http://www.siriusfs.com/SFI/Export/GL_Export/20051005'  
                                              As [EXPORT_HEADER!1!xmlns],  
            'http://www.w3.org/2001/XMLSchema-instance'  
                                              As [EXPORT_HEADER!1!xmlns:xsi],  
            'http://www.siriusfs.com/SFI/Export/GL_Export/20051005  GL_EXPORT.xsd'                                As [EXPORT_HEADER!1!xsi:schemaLocation],  
            GetDate()                         As [EXPORT_HEADER!1!date_exported],  
            'GL_EXPORT'                       As [EXPORT_HEADER!1!interface_name],  
            @batch_id                         As [EXPORT_HEADER!1!batch_id],  
            b.batch_ref                       As [EXPORT_HEADER!1!batch_reference],  
            b.total_transactions              As [EXPORT_HEADER!1!total_transactions],  
            b.total_amount                    As [EXPORT_HEADER!1!total_amount],  
			Null As [TRANSACTION!2!transdetail_id], 
			Null As [TRANSACTION!2!document_id], 
			Null As [TRANSACTION!2!document_date], 
			Null As [TRANSACTION!2!document_sequence], 
			Null As [TRANSACTION!2!document_ref], 
			Null As [TRANSACTION!2!documenttype_code], 
			Null As [TRANSACTION!2!company_code], 
			Null As [TRANSACTION!2!sub_branch_code], 
			Null As [TRANSACTION!2!account_code], 
			Null As [TRANSACTION!2!insurance_holder_shortname], 
			Null As [TRANSACTION!2!period_id], 
			Null As [TRANSACTION!2!period_name], 
			Null As [TRANSACTION!2!transaction_currency_code], 
			Null As [TRANSACTION!2!transaction_amount], 
			Null As [TRANSACTION!2!transaction_base_rate], 
			Null As [TRANSACTION!2!base_currency_code], 
			Null As [TRANSACTION!2!base_amount], 
			Null As [TRANSACTION!2!transaction_account_rate], 
			Null As [TRANSACTION!2!account_currency_code], 
			Null As [TRANSACTION!2!account_amount], 
			Null As [TRANSACTION!2!transaction_system_rate], 
			Null As [TRANSACTION!2!system_currency_code], 
			Null As [TRANSACTION!2!system_amount], 
			Null As [TRANSACTION!2!outstanding_transaction_amount], 
			Null As [TRANSACTION!2!outstanding_base_amount], 
			Null As [TRANSACTION!2!outstanding_account_amount], 
			Null As [TRANSACTION!2!outstanding_system_amount], 
			Null As [TRANSACTION!2!insurance_file_cnt], 
			Null As [TRANSACTION!2!department], 
			Null As [TRANSACTION!2!purchase_order_no], 
			Null As [TRANSACTION!2!postingstatus_code], 
			Null As [TRANSACTION!2!loss_code], 
			Null As [TRANSACTION!2!write_off_reason_code], 
			Null As [TRANSACTION!2!purchase_invoice_no], 
			Null As [TRANSACTION!2!transaction_comment], 
			Null As [TRANSACTION!2!created_by_username], 
			Null As [TRANSACTION!2!transdetail_type_code], 
			Null As [TRANSACTION!2!spare], 
			Null As [TRANSACTION!2!created_date], 
			Null As [TRANSACTION!2!cover_start_date], 
			Null As [TRANSACTION!2!expiry_date], 
			Null As [TRANSACTION!2!effective_date],
			Null As [TRANSACTION!2!due_date],
			Null As [TRANSACTION!2!authorised_date], 
			Null As [TRANSACTION!2!loss_date], 
			Null As [TRANSACTION!2!reason], 
			Null As [TRANSACTION!2!ledger_code], 
			Null As [TRANSACTION!2!auditset_code], 
			Null As [TRANSACTION!2!agent_shortname], 
			Null As [TRANSACTION!2!insurance_file_ref], 
			Null As [TRANSACTION!2!reference], 
			Null As [TRANSACTION!2!product_code], 
			Null As [TRANSACTION!2!underwriting_year_code], 
			Null As [TRANSACTION!2!business_type_code], 
			Null As [TRANSACTION!2!reference_1], 
			Null As [TRANSACTION!2!document_comment], 
			Null As [TRANSACTION!2!account_handler_shortname], 
			Null As [TRANSACTION!2!mediatype_code], 
			Null As [TRANSACTION!2!correspondence_address_1], 
			Null As [TRANSACTION!2!correspondence_address_2], 
			Null As [TRANSACTION!2!correspondence_address_3], 
			Null As [TRANSACTION!2!correspondence_address_4], 
			Null As [TRANSACTION!2!correspondence_postal_code], 
			Null As [TRANSACTION!2!correspondence_country], 
			Null As [TRANSACTION!2!payee_name], 
			Null As [TRANSACTION!2!payee_branch_code], 
			Null As [TRANSACTION!2!payee_account_code], 
			Null As [TRANSACTION!2!sepa_payee_bic],
			Null As [TRANSACTION!2!sepa_payee_iban],
			Null As [TRANSACTION!2!balance_type],  
			Null As [TRANSACTION!2!cashdeposit_number], 
			NULL AS [TRANSACTION!2!term],
			NULL AS [TRANSACTION!2!installment],
			Null AS [CLAIM!3!id],
			Null AS [CLAIM!3!claim_number],
			Null AS [CLAIM!3!underwriting_year_code],
			Null as [PERILTYPE!4!id],
			Null as [PERILTYPE!4!code],
			Null as [PERILTYPE!4!description],
			Null as [RESERVETYPE!5!id],
			Null As [RESERVETYPE!5!total], 
			Null As [RESERVETYPE!5!description], 
			Null As [RESERVETYPE!5!name], 
			Null As [RESERVETYPE!5!is_excess]

    From    batch b WITH (NOLOCK)
    Where   b.batch_id = @batch_id  
  
UNION

--********************************************************************
-- SECOND TIER - TRANSACTION INFO ONLY
--********************************************************************

    -- Define data portion of XML  
    Select  2                                 As Tag,  
            1                                 As Parent,  
            'http://www.siriusfs.com/SFI/Export/GL_Export/20051005'  
                                              As [EXPORT_HEADER!1!xmlns],  
            'http://www.w3.org/2001/XMLSchema-instance'  
                                              As [EXPORT_HEADER!1!xmlns:xsi],  
            'http://www.siriusfs.com/SFI/Export/GL_Export/20051005  GL_EXPORT.xsd'                                As [EXPORT_HEADER!1!xsi:schemaLocation],  
            GetDate()                         As [EXPORT_HEADER!1!date_exported],  
            'GL_EXPORT'                       As [EXPORT_HEADER!1!interface_name],  
            @batch_id                         As [EXPORT_HEADER!1!batch_id],  
            b.batch_ref                       As [EXPORT_HEADER!1!batch_reference],  
            b.total_transactions              As [EXPORT_HEADER!1!total_transactions],  
            b.total_amount                    As [EXPORT_HEADER!1!total_amount],  
            td.transdetail_id                 As [TRANSACTION!2!transdetail_id],  
            d.document_id                     As [TRANSACTION!2!document_id],  
            d.document_date                   As [TRANSACTION!2!document_date],  
            td.document_sequence              As [TRANSACTION!2!document_sequence],  
            RTrim(d.document_ref)             As [TRANSACTION!2!document_ref],  
            RTrim(dt.code)                    As [TRANSACTION!2!documenttype_code],  
            RTrim(c.code)                     As [TRANSACTION!2!company_code],  
            RTrim(sb.code)                    As [TRANSACTION!2!sub_branch_code],  
            RTrim(a.short_code)               As [TRANSACTION!2!account_code],  
            RTrim(ipi.shortname)              As [TRANSACTION!2!insurance_holder_shortname],  
            p.period_id                       As [TRANSACTION!2!period_id],  
            RTrim(p.period_name)              As [TRANSACTION!2!period_name],  
            RTrim(ct.code)                    As [TRANSACTION!2!transaction_currency_code],  
            td.currency_amount                As [TRANSACTION!2!transaction_amount],  
            td.currency_base_xrate            As [TRANSACTION!2!transaction_base_rate],  
            RTrim(tdcb.code)                  As [TRANSACTION!2!base_currency_code],  
            td.amount                         As [TRANSACTION!2!base_amount],  
			convert(varchar,td.account_base_xrate)   As [TRANSACTION!2!transaction_account_rate],  
            RTrim(tdca.code)                  As [TRANSACTION!2!account_currency_code],  
            td.account_amount                 As [TRANSACTION!2!account_amount],  
			convert(varchar,td.system_base_xrate) As [TRANSACTION!2!transaction_system_rate],  
            RTrim(tdcs.code)                  As [TRANSACTION!2!system_currency_code],  
            td.system_amount                  As [TRANSACTION!2!system_amount],  
            td.outstanding_currency_amount    As [TRANSACTION!2!outstanding_transaction_amount],  
            td.outstanding_amount			  As [TRANSACTION!2!outstanding_base_amount],  
            td.outstanding_account_amount     As [TRANSACTION!2!outstanding_account_amount],  
            td.outstanding_system_amount      As [TRANSACTION!2!outstanding_system_amount],  
            i.insurance_file_cnt              As [TRANSACTION!2!insurance_file_cnt],  
            RTrim(tdd.code)                   As [TRANSACTION!2!department],  
			td.purchase_order_no              As [TRANSACTION!2!purchase_order_no],  
            RTrim(tdps.code)                  As [TRANSACTION!2!postingstatus_code],  
            dc.claim_number                   As [TRANSACTION!2!loss_code],  
            RTrim(dwor.description)           As [TRANSACTION!2!write_off_reason_code],  
            td.purchase_invoice_no            As [TRANSACTION!2!purchase_invoice_no],  
            RTrim(td.comment)                 As [TRANSACTION!2!transaction_comment],  
            RTrim(u.username)                 As [TRANSACTION!2!created_by_username],  
            RTrim(tdt.code)                   As [TRANSACTION!2!transdetail_type_code],  
            RTrim(td.spare)                   As [TRANSACTION!2!spare],  
            td.accounting_date                As [TRANSACTION!2!created_date],  
            i.cover_start_date                As [TRANSACTION!2!cover_start_date],  
            i.expiry_date                     As [TRANSACTION!2!expiry_date],  
            i.inception_date_tpi              As [TRANSACTION!2!effective_date],
			td.due_date 					  As [TRANSACTION!2!due_date],				
            d.authorised_date                 As [TRANSACTION!2!authorised_date],  
            dc.loss_from_date                 As [TRANSACTION!2!loss_date],  
            RTrim(d.reason)                   As [TRANSACTION!2!reason],  
            RTrim(al.ledger_short_name)       As [TRANSACTION!2!ledger_code],  
            Null                              As [TRANSACTION!2!auditset_code],  
            RTrim(ipa.shortname)              As [TRANSACTION!2!agent_shortname],  
            RTrim(i.insurance_ref)            As [TRANSACTION!2!insurance_file_ref],  
            Null                              As [TRANSACTION!2!reference],  
            RTrim(ip.code)                    As [TRANSACTION!2!product_code],  
            RTrim(tduy.code)                  As [TRANSACTION!2!underwriting_year_code],  
            RTrim(ibt.code)                   As [TRANSACTION!2!business_type_code],  
            dcp.payeecomments                 As [TRANSACTION!2!reference_1],  
            RTrim(d.comment)                  As [TRANSACTION!2!document_comment],  
            RTrim(iph.shortname)              As [TRANSACTION!2!account_handler_shortname],  
            Case When dt.code In ('SPY', 'SRP') Then RTrim(tdmt.code)  
                 When dt.code = 'CLP' Then RTrim(dcpmt.code) End  
                                              As [TRANSACTION!2!mediatype_code],  
  
            Case When dt.code In ('SPY', 'SRP') Then RTrim(tdcli.address1) Else RTrim(apa.address1) End  
                                              As [TRANSACTION!2!correspondence_address_1],  
            Case When dt.code In ('SPY', 'SRP') Then RTrim(tdcli.address2) Else RTrim(apa.address2) End  
                                              As [TRANSACTION!2!correspondence_address_2],  
            Case When dt.code In ('SPY', 'SRP') Then RTrim(tdcli.address3) Else RTrim(apa.address3) End  
                                              As [TRANSACTION!2!correspondence_address_3],  
            Case When dt.code In ('SPY', 'SRP') Then RTrim(tdcli.address4) Else RTrim(apa.address4) End  
                                              As [TRANSACTION!2!correspondence_address_4],  
            Case When dt.code In ('SPY', 'SRP') Then RTrim(tdcli.postal_code) Else RTrim(apa.postal_code) End  
                                              As [TRANSACTION!2!correspondence_postal_code],  
            Case When dt.code In ('SPY', 'SRP') Then RTrim(tdclic.description) Else RTrim(apc.description) End  
                                              As [TRANSACTION!2!correspondence_country],  
            Case When dt.code In ('SPY', 'SRP') Then RTrim(tdcli.payment_name)  
                 When dt.code = 'CLP' Then RTrim(dcp.payeename)
				 When dt.code = 'CLR' Then RTrim(dcr.payeename) End  
                                              As [TRANSACTION!2!payee_name],  
            Case When dt.code In ('SPY', 'SRP') Then RTrim(tdcli.payment_branch_code)  
                 When dt.code = 'CLP' Then RTrim(dcp.payeesortcode)
				 When dt.code = 'CLR' Then RTrim(dcr.payeesortcode) End  
                                              As [TRANSACTION!2!payee_branch_code],
            Case When dt.code In ('SPY', 'SRP') Then RTrim(tdcli.payment_account_code)  
                 When dt.code = 'CLP' Then RTrim(dcp.payeeaccountno) 
				  When dt.code = 'CLR' Then RTrim(dcr.payeeaccountno) End  
                                              As [TRANSACTION!2!payee_account_code],
            Case When dt.code In ('SPY') Then RTrim(ISNULL(tdcli.business_identifier_code,''))  
                 When dt.code = 'CLP' Then RTrim(ISNULL(dcp.business_identifier_code,'')) End  
                                              As [TRANSACTION!2!sepa_payee_bic], 
            Case When dt.code In ('SPY') Then RTrim(ISNULL(tdcli.international_bank_account_number,''))  
                 When dt.code = 'CLP' Then RTrim(ISNULL(dcp.international_bank_account_number,'')) End  
                                              As [TRANSACTION!2!sepa_payee_iban],
            i.balance_type              As [TRANSACTION!2!balance_type],  
			CDT.CashDeposit_Ref As [TRANSACTION!2!cashdeposit_number], 
			CASE WHEN i.insurance_file_cnt IS NOT NULL 
				THEN
					CASE WHEN i.insurance_file_cnt < (
													ISNULL((
												  		SELECT TOP 1 
													  		insurance_file_cnt 
												  		FROM 
													  		insurance_file i_f
												  		WHERE 
													  		i_f.insurance_file_type_id=2 
													  		AND i_f.insurance_file_status_id IS NULL
													  		AND i_f.policy_version>1 
													  		AND i_f.insurance_folder_cnt=i.insurance_folder_cnt),2147483647))
						THEN
							CASE WHEN	
													(ISNULL((
												  		SELECT TOP 1 
													  		insurance_file_cnt 
												  		FROM 
													  		insurance_file i_f
												  		WHERE 
													  		i_f.insurance_file_type_id=2 
													  		AND i_f.insurance_file_status_id IS NULL
													  		AND i_f.policy_version>1 
													  		AND i_f.insurance_folder_cnt=i.insurance_folder_cnt),0)<>0)
												AND (dt.code IN ('CLO', 'CLA','CLP'))
				 			THEN 'REN'
							ELSE 'NB'							
							END

				 	ELSE 'REN'
					END 
				ELSE NULL
			END AS [TRANSACTION!2!term],

			-- For older records when pfinstaments_id was not set in transdetail table
			CASE WHEN td.pfinstalments_id IS NOT NULL
				THEN
					pf1.instalmentNumber
				ELSE
					pf2.instalmentNumber
			END
				 AS [TRANSACTION!2!installment],
			Null AS [CLAIM!3!id],
			Null AS [CLAIM!3!claim_number],
			Null AS [CLAIM!3!underwriting_year_code],
  			Null as [PERILTYPE!4!id],
  			Null as [PERILTYPE!4!code],
			Null as [PERILTYPE!4!description],
    		Null as [RESERVETYPE!5!id],
   			Null As [RESERVETYPE!5!total], 
  			null As [RESERVETYPE!5!description], 
			null As [RESERVETYPE!5!name], 
			null As [RESERVETYPE!5!is_excess]

    From    batch b WITH (NOLOCK)
    Join    transdetail td  WITH (NOLOCK)
            On td.batch_id = b.batch_id  
    Join    document d WITH (NOLOCK) 
            On d.document_id = td.document_id  
    Join    documenttype dt WITH (NOLOCK) 
            On dt.documenttype_id = d.documenttype_id  
    Join    company c WITH (NOLOCK)
            On c.company_id = td.company_id  
    Join    sub_branch sb WITH (NOLOCK)  
            On sb.sub_branch_id = td.sub_branch_id  
    Join    account a WITH (NOLOCK)
            On a.account_id = td.account_id  
    Join    period p WITH (NOLOCK)  
            On p.period_id = td.period_id  
    Join    currency ct WITH (NOLOCK) -- currency - transaction  
            On ct.currency_id = td.currency_id  
    Join    pmuser u WITH (NOLOCK)
            On u.user_id = td.operator_id  
  
    -- Joins from account  
    Left Join   party ap WITH (NOLOCK)  
                On ap.party_cnt = a.account_key  
    Left Join   party_address_usage apau WITH (NOLOCK) 
                On apau.party_cnt = ap.party_cnt  
                And apau.address_usage_type_id = 4  
    Left Join   address apa WITH (NOLOCK)
                On apa.address_cnt = apau.address_cnt  
    Left Join   country apc WITH (NOLOCK)
                On apc.country_id = apa.country_id  
    Left Join   ledger al WITH (NOLOCK)
                On al.ledger_id = a.ledger_id  
  
    -- Simple Left joins from transdetail  
    Left Join   department tdd WITH (NOLOCK)  
                On tdd.department_id = td.department_id  
    Left Join   currency tdcb WITH (NOLOCK)  
                On tdcb.currency_id = td.amount_currency_id  
    Left Join   currency tdca WITH (NOLOCK) 
                On tdca.currency_id = td.account_currency_id  
    Left Join   currency tdcs WITH (NOLOCK) 
                On tdcs.currency_id = td.system_currency_id  
    Left Join   postingstatus tdps WITH (NOLOCK) 
                On tdps.postingstatus_id = td.postingstatus_id  
    Left Join   cashlistitem tdcli WITH (NOLOCK)
                On tdcli.transdetail_id = td.transdetail_id  
    Left Join   country tdclic WITH (NOLOCK)  
                On tdclic.country_id = tdcli.address_country  
    Left Join   mediatype tdmt WITH (NOLOCK) 
                On tdmt.mediatype_id = tdcli.mediatype_id  
    Left Join   transdetail_type tdt WITH (NOLOCK)
                On tdt.transdetail_type_id = td.transdetail_type_id  
    Left Join   underwriting_year tduy WITH (NOLOCK)  
                On tduy.underwriting_year_id = td.underwriting_year_id  
  
    -- Simple left joins from document  
    Left Join   write_off_reason dwor WITH (NOLOCK) 
                On dwor.write_off_reason_id = d.write_off_reason_id  
    Left Join   claim_payment dcp WITH (NOLOCK)  
                On dcp.document_id = d.document_id    and dcp.claim_payment_id=dcp.base_claim_payment_id 
	Left Join   claim_receipt dcr WITH (NOLOCK)  
                On dcr.document_id = d.document_id  and dcr.claim_receipt_id=dcr.base_claim_receipt_id
    Left Join   claim dc WITH (NOLOCK) 
                On dc.claim_id = dcp.claim_id  
    Left Join   mediatype dcpmt WITH (NOLOCK) 
	ON dcpmt.mediatype_id =  dcp.payeemediatype  
  
    -- Left join out to insurance file related data  
    Left Join   insurance_file i WITH (NOLOCK)  
                On i.insurance_file_cnt = d.insurance_file_cnt  
    Left Join   party ipi WITH (NOLOCK)-- party - insured  
                On ipi.party_cnt = i.insured_cnt  
    Left Join   party ipa WITH (NOLOCK)-- party - agent  
                On ipa.party_cnt = i.lead_agent_cnt  
    Left Join   party iph WITH (NOLOCK)-- party - account handler  
                On iph.party_cnt = i.account_handler_cnt  
    Left Join   product ip WITH (NOLOCK) 
                On ip.product_id = i.product_id  
    Left Join   business_type ibt WITH (NOLOCK) 
                On ibt.business_type_id = i.business_type_id  
	LEFT JOIN CashDeposit_Policy_Link CPL
		ON CPL.Insurance_File_Cnt=i.insurance_file_cnt
	LEFT JOIN CashDeposit CDT
		ON CDT.CashDeposit_ID=CPL.CashDeposit_ID
	LEFT OUTER JOIN PFInstalments pf1
		ON pf1.pfinstalments_id=td.pfinstalments_id	
	LEFT OUTER JOIN PFInstalments pf2
		ON pf2.pftransaction_id=td.transdetail_id
  
    -- Only the current batch  
    Where   b.batch_id = @batch_id  
  
UNION


--********************************************************************
-- THIRD TIER - CLAIM INFO ONLY FOR TRANSACTIONS AGAINST RESERVE ACCOUNTS
--********************************************************************

    -- Define data portion of XML  
    Select  3                                 As Tag,  
            2                                 As Parent,  
			null As [EXPORT_HEADER!1!xmlns],  
			null As [EXPORT_HEADER!1!xmlns:xsi],  
			null As [EXPORT_HEADER!1!xsi:schemaLocation],  
			null As [EXPORT_HEADER!1!date_exported],  
			null As [EXPORT_HEADER!1!interface_name],  
            @batch_id                         As [EXPORT_HEADER!1!batch_id],  
            null                       As [EXPORT_HEADER!1!batch_reference],  
            null              As [EXPORT_HEADER!1!total_transactions],  
            null                 As [EXPORT_HEADER!1!total_amount],  
            transdetail.transdetail_id                 As [TRANSACTION!2!transdetail_id],  
			Null As [TRANSACTION!2!document_id], 
			Null As [TRANSACTION!2!document_date], 
			Null As [TRANSACTION!2!document_sequence], 
			Null As [TRANSACTION!2!document_ref], 
			Null As [TRANSACTION!2!documenttype_code], 
			Null As [TRANSACTION!2!company_code], 
			Null As [TRANSACTION!2!sub_branch_code], 
			Null As [TRANSACTION!2!account_code], 
			Null As [TRANSACTION!2!insurance_holder_shortname], 
			Null As [TRANSACTION!2!period_id], 
			Null As [TRANSACTION!2!period_name], 
			Null As [TRANSACTION!2!transaction_currency_code], 
			Null As [TRANSACTION!2!transaction_amount], 
			Null As [TRANSACTION!2!transaction_base_rate], 
			Null As [TRANSACTION!2!base_currency_code], 
			Null As [TRANSACTION!2!base_amount], 
			Null As [TRANSACTION!2!transaction_account_rate], 
			Null As [TRANSACTION!2!account_currency_code], 
			Null As [TRANSACTION!2!account_amount], 
			Null As [TRANSACTION!2!transaction_system_rate], 
			Null As [TRANSACTION!2!system_currency_code], 
			Null As [TRANSACTION!2!system_amount], 
			Null As [TRANSACTION!2!outstanding_transaction_amount], 
			Null As [TRANSACTION!2!outstanding_base_amount], 
			Null As [TRANSACTION!2!outstanding_account_amount], 
			Null As [TRANSACTION!2!outstanding_system_amount], 
			Null As [TRANSACTION!2!insurance_file_cnt], 
			Null As [TRANSACTION!2!department], 
			Null As [TRANSACTION!2!purchase_order_no], 
			Null As [TRANSACTION!2!postingstatus_code], 
			Null As [TRANSACTION!2!loss_code], 
			Null As [TRANSACTION!2!write_off_reason_code], 
			Null As [TRANSACTION!2!purchase_invoice_no], 
			Null As [TRANSACTION!2!transaction_comment], 
			Null As [TRANSACTION!2!created_by_username], 
			Null As [TRANSACTION!2!transdetail_type_code], 
			Null As [TRANSACTION!2!spare], 
			Null As [TRANSACTION!2!created_date], 
			Null As [TRANSACTION!2!cover_start_date], 
			Null As [TRANSACTION!2!expiry_date], 
			Null As [TRANSACTION!2!effective_date], 
			Null As [TRANSACTION!2!due_date],
			Null As [TRANSACTION!2!authorised_date], 
			Null As [TRANSACTION!2!loss_date], 
			Null As [TRANSACTION!2!reason], 
			Null As [TRANSACTION!2!ledger_code], 
			Null As [TRANSACTION!2!auditset_code], 
			Null As [TRANSACTION!2!agent_shortname], 
			Null As [TRANSACTION!2!insurance_file_ref], 
			Null As [TRANSACTION!2!reference], 
			Null As [TRANSACTION!2!product_code], 
			Null As [TRANSACTION!2!underwriting_year_code], 
			Null As [TRANSACTION!2!business_type_code], 
			Null As [TRANSACTION!2!reference_1], 
			Null As [TRANSACTION!2!document_comment], 
			Null As [TRANSACTION!2!account_handler_shortname], 
			Null As [TRANSACTION!2!mediatype_code], 
			Null As [TRANSACTION!2!correspondence_address_1], 
			Null As [TRANSACTION!2!correspondence_address_2], 
			Null As [TRANSACTION!2!correspondence_address_3], 
			Null As [TRANSACTION!2!correspondence_address_4], 
			Null As [TRANSACTION!2!correspondence_postal_code], 
			Null As [TRANSACTION!2!correspondence_country], 
			Null As [TRANSACTION!2!payee_name], 
			Null As [TRANSACTION!2!payee_branch_code], 
			Null As [TRANSACTION!2!payee_account_code],
			Null As [TRANSACTION!2!sepa_payee_bic],
			Null As [TRANSACTION!2!sepa_payee_iban],			 
			Null As [TRANSACTION!2!balance_type],  
			Null As [TRANSACTION!2!cashdeposit_number], 
			NULL AS [TRANSACTION!2!term],
			NULL AS [TRANSACTION!2!installment],
			claim.claim_id AS [CLAIM!3!id],
			MIN(claim.claim_number) AS [CLAIM!3!claim_number],
			MIN(underwriting_year.code) AS [CLAIM!3!underwriting_year_code],
			Null as [PERILTYPE!4!id],
			Null as [PERILTYPE!4!code],
			Null as [PERILTYPE!4!description],
			Null as [RESERVETYPE!5!id],
			Null As [RESERVETYPE!5!total],
			null  As [RESERVETYPE!5!_description], 
			null  As [RESERVETYPE!5!name], 
			null  As [RESERVETYPE!5!is_excess] 

  From    batch b WITH (NOLOCK)  

    	INNER Join    transdetail WITH (NOLOCK) ON
		transdetail.batch_id = b.batch_id  

	INNER JOIN #class_of_business_account_link WITH (NOLOCK) ON 
		transdetail.account_id = #class_of_business_account_link.account_id
	
	INNER JOIN document WITH (NOLOCK) ON 
		transdetail.document_id = document.document_id
 
	INNER JOIN stats_folder WITH (NOLOCK) ON 
		stats_folder.document_ref = document.document_ref

	inner join claim WITH (NOLOCK) ON 
		stats_folder.loss_id = claim.claim_id

		LEFT JOIN underwriting_year WITH (NOLOCK) ON
			claim.underwriting_year_id = underwriting_year.underwriting_year_id
	
	inner join claim_peril WITH (NOLOCK) ON 
		claim.claim_id =claim_peril.claim_id
	
	inner join peril_type WITH (NOLOCK) ON 
		claim_peril.peril_type_id = peril_type.peril_type_id
	AND peril_type.class_of_business_id = #class_of_business_account_link.class_of_business_id
    Left join documenttype dt on document.documenttype_id=dt.documenttype_id
    -- Only the current batch  
    WHERE   b.batch_id = @batch_id  
    AND dt.code in ('CLO', 'CLA','CLP')

   GROUP BY 
            b.batch_id,
            transdetail.transdetail_id,
	    claim.claim_id 

UNION




--********************************************************************
-- FOURTH TIER - PERILTYPE INFO ONLY
--********************************************************************

    -- Define data portion of XML  
    Select  4                                 As Tag,  
            3                                 As Parent,  
			null As [EXPORT_HEADER!1!xmlns],  
			null As [EXPORT_HEADER!1!xmlns:xsi],  
			null As [EXPORT_HEADER!1!xsi:schemaLocation],  
			null As [EXPORT_HEADER!1!date_exported],  
			null As [EXPORT_HEADER!1!interface_name],  
            @batch_id                         As [EXPORT_HEADER!1!batch_id],  
            null                       As [EXPORT_HEADER!1!batch_reference],  
            null              As [EXPORT_HEADER!1!total_transactions],  
            null                 As [EXPORT_HEADER!1!total_amount],  
            transdetail.transdetail_id                 As [TRANSACTION!2!transdetail_id],  
			Null As [TRANSACTION!2!document_id], 
			Null As [TRANSACTION!2!document_date], 
			Null As [TRANSACTION!2!document_sequence], 
			Null As [TRANSACTION!2!document_ref], 
			Null As [TRANSACTION!2!documenttype_code], 
			Null As [TRANSACTION!2!company_code], 
			Null As [TRANSACTION!2!sub_branch_code], 
			Null As [TRANSACTION!2!account_code], 
			Null As [TRANSACTION!2!insurance_holder_shortname], 
			Null As [TRANSACTION!2!period_id], 
			Null As [TRANSACTION!2!period_name], 
			Null As [TRANSACTION!2!transaction_currency_code], 
			Null As [TRANSACTION!2!transaction_amount], 
			Null As [TRANSACTION!2!transaction_base_rate], 
			Null As [TRANSACTION!2!base_currency_code], 
			Null As [TRANSACTION!2!base_amount], 
			Null As [TRANSACTION!2!transaction_account_rate], 
			Null As [TRANSACTION!2!account_currency_code], 
			Null As [TRANSACTION!2!account_amount], 
			Null As [TRANSACTION!2!transaction_system_rate], 
			Null As [TRANSACTION!2!system_currency_code], 
			Null As [TRANSACTION!2!system_amount], 
			Null As [TRANSACTION!2!outstanding_transaction_amount], 
			Null As [TRANSACTION!2!outstanding_base_amount], 
			Null As [TRANSACTION!2!outstanding_account_amount], 
			Null As [TRANSACTION!2!outstanding_system_amount], 
			Null As [TRANSACTION!2!insurance_file_cnt], 
			Null As [TRANSACTION!2!department], 
			Null As [TRANSACTION!2!purchase_order_no], 
			Null As [TRANSACTION!2!postingstatus_code], 
			Null As [TRANSACTION!2!loss_code], 
			Null As [TRANSACTION!2!write_off_reason_code], 
			Null As [TRANSACTION!2!purchase_invoice_no], 
			Null As [TRANSACTION!2!transaction_comment], 
			Null As [TRANSACTION!2!created_by_username], 
			Null As [TRANSACTION!2!transdetail_type_code], 
			Null As [TRANSACTION!2!spare], 
			Null As [TRANSACTION!2!created_date], 
			Null As [TRANSACTION!2!cover_start_date], 
			Null As [TRANSACTION!2!expiry_date], 
			Null As [TRANSACTION!2!effective_date],
			Null As [TRANSACTION!2!due_date],
			Null As [TRANSACTION!2!authorised_date], 
			Null As [TRANSACTION!2!loss_date], 
			Null As [TRANSACTION!2!reason], 
			Null As [TRANSACTION!2!ledger_code], 
			Null As [TRANSACTION!2!auditset_code], 
			Null As [TRANSACTION!2!agent_shortname], 
			Null As [TRANSACTION!2!insurance_file_ref], 
			Null As [TRANSACTION!2!reference], 
			Null As [TRANSACTION!2!product_code], 
			Null As [TRANSACTION!2!underwriting_year_code], 
			Null As [TRANSACTION!2!business_type_code], 
			Null As [TRANSACTION!2!reference_1], 
			Null As [TRANSACTION!2!document_comment], 
			Null As [TRANSACTION!2!account_handler_shortname], 
			Null As [TRANSACTION!2!mediatype_code], 
			Null As [TRANSACTION!2!correspondence_address_1], 
			Null As [TRANSACTION!2!correspondence_address_2], 
			Null As [TRANSACTION!2!correspondence_address_3], 
			Null As [TRANSACTION!2!correspondence_address_4], 
			Null As [TRANSACTION!2!correspondence_postal_code], 
			Null As [TRANSACTION!2!correspondence_country], 
			Null As [TRANSACTION!2!payee_name], 
			Null As [TRANSACTION!2!payee_branch_code], 
			Null As [TRANSACTION!2!payee_account_code],
			Null As [TRANSACTION!2!sepa_payee_bic],
			Null As [TRANSACTION!2!sepa_payee_iban],
			Null As [TRANSACTION!2!balance_type],  
			Null As [TRANSACTION!2!cashdeposit_number], 
			NULL AS [TRANSACTION!2!term],
			NULL AS [TRANSACTION!2!installment],
			claim.claim_id AS [CLAIM!3!id],
			Null AS [CLAIM!3!claim_number],
			Null AS [CLAIM!3!underwriting_year_code],
			Peril_Type.peril_type_id as [PERILTYPE!4!id],
			MIN(Peril_Type.code) as [PERILTYPE!4!code],
			MIN(Peril_Type.description) as [PERILTYPE!4!description],
			Null as [RESERVETYPE!5!id],
			Null As [RESERVETYPE!5!total],
			null  As [RESERVETYPE!5!_description], 
			null  As [RESERVETYPE!5!name], 
			null  As [RESERVETYPE!5!is_excess] 

  From    batch b WITH (NOLOCK)

    	INNER Join    transdetail WITH (NOLOCK) ON
		transdetail.batch_id = b.batch_id  

	INNER JOIN #class_of_business_account_link WITH (NOLOCK) ON 
		transdetail.account_id = #class_of_business_account_link.account_id
	
	INNER JOIN document WITH (NOLOCK) ON 
		transdetail.document_id = document.document_id
 
	INNER JOIN stats_folder WITH (NOLOCK) ON 
		stats_folder.document_ref = document.document_ref

	inner join claim WITH (NOLOCK) ON 
		stats_folder.loss_id = claim.claim_id
	
	inner join claim_peril WITH (NOLOCK) ON 
		claim.claim_id =claim_peril.claim_id
	
	inner join peril_type WITH (NOLOCK) ON 
		claim_peril.peril_type_id = peril_type.peril_type_id
	AND peril_type.class_of_business_id = #class_of_business_account_link.class_of_business_id

	Left join documenttype dt WITH (NOLOCK) on document.documenttype_id=dt.documenttype_id
    -- Only the current batch  
    WHERE   b.batch_id = @batch_id  
    AND dt.code in ('CLO', 'CLA','CLP')

   GROUP BY 
            b.batch_id,
            transdetail.transdetail_id,
	    claim.claim_id,
            peril_type.peril_type_id 
	
UNION
--********************************************************************
-- FIFTH TIER - RESERVETYPE INFO ONLY
--********************************************************************

    -- Define data portion of XML  
    Select  5                                As Tag,  
            4                                 As Parent,  
			null As [EXPORT_HEADER!1!xmlns],  
			null As [EXPORT_HEADER!1!xmlns:xsi],  
			null As [EXPORT_HEADER!1!xsi:schemaLocation],  
			null As [EXPORT_HEADER!1!date_exported],  
			null As [EXPORT_HEADER!1!interface_name],  
            @batch_id                         As [EXPORT_HEADER!1!batch_id],  
            null                       As [EXPORT_HEADER!1!batch_reference],  
            null              As [EXPORT_HEADER!1!total_transactions],  
            null                 As [EXPORT_HEADER!1!total_amount],  
            transdetail.transdetail_id                 As [TRANSACTION!2!transdetail_id],  
			Null As [TRANSACTION!2!document_id], 
			Null As [TRANSACTION!2!document_date], 
			Null As [TRANSACTION!2!document_sequence], 
			Null As [TRANSACTION!2!document_ref], 
			Null As [TRANSACTION!2!documenttype_code], 
			Null As [TRANSACTION!2!company_code], 
			Null As [TRANSACTION!2!sub_branch_code], 
			Null As [TRANSACTION!2!account_code], 
			Null As [TRANSACTION!2!insurance_holder_shortname], 
			Null As [TRANSACTION!2!period_id], 
			Null As [TRANSACTION!2!period_name], 
			Null As [TRANSACTION!2!transaction_currency_code], 
			Null As [TRANSACTION!2!transaction_amount], 
			Null As [TRANSACTION!2!transaction_base_rate], 
			Null As [TRANSACTION!2!base_currency_code], 
			Null As [TRANSACTION!2!base_amount], 
			Null As [TRANSACTION!2!transaction_account_rate], 
			Null As [TRANSACTION!2!account_currency_code], 
			Null As [TRANSACTION!2!account_amount], 
			Null As [TRANSACTION!2!transaction_system_rate], 
			Null As [TRANSACTION!2!system_currency_code], 
			Null As [TRANSACTION!2!system_amount], 
			Null As [TRANSACTION!2!outstanding_transaction_amount], 
			Null As [TRANSACTION!2!outstanding_base_amount], 
			Null As [TRANSACTION!2!outstanding_account_amount], 
			Null As [TRANSACTION!2!outstanding_system_amount], 
			Null As [TRANSACTION!2!insurance_file_cnt], 
			Null As [TRANSACTION!2!department], 
			Null As [TRANSACTION!2!purchase_order_no], 
			Null As [TRANSACTION!2!postingstatus_code], 
			Null As [TRANSACTION!2!loss_code], 
			Null As [TRANSACTION!2!write_off_reason_code], 
			Null As [TRANSACTION!2!purchase_invoice_no], 
			Null As [TRANSACTION!2!transaction_comment], 
			Null As [TRANSACTION!2!created_by_username], 
			Null As [TRANSACTION!2!transdetail_type_code], 
			Null As [TRANSACTION!2!spare], 
			Null As [TRANSACTION!2!created_date], 
			Null As [TRANSACTION!2!cover_start_date], 
			Null As [TRANSACTION!2!expiry_date], 
			Null As [TRANSACTION!2!effective_date], 
			Null As [TRANSACTION!2!due_date],
			Null As [TRANSACTION!2!authorised_date], 
			Null As [TRANSACTION!2!loss_date], 
			Null As [TRANSACTION!2!reason], 
			Null As [TRANSACTION!2!ledger_code], 
			Null As [TRANSACTION!2!auditset_code], 
			Null As [TRANSACTION!2!agent_shortname], 
			Null As [TRANSACTION!2!insurance_file_ref], 
			Null As [TRANSACTION!2!reference], 
			Null As [TRANSACTION!2!product_code], 
			Null As [TRANSACTION!2!underwriting_year_code], 
			Null As [TRANSACTION!2!business_type_code], 
			Null As [TRANSACTION!2!reference_1], 
			Null As [TRANSACTION!2!document_comment], 
			Null As [TRANSACTION!2!account_handler_shortname], 
			Null As [TRANSACTION!2!mediatype_code], 
			Null As [TRANSACTION!2!correspondence_address_1], 
			Null As [TRANSACTION!2!correspondence_address_2], 
			Null As [TRANSACTION!2!correspondence_address_3], 
			Null As [TRANSACTION!2!correspondence_address_4], 
			Null As [TRANSACTION!2!correspondence_postal_code], 
			Null As [TRANSACTION!2!correspondence_country], 
			Null As [TRANSACTION!2!payee_name], 
			Null As [TRANSACTION!2!payee_branch_code], 
			Null As [TRANSACTION!2!payee_account_code],
			Null As [TRANSACTION!2!sepa_payee_bic],
			Null As [TRANSACTION!2!sepa_payee_iban],
			Null As [TRANSACTION!2!balance_type],  
			Null As [TRANSACTION!2!cashdeposit_number], 
			NULL AS [TRANSACTION!2!term],
			NULL AS [TRANSACTION!2!installment],
			claim.claim_id AS [CLAIM!3!id],
			Null AS [CLAIM!3!claim_number],
			Null AS [CLAIM!3!underwriting_year_code],
			Peril_Type.peril_type_id as [PERILTYPE!4!id],
			null as [PERILTYPE!4!code],
			null as [PERILTYPE!4!description],
			reserve_type.reserve_type_id as [RESERVETYPE!5!id],
			CASE dt.code 
				WHEN 'CLP' THEN ROUND(SUM(reserve.this_payment),2) 
                ELSE ROUND(SUM(reserve.this_revision),2)
			END [RESERVETYPE!5!total], 
			MIN(reserve_type.description)   As [RESERVETYPE!5!description], 
			MIN(reserve_type.name)   As [RESERVETYPE!5!name], 
			MIN(reserve_type.is_excess)   As [RESERVETYPE!5!is_excess] 

    From    batch b WITH (NOLOCK)

    INNER Join    transdetail WITH (NOLOCK) 
            On transdetail.batch_id = b.batch_id  

	INNER JOIN #class_of_business_account_link WITH (NOLOCK) ON 
		transdetail.account_id = #class_of_business_account_link.account_id
	
	INNER JOIN document WITH (NOLOCK) on 
		transdetail.document_id = document.document_id
 
	INNER JOIN stats_folder WITH (NOLOCK) ON 
		stats_folder.document_ref = document.document_ref

	inner join claim WITH (NOLOCK) on 
	stats_folder.loss_id = claim.claim_id
	
	inner join claim_peril WITH (NOLOCK) on 
	claim.claim_id =claim_peril.claim_id

		inner join reserve WITH (NOLOCK) on 
		claim_peril.claim_peril_id = reserve.claim_peril_id
	
			inner join reserve_type WITH (NOLOCK) on 
			reserve.reserve_type_id = reserve_type.reserve_type_id			

	inner join peril_type WITH (NOLOCK) on 
	claim_peril.peril_type_id = peril_type.peril_type_id
	AND peril_type.class_of_business_id = 	#class_of_business_account_link.class_of_business_id

	Left join documenttype dt WITH (NOLOCK) on document.documenttype_id=dt.documenttype_id
    -- Only the current batch  
    Where   b.batch_id = @batch_id  
    AND dt.code in ('CLO', 'CLA','CLP')

    GROUP BY 
            b.batch_ref,  
            b.total_transactions, 
            b.total_amount,
            transdetail.transdetail_id,
            transdetail.account_id,  
	    claim.claim_id,
            peril_type.peril_type_id, 
            reserve_type.reserve_type_id,dt.code

 HAVING SUM(reserve.this_payment) <> 0 OR SUM(reserve.this_revision) <> 0


  ORDER BY 

[EXPORT_HEADER!1!batch_id], 
[TRANSACTION!2!transdetail_id],
[CLAIM!3!id],
[PERILTYPE!4!id],
[RESERVETYPE!5!id]

FOR XML EXPLICIT

DROP TABLE #class_of_business_account_link

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

