SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_client_details'
GO


CREATE PROCEDURE spu_get_client_details
    @pol_id int,
    @clm_dt datetime
AS


--Commented Old query as it was failing if a contact did not exist for the party
--DC260101 added hidden option check BrkUnderwrite
Declare  @PrtyCnt int,
     @BrkUnderwrite char

--SJP (13/06/2002) UW_Type selected by branch_id  = 1 and option_number = 1 
--to ensure unique record
Select @BrkUnderwrite = value from hidden_options
	where branch_id = 1 and option_number = 1

IF @BrkUnderwrite = 'A'
BEGIN

--DC250901 -start -rejig thw way check made to see if there are contact info

--as was ->

/*
	Select @PrtyCnt=Count(Contact_cnt) from Party_Contact_usage
--	DC260101 use insurance_file instead of event_insurance_file
--	where Party_Cnt IN (Select Insured_cnt from Event_Insurance_File
	where Party_Cnt IN (Select Insured_cnt from Insurance_File
	where insurance_file_cnt= @pol_id)
*/

--as is now ->

	select @PrtyCnt=Count(c.area_code)
 		from 	party p,
			party_address_usage pau,
			address a,
			contact_address_Usage cau,
			contact c,
			contact_type ct
		where 	p.party_cnt IN
 			(	
			select 	Insured_cnt 
			from 	Insurance_file 
			where 	insurance_file_cnt = @pol_id
			)
 		and 	(p.party_type_id = 1 OR p.party_type_id = 2 OR p.party_type_id = 4)
		and 	pau.party_cnt = p.party_cnt
		and	( 
			pau.address_usage_type_id = 3  
 			or  pau.address_usage_type_id = 4
			)
		and	a.address_cnt = pau.address_cnt
		and	cau.address_cnt = a.address_cnt
 		and	c.contact_cnt = cau.contact_cnt
 		and	ct.contact_type_id = c.contact_type_id 		

--DC250901 -end

	If @PrtyCnt > 0
	BEGIN

--DC250901 -start -rejigged the following, to get the correct result

--as was  ->

/*		SELECT Party.name, Party.shortname, Address.address1,
 		Address.address2, Address.address3, Address.address4,
 		Address.postal_code, Contact.area_code, Contact.number,
 		Contact.extension, Contact_Type.contact_type_id,
 		Contact_Type.code, Party.party_cnt
 		from Party,party_Address_usage,Address,party_Contact_Usage,Contact,contact_type
		where Party.party_cnt= Party_Address_Usage.Party_cnt AND
 		Party_Address_Usage.Address_cnt=Address.Address_cnt AND
 		Party.Party_cnt=Party_contact_Usage.Party_cnt AND
 		Party_Contact_Usage.Contact_cnt=Contact.Contact_cnt AND
 		Contact.Contact_type_id =Contact_type.Contact_type_id AND
 		( Party.party_type_id = 1 OR Party.party_type_id = 2 OR Party.party_type_id = 4)
 		AND party_address_usage.address_usage_type_id = 4
 		AND Party.party_cnt IN
--		DC260101 use insurance_file instead of event_insurance_file
-- 		(Select Insured_cnt from Event_Insurance_file where insurance_file_cnt = @pol_id )
 		(Select Insured_cnt from Insurance_file where insurance_file_cnt = @pol_id )
--		AND Insurance_file_cnt IN
-- 		(Select event_cnt from event_log where CONVERT(char(12),event_log.event_date,103) = CONVERT(char(12), @clm_dt, 103)))
*/

-- as is now ->
		--DC050202 use resolved name instead of name
		select 	p.resolved_name, p.shortname, 
			a.address1, a.address2, a.address3, a.address4, a.postal_code,
			c.area_code, c.number, c.extension, 
			ct.contact_type_id, ct.code, p.party_cnt, a.address_cnt
 		from 	party p,
			party_address_usage pau,
			address a,
			contact_address_Usage cau,
			contact c,
			contact_type ct
		where 	p.party_cnt IN
 			(	
			select 	Insured_cnt 
			from 	Insurance_file 
			where 	insurance_file_cnt = @pol_id
			)
 		and 	(p.party_type_id = 1 OR p.party_type_id = 2 OR p.party_type_id = 4)
		and 	pau.party_cnt = p.party_cnt
		and	( 
			pau.address_usage_type_id = 3  
 			or  pau.address_usage_type_id = 4
			)
		and	a.address_cnt = pau.address_cnt
		and	cau.address_cnt = a.address_cnt
 		and	c.contact_cnt = cau.contact_cnt
 		and	ct.contact_type_id = c.contact_type_id 		

--DC250901 -end

	END
	else --partycnt > 0
	BEGIN
		--DC050202 use resolved name instead of name
		SELECT Party.resolved_name, Party.shortname, Address.address1,
 		Address.address2, Address.address3, Address.address4,
 		Address.postal_code, NULL area_code, NULL number,
 		NULL extension, NULL contact_type_id,
 		NULL code, Party.party_cnt, Address.address_cnt
 		from Party,party_Address_usage,Address
 		where Party.party_cnt= Party_Address_Usage.Party_cnt AND
		Party_Address_Usage.Address_cnt=Address.Address_cnt AND
 --		Party.Party_cnt=Party_contact_Usage.Party_cnt AND
 --		Party_Contact_Usage.Contact_cnt=Contact.Contact_cnt AND
 --		Contact.Contact_type_id =Contact_type.Contact_type_id AND
 		(Party.party_type_id = 1 OR Party.party_type_id = 2 OR Party.party_type_id = 4)
 		AND party_address_usage.address_usage_type_id = 4
 		AND Party.party_cnt IN
--		DC260101 use insurance_file instead of event_insurance_file
--		(Select Insured_cnt from Event_Insurance_file where insurance_file_cnt = @pol_id )
		(Select Insured_cnt from Insurance_file where insurance_file_cnt = @pol_id )
--		AND Insurance_file_cnt IN
-- 		(Select event_cnt from event_log where CONVER T(char(12),event_log.event_date,103) = CONVERT(char(12), @clm_dt, 103)))
	END
	
END
ELSE
BEGIN

--DC260101 added seperate part for underwriting - uses event insurance file
    Select @PrtyCnt=Count(Contact_cnt) from Party_Contact_usage
    where Party_Cnt IN (Select Insured_cnt from Event_Insurance_File
    where insurance_file_cnt= @pol_id)
    If @PrtyCnt > 0
    BEGIN
        SELECT Party.name, Party.shortname, Address.address1,
        Address.address2, Address.address3, Address.address4,
        Address.postal_code, Contact.area_code, Contact.number,
        Contact.extension, Contact_Type.contact_type_id,
        Contact_Type.code, Party.party_cnt, Address.address_cnt
        from Party,party_Address_usage,Address,party_Contact_Usage,Contact,contact_type
        where Party.party_cnt= Party_Address_Usage.Party_cnt AND
        Party_Address_Usage.Address_cnt=Address.Address_cnt AND
        Party.Party_cnt=Party_contact_Usage.Party_cnt AND
        Party_Contact_Usage.Contact_cnt=Contact.Contact_cnt AND
        Contact.Contact_type_id =Contact_type.Contact_type_id AND
        ( Party.party_type_id = 1 OR Party.party_type_id = 2 OR Party.party_type_id = 4)
        AND party_address_usage.address_usage_type_id = 4
        AND Party.party_cnt IN
        (Select Insured_cnt from Event_Insurance_file where insurance_file_cnt = @pol_id )
    END
    else --partycnt > 0
    BEGIN
        SELECT Party.name, Party.shortname, Address.address1,
        Address.address2, Address.address3, Address.address4,
        Address.postal_code, NULL area_code, NULL number,
        NULL extension, NULL contact_type_id,
        NULL code, Party.party_cnt, Address.address_cnt
        from Party,party_Address_usage,Address
        where Party.party_cnt= Party_Address_Usage.Party_cnt AND
        Party_Address_Usage.Address_cnt=Address.Address_cnt AND
        (Party.party_type_id = 1 OR Party.party_type_id = 2 OR Party.party_type_id = 4)
        AND party_address_usage.address_usage_type_id = 4
        AND Party.party_cnt IN
        (Select Insured_cnt from Event_Insurance_file where insurance_file_cnt = @pol_id )
    END

END
GO


