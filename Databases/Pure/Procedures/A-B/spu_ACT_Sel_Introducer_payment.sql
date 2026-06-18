SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Sel_Introducer_payment'
GO

CREATE PROCEDURE spu_ACT_Sel_Introducer_payment
    @document_id  int
AS
BEGIN

	select  td.transdetail_id, 
		td.account_id, 
		((epg.agent_commission_value - epg.agent_commission_amount) * -1)
		from document d
		join transdetail td
		on d.document_id = td.document_id
		join account a
		on td.account_id = a.account_id
		join ledger l
		on l.ledger_id = a.ledger_id
		join ledgertype lt
		on l.ledgertype_id = lt.ledgertype_id
		join party p
		on p.party_cnt = a.account_key
		join document d2
		on d2.document_ref = substring(td.spare, 10, 11)
		and d2.company_id = d.company_id
		join Transaction_export_folder tef
		on tef.source_id = d2.company_id 
		and tef.document_ref = d2.document_ref
		join event_log el
		on el.transaction_export_folder_cnt = tef.transaction_export_folder_cnt
		join event_policy_agents epg
		on epg.insurance_file_Cnt = el.event_Cnt
		and epg.agent_cnt = p.party_cnt
		where d.document_id = @document_id
		and l.ledger_short_name = 'TR'
		and left(td.spare, 8) = 'INT COMM'
	
END

GO
 
