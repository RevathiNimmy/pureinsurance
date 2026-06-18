DDLDROPPROCEDURE 'spu_pf_GetClientDetails'
GO
CREATE PROCEDURE spu_pf_GetClientDetails(
 		 @party_cnt int)
AS
DECLARE @partytypeid INT
SET @partytypeid = (SELECT party_type_id FROM party WHERE party_cnt = @party_cnt)
IF @partytypeid = 1
BEGIN
	SELECT 	PAR.shortname, PAR.resolved_name, PAR.name, PPC.forename, PPC.party_title_code, ADR.address1, ADR.address2, ADR.address3, ADR.address4, ADR.postal_code, 1 as no_of_employees
	FROM	party PAR, address ADR, party_address_usage PAU, address_usage_type AUT, party_personal_client PPC
	WHERE   PAR.party_cnt = @party_cnt
	AND	PAR.party_cnt = PPC.party_cnt
	AND	PAR.party_cnt = PAU.party_cnt
	AND	PAU.address_cnt = ADR.address_cnt
	AND     PAU.address_usage_type_id = AUT.address_usage_type_id 
	AND     AUT.code = '3131 XCO'
END
IF @partytypeid = 4
BEGIN
	SELECT 	PAR.shortname, PAR.resolved_name, PAR.name, '' as forename, '' as party_title_code, ADR.address1, ADR.address2, ADR.address3, ADR.address4, ADR.postal_code, PCC.no_of_employees
	FROM	party PAR, address ADR, party_address_usage PAU, address_usage_type AUT, party_corporate_client PCC
	WHERE   PAR.party_cnt = @party_cnt
	AND	PAR.party_cnt = PAU.party_cnt
	AND	PAU.address_cnt = ADR.address_cnt
	AND     PAU.address_usage_type_id = AUT.address_usage_type_id 
	AND     AUT.code = '3131 XCO'
	AND     PCC.party_cnt = PAR.party_cnt
END
