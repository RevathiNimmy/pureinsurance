SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_EDI_Data'
GO

CREATE PROCEDURE spu_Get_EDI_Data
	@InsFileCnt integer
AS

	DECLARE @MAILBOX VARCHAR(100)
	DECLARE @AGENCYREF VARCHAR(100)
	DECLARE @RENEWALFREQUENCY VARCHAR(100)
	DECLARE @ADDRESSLINE1 VARCHAR(100)
	DECLARE @ADDRESSLINE2 VARCHAR(100)
	DECLARE @ADDRESSLINE3 VARCHAR(100)
	DECLARE @ADDRESSLINE4 VARCHAR(100)
	DECLARE @POSTCODE VARCHAR(10)
	DECLARE @EXPIRYDATE VARCHAR(100)
	DECLARE @ANNUALPREM VARCHAR(100)
	DECLARE @PREMIUM VARCHAR(100)
	DECLARE @NETPREM VARCHAR(100)
	DECLARE @COMMISSIONAMT VARCHAR(100)
	DECLARE @IPTAMT VARCHAR(100)
	DECLARE @IPTPERCENT VARCHAR(100)
	DECLARE @TAXAMT VARCHAR(100)
	DECLARE @VATABLEAMT VARCHAR(100)
	DECLARE @VATPERCENT VARCHAR(100)
	DECLARE @VATAMT VARCHAR(100)

	SELECT	@MAILBOX = isnull(GS.edi_mail_box, ''), @AGENCYREF = isnull(GS.agency_code, ''), @RENEWALFREQUENCY = isnull(RF.description, '')
	FROM	gis_scheme as GS
	JOIN	gis_policy_link as GPL
	ON	GPL.gis_scheme_id = GS.gis_scheme_id
	JOIN	Renewal_Frequency as RF
	ON	GS.renewal_frequency_id = RF.renewal_frequency_id
	WHERE	GPL.insurance_file_cnt = @InsFileCnt

	SELECT	@ADDRESSLINE1 = isnull(address1, ''), @ADDRESSLINE2 = isnull(address2, ''), @ADDRESSLINE3 = isnull(address3, ''), @ADDRESSLINE4 = isnull(address4, ''), @POSTCODE = isnull(postal_code, '')
	FROM	address AS A
	JOIN	Party_Address_Usage AS PAU
	ON	PAU.address_cnt = A.address_cnt
	JOIN	Address_Usage_Type AS ATU
	ON	ATU.Address_Usage_Type_id = PAU.Address_Usage_Type_id
	JOIN	insurance_file AS INSFILE
	ON	PAU.party_cnt = INSFILE.insured_cnt
	WHERE	ATU.code = '3131 XCO'
	AND	INSFILE.insurance_file_cnt = @InsFileCnt

	SELECT	@EXPIRYDATE = isnull(expiry_date, ''), @ANNUALPREM = isnull(annual_premium, ''), @PREMIUM = isnull(this_premium, ''), @NETPREM = isnull(net_premium, ''), 
		@COMMISSIONAMT = isnull(commission_amount, ''), @IPTAMT = isnull(iptable_amount, ''), @IPTPERCENT = isnull(ipt_percentage, ''), 
		@TAXAMT = isnull(tax_amount, ''), @VATABLEAMT = isnull(vatable_amount, ''), @VATPERCENT = isnull(vat_percentage, ''), @VATAMT = isnull(vat_amount, '')
	FROM	insurance_file
	WHERE	insurance_file_cnt = @InsFileCnt

	SELECT	'' AS Sender_id, '' AS Receiver_id, @MAILBOX as MAILBOX, @AGENCYREF as AGENCYREF, @RENEWALFREQUENCY as RENEWALFREQUENCY, @ADDRESSLINE1 as ADDRESSLINE1, @ADDRESSLINE2 as ADDRESSLINE2, @ADDRESSLINE3 as ADDRESSLINE3, @ADDRESSLINE4 as ADDRESSLINE4, @POSTCODE as POSTCODE, @EXPIRYDATE as EXPIRYDATE, @ANNUALPREM as ANNUALPREM, @PREMIUM as PREMIUM, @NETPREM as NETPREM, @COMMISSIONAMT as COMMISSIONAMT, @IPTAMT as IPTAMT, @IPTPERCENT as IPTPERCENT, @TAXAMT as TAXAMT, @VATABLEAMT as VATABLEAMT, @VATPERCENT as VATPERCENT, @VATAMT as VATAMT
GO