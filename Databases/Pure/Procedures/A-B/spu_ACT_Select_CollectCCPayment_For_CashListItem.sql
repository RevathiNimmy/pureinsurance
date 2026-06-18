EXEC DDLDropProcedure 'spu_ACT_Select_CollectCCPayment_For_CashListItem'
GO

CREATE PROCEDURE spu_ACT_Select_CollectCCPayment_For_CashListItem
	@cashlistitem_id INT
AS

	SELECT
		CL.cashlisttype_id,
		MTC.code,
		CLI.amount,
		CLI.cc_name,
		CLI.cc_customer,
		CLI.cc_number,
		CLI.cc_expiry_date,
		CLI.cc_start_date,
		CLI.cc_issue,
		CLI.cc_pin,
		CLI.cc_auth_code,
		CLI.cc_manual_auth_code,
		CLI.cc_transaction_code,
		CLI.address1,
		CLI.postal_code,
		MTC.connector_address, 
		MTC.connector_port,
		MTC.connector_timeout_seconds
	FROM
		MediaType_Connector MTC
	INNER JOIN
		MediaType_Issuer MTI ON MTI.mediatype_connector_id=MTC.mediatype_connector_id
	INNER JOIN
		CashListItem CLI ON CLI.mediatype_issuer_id=MTI.mediatype_issuer_id
	INNER JOIN
		CashList CL ON CL.cashlist_id=CLI.cashlist_id
	WHERE
		CLI.cashlistitem_id=@cashlistitem_id

GO

