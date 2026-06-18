SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_client_details_U'
GO

CREATE PROCEDURE spu_get_client_details_U  
    @pol_id int,  
    @clm_dt datetime  
AS  
  
/*  
- 06/12/2000 Tinny Created  
- we do not use event file any more  
- 05/06/2001 Jude Killip - retrieve country ID  
- 13/06/2001 RWH - Added UNION to retrieve contacts attached to addresses for client.  
- 05/10/2001 RWH - Ensure contact details not attached to an address do not bring back  
                    address_usage_type.  
- 15/10/2001 RWH - Updated UNION to be 3 section. Contacts attached to party, addresses  
                    attached to party and contacts attached to addresses. If try and retrieve  
                    addresses and contacts attached to addresses in one go then you only get  
                    addresses that have contatcts attached. Can't outer join properly thru' 2  
                    many-to-many tables (party_address_usage & contact_address_usage).  
- 16/02/2002 Tom - Updated UNION to be UNION ALL.  We _know_ there won't be any duplicates, so  
                    there's no need to let the UNION remove them  
*/  

DECLARE @PrtyCnt int  
DECLARE @Table table(insurance_holder_cnt int )

INSERT INTO @Table  
SELECT  ifo.insurance_holder_cnt
FROM    Insurance_File ifi,
        Insurance_Folder ifo
WHERE   ifi.insurance_file_cnt= @pol_id
AND     ifi.insurance_folder_cnt = ifo.insurance_folder_cnt


  
SELECT  @PrtyCnt=Count(Contact_cnt)  
FROM    Party_Contact_usage  
WHERE   Party_Cnt IN  
( SELECT  insurance_holder_cnt FROM @Table) 



If @PrtyCnt > 0  
  
BEGIN  
  
-----------------------------  
-- Contacts attached to Party.  
-----------------------------  
  
    SELECT      Party.resolved_name,  
                Party.shortname,  
                Null as address1,  
                Null as address2,  
                Null as address3,  
                Null as address4,  
                Null as address5,  
                Contact.area_code,  
                Contact.number,  
                Contact.extension,  
                Contact_Type.contact_type_id,  
                Contact_Type.code,  
                Party.party_cnt,  
                Null as country_id,  
                Null as address_usage_type_description,  
                Null as address_usage_type,  
                Null as address_cnt 
  
    FROM        Party,  
                party_Contact_Usage,  
                Contact,  
                contact_type  
  
    WHERE       Party.Party_cnt IN     ( SELECT  insurance_holder_cnt FROM @Table) 
            
    AND         Party.party_cnt= Party_contact_Usage.Party_cnt  
    AND         Party_Contact_Usage.Contact_cnt=Contact.Contact_cnt  
    AND         Contact.Contact_type_id =Contact_type.Contact_type_id  
    AND         Party.party_type_id IN (1, 2, 4)  
   
                 
      
  
    UNION ALL  
  
---------------------------------  
-- Contacts attached to addresses.  
---------------------------------  
    SELECT  p.resolved_name,  
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
            p.party_cnt,  
            a.country_id,  
            aut.description,  
            aut.address_usage_type_id,  
            a.address_cnt  
  
    FROM    party p,  
            party_address_usage pau,  
            contact_address_usage cau,  
            contact c,  
            address a,  
            address_usage_type aut,  
            contact_type ct  
  
    WHERE   p.Party_cnt IN     ( SELECT  insurance_holder_cnt FROM @Table) 
    AND     pau.party_cnt = p.party_cnt  
    AND     cau.address_cnt = pau.address_cnt  
    AND     c.contact_cnt = cau.contact_cnt  
    AND     a.address_cnt = pau.address_cnt  
    AND     aut.address_usage_type_id = pau.address_usage_type_id  
    AND     ct.contact_type_id = c.contact_type_id  
  
    UNION ALL  
---------------------------------  
-- Addresses attached to Party.  
---------------------------------  
    SELECT  p.resolved_name,  
            p.shortname,  
            a.address1,  
            a.address2,  
            a.address3,  
            a.address4,  
            a.postal_code,  
            Null,  
            Null,  
            Null,  
            Null,  
            Null,  
            p.party_cnt,  
            a.country_id,  
            aut.description,  
            aut.address_usage_type_id,  
            a.address_cnt  
  
    FROM    party p,  
            party_address_usage pau,  
            address a,  
            address_usage_type aut  
  
    WHERE   p.Party_cnt IN     ( SELECT  insurance_holder_cnt FROM @Table)                                     
    AND     pau.party_cnt = p.party_cnt  
    AND     a.address_cnt = pau.address_cnt  
    AND     aut.address_usage_type_id = pau.address_usage_type_id  
  
END  
  
ELSE --partycnt < 0  
  
BEGIN  
  
    SELECT      Party.resolved_name,  
        Party.shortname,  
        Address.address1,  
        Address.address2,  
        Address.address3,  
        Address.address4,  
        Address.postal_code,  
        NULL area_code,  
        NULL number,  
        NULL extension,  
        NULL contact_type_id,  
        NULL code,  
        Party.party_cnt,  
        Address.country_id,  
        Address_Usage_Type.description,  
        Address_Usage_Type.address_usage_type_id,  
        Address.address_cnt  
  
    FROM    Party, party_Address_usage, Address, Address_Usage_Type
    WHERE   Party.party_cnt= Party_Address_Usage.Party_cnt  
    AND     Party_Address_Usage.Address_cnt=Address.Address_cnt  
    AND     Party.party_type_id IN (1, 2, 4)  
    AND     party_address_usage.address_usage_type_id = 4  
    AND     party_address_usage.address_usage_type_id = Address_Usage_Type.address_usage_type_id
    AND     Party.Party_cnt IN     ( SELECT  insurance_holder_cnt FROM @Table) 

   UNION ALL  
  
    SELECT  p.resolved_name,  
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
            p.party_cnt,  
            a.country_id,  
            aut.description,  
            aut.address_usage_type_id,  
            a.address_cnt  
  
    FROM        party p,  
            party_address_usage pau,  
            contact_address_usage cau,  
            contact c,  
            address a,  
            address_usage_type aut,  
            contact_type ct  
  
    WHERE   P.Party_cnt IN     ( SELECT  insurance_holder_cnt FROM @Table) 
    AND     pau.party_cnt = p.party_cnt  
    AND     cau.address_cnt = pau.address_cnt  
    AND     c.contact_cnt = cau.contact_cnt  
    AND     a.address_cnt = pau.address_cnt  
    AND     aut.address_usage_type_id = pau.address_usage_type_id  
    AND     ct.contact_type_id = c.contact_type_id  
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
