SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_insurer_details'
GO


CREATE PROCEDURE spu_get_insurer_details
    @pol_id int,
    @TransactionMode int
AS

/************************************************************************************************/
/* Change History : Amended UW section to look at Insurance_File                                */
/*          rather than Event_Insurance_File.       10/04/2001  RWH                             */
/*                                                                                              */
/*
    05/06/2001 Jude Killip - retrieve country ID (UW only)
    05/10/2001 RWH  - Incorporated DC's changes of 250901 except those for UW.
    05/10/2001 RWH  - Add UNION for UW to bring back all contacts attached to all
                    addresses for an agent and include address_usage_type in recordset.
    08/10/2001 RWH  - UW, ensure all SELECT statements return same no. of columns.
    13/06/2002 SJP  - Underwriting or Value is retrieved by option number and branch
    			to ensure unique record retrieved.
*************************************************************************************************/
--Commented Old query as it was failing if a contact did not exist for the party
Declare @PrtyCnt int
Declare @underwriting_broking char(1)

declare @LeadAgentCnt int,
		@IsInTransferMode int,
		@TransferToPartyCnt int
		
--SJP (13/06/2002) UW_Type selected by branch_id  = 1 and option_number = 1 
--to ensure unique record

select @underwriting_broking = value from hidden_options 
	where branch_id = 1 and option_number = 1
if @underwriting_broking = 'A'
Begin

--DC250901 -start -rejig the way check is made for contacts set up for insurer

--as was ->

/*
    Select @PrtyCnt=Count(Contact_cnt) from Party_Contact_usage
--  DC260101 use insurance_file instead of event_insurance_file
--  where Party_Cnt IN (Select lead_insurer_cnt from Event_Insurance_File where insurance_file_cnt= @pol_id)
    where Party_Cnt IN (Select lead_insurer_cnt from Insurance_File where insurance_file_cnt= @pol_id)
*/

--as is now ->

    select @PrtyCnt=Count(c.area_code)
        from    party p,
            party_address_usage pau,
            address a,
            contact_address_Usage cau,
            contact c,
            contact_type ct
        where   p.party_cnt IN
            (
            select  lead_Insurer_cnt
            from    Insurance_file
            where   insurance_file_cnt = @pol_id
            )
        and     p.party_type_id = 7
        and     pau.party_cnt = p.party_cnt
        and pau.address_usage_type_id =  4
        and a.address_cnt = pau.address_cnt
        and cau.address_cnt = a.address_cnt
        and c.contact_cnt = cau.contact_cnt
        and ct.contact_type_id = c.contact_type_id

--DC250901 -end

    If @PrtyCnt > 0
    BEGIN

--as was ->
/*
        SELECT Party.name, Party.shortname, Address.address1,
        Address.address2, Address.address3, Address.address4,
        Address.postal_code, Contact.area_code, Contact.number,
        Contact.extension, Contact_Type.contact_type_id,
        Contact_Type.code
        from Party, party_Address_usage, Address, party_Contact_Usage, Contact, contact_type
        where Party.party_cnt= Party_Address_Usage.Party_cnt AND
        Party_Address_Usage.Address_cnt=Address.Address_cnt AND
        Party.Party_cnt=Party_contact_Usage.Party_cnt AND
        Party_Contact_Usage.Contact_cnt=Contact.Contact_cnt AND
        Contact.Contact_type_id =Contact_type.Contact_type_id AND
        Party.party_type_id = 7 and party_address_usage.address_usage_type_id = 4
        AND Party.party_cnt IN
--      DC260101 use insurance_file instead of event_insurance_file
--      (Select lead_insurer_cnt from Event_Insurance_file where insurance_file_cnt = @pol_id )
        (Select lead_insurer_cnt from Insurance_file where insurance_file_cnt = @pol_id )
--      AND Insurance_file_cnt IN
--      (Select event_cnt from event_log where CONVERT(char(12), event_log.event_date, 103) = CONVERT(char(12), @clm_dt, 103)))

*/

--as is now ->

        select  p.name, p.shortname,
            a.address1, a.address2, a.address3, a.address4, a.postal_code,
            c.area_code, c.number, c.extension,
            ct.contact_type_id, ct.code, p.party_cnt, a.address_cnt
        from    party p,
            party_address_usage pau,
            address a,
            contact_address_Usage cau,
            contact c,
            contact_type ct
        where   p.party_cnt IN
            (
            select  lead_insurer_cnt
            from    Insurance_file
            where   insurance_file_cnt = @pol_id
            )
        and     p.party_type_id = 7
        and     pau.party_cnt = p.party_cnt
        and pau.address_usage_type_id = 4
        and a.address_cnt = pau.address_cnt
        and cau.address_cnt = a.address_cnt
        and c.contact_cnt = cau.contact_cnt
        and ct.contact_type_id = c.contact_type_id

--DC250901 -end

    END
    else
    BEGIN
        SELECT Party.name, Party.shortname, Address.address1,
        Address.address2, Address.address3, Address.address4,
        Address.postal_code, NULL area_code, NULL number,
        NULL extension, NULL contact_type_id,
        NULL code, NULL party_cnt, Address.address_cnt
        from Party, party_Address_usage, Address
        where Party.party_cnt= Party_Address_Usage.Party_cnt AND
        Party_Address_Usage.Address_cnt=Address.Address_cnt AND
--      Party.Party_cnt=Party_contact_Usage.Party_cnt AND
--      Party_Contact_Usage.Contact_cnt=Contact.Contact_cnt AND
--      Contact.Contact_type_id =Contact_type.Contact_type_id AND
        Party.party_type_id = 7 and party_address_usage.address_usage_type_id = 4
        AND Party.party_cnt IN

--      DC260101 use insurance_file instead of insurance_file
--      (Select lead_insurer_cnt from Event_Insurance_file where insurance_file_cnt = @pol_id )
        (Select lead_insurer_cnt from Insurance_file where insurance_file_cnt = @pol_id )
--      AND Insurance_file_cnt IN
--      (Select event_cnt from event_log where CONVERT(char(12), event_log.event_date, 103) = CONVERT(char(12), @clm_dt, 103)))
    END
End -- if underwriting_broking
else
--This is underwriting
Begin


	SELECT  @LeadAgentCnt=pa.party_cnt, 
			@IsInTransferMode=pa.is_in_transfer_mode, 
			@TransferToPartyCnt=pa.transfer_to_party_cnt
	FROM	Party_Agent pa JOIN Insurance_File ifi ON pa.party_cnt = ifi.lead_agent_cnt
	WHERE 	ifi.insurance_file_cnt = @pol_id    	

	SELECT 	@LeadAgentCnt = IsNull(@LeadAgentCnt,0),
			@IsInTransferMode = IsNull(@IsInTransferMode,0),
			@TransferToPartyCnt = IsNull(@TransferToPartyCnt,0)
	
	IF @TransactionMode = 1 -- Open claim
	BEGIN
		IF @LeadAgentCnt <> 0 AND @IsInTransferMode <> 0 AND @TransferToPartyCnt<> 0
			SELECT @LeadAgentCnt = @TransferToPartyCnt
	END
	
	IF @LeadAgentCnt <> 0
	BEGIN
	
		-- NOTE : only diff between the two sql is the contact joins
		SELECT  p.name,
				p.shortname,
				a.address1,
				a.address2,
				a.address3,
				a.address4,
				a.postal_code,
				c.area_code,
				c.number,
				c.extension,
				ct.contact_type_id,
				ct.code,
				a.country_id,
				aut.description,
				aut.address_usage_type_id,
				a.address_cnt,
				pa.contact_person as insurer_contact
		FROM 	Party p
				LEFT JOIN Party_Agent pa ON pa.party_cnt=p.party_cnt
				LEFT JOIN Party_Address_Usage pau ON p.party_cnt = pau.party_cnt AND pau.address_usage_type_id = 4
				LEFT JOIN Address a ON a.address_cnt = pau.address_cnt
				LEFT JOIN Address_Usage_Type aut ON aut.address_usage_type_id = pau.address_usage_type_id				
				LEFT JOIN Party_Contact_Usage pcu ON pcu.party_cnt = p.party_cnt
				LEFT JOIN Contact c ON c.contact_cnt = pcu.contact_cnt
				LEFT JOIN Contact_Type ct ON ct.contact_type_id = c.contact_type_id
		WHERE	p.party_cnt = @LeadAgentCnt
		AND		p.party_type_id = 3

		UNION

		SELECT  p.name,
				p.shortname,
				a.address1,
				a.address2,
				a.address3,
				a.address4,
				a.postal_code,
				c.area_code,
				c.number,
				c.extension,
				ct.contact_type_id,
				ct.code,
				a.country_id,
				aut.description,
				aut.address_usage_type_id,
				a.address_cnt,
				pa.contact_person as insurer_contact
		FROM    Party p
				LEFT JOIN Party_Agent pa ON pa.party_cnt=p.party_cnt
				LEFT JOIN Party_Address_Usage pau ON pau.party_cnt = p.party_cnt
				LEFT JOIN Contact_Address_Usage cau ON cau.address_cnt = pau.address_cnt
				LEFT JOIN Contact c ON c.contact_cnt = cau.contact_cnt
				LEFT JOIN Address a ON a.address_cnt = pau.address_cnt
				LEFT JOIN Address_Usage_Type aut ON aut.address_usage_type_id = pau.address_usage_type_id
				LEFT JOIN Contact_Type ct ON ct.contact_type_id = c.contact_type_id
		WHERE	p.party_cnt = @LeadAgentCnt
	END

End
GO


