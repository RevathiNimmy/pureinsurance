EXECUTE DDLDropProcedure 'spu_update_wpaddresses'
go

CREATE PROCEDURE spu_update_wpaddresses

AS

DECLARE @AddressDescription Varchar(255)
DECLARE @AddressCode char(10)

SET NOCOUNT ON

DELETE FROM wp_fields WHERE sql='spu_wp_addresses'
DELETE FROM wp_fields WHERE sql='spu_wp_insurer_addresses'
DELETE FROM wp_fields WHERE sql='spu_wp_agent_addresses'
DELETE FROM wp_fields WHERE sql='spu_wp_underwriter_addresses'


DECLARE cAddresses CURSOR FAST_FORWARD FOR

--  R Griffiths 2003-06-04
--  Added "WHERE is_deleted=0", otherwise you can get duplicate key violations
--  when adding to the wp_fields table, as experienced by CMIB.

--  SELECT description, code FROM address_usage_type
    SELECT description, code FROM address_usage_type WHERE is_deleted=0

OPEN cAddresses
FETCH NEXT FROM cAddresses INTO @AddressDescription, @AddressCode
WHILE @@FETCH_STATUS = 0 BEGIN

    --DC110304 PN10913 -start -to overcome problem with puntuation in address description and code
    --              probably better way to do this but for now this will have to do
    SELECT @AddressDescription = replace(@AddressDescription,'!','') 
    SELECT @AddressDescription = replace(@AddressDescription,'"','')
    SELECT @AddressDescription = replace(@AddressDescription,'£','') 
    SELECT @AddressDescription = replace(@AddressDescription,'$','')
    SELECT @AddressDescription = replace(@AddressDescription,'%','') 
    SELECT @AddressDescription = replace(@AddressDescription,'^','')
    SELECT @AddressDescription = replace(@AddressDescription,'&','') 
    SELECT @AddressDescription = replace(@AddressDescription,'*','')
    SELECT @AddressDescription = replace(@AddressDescription,'(','') 
    SELECT @AddressDescription = replace(@AddressDescription,')','')
    SELECT @AddressDescription = replace(@AddressDescription,'-','') 
    SELECT @AddressDescription = replace(@AddressDescription,'+','')
    SELECT @AddressDescription = replace(@AddressDescription,'=','')
    SELECT @AddressDescription = replace(@AddressDescription,'[','') 
    SELECT @AddressDescription = replace(@AddressDescription,']','')
    SELECT @AddressDescription = replace(@AddressDescription,':','') 
    SELECT @AddressDescription = replace(@AddressDescription,';','')
    SELECT @AddressDescription = replace(@AddressDescription,'@','') 
    SELECT @AddressDescription = replace(@AddressDescription,'~','')
    SELECT @AddressDescription = replace(@AddressDescription,'#','') 
    SELECT @AddressDescription = replace(@AddressDescription,'<','')
    SELECT @AddressDescription = replace(@AddressDescription,'>','') 
    SELECT @AddressDescription = replace(@AddressDescription,',','')
    SELECT @AddressDescription = replace(@AddressDescription,'.','') 
    SELECT @AddressDescription = replace(@AddressDescription,'?','')
    SELECT @AddressDescription = replace(@AddressDescription,'/','')
    SELECT @AddressDescription = replace(@AddressDescription,'\','')
    SELECT @AddressDescription = replace(@AddressDescription,'|','')
     --DC110304 PN10913 -end

    --Add Client Address
    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'Line1')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(@AddressDescription, ' ', '') + 'Line1','spu_wp_addresses',
                      replace(@AddressDescription,' ','_') + '_Address1',0,'Address - Party',@AddressDescription,
                      'Address 1',1,9)

    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'Line2')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(@AddressDescription, ' ', '') + 'Line2','spu_wp_addresses',
                      replace(@AddressDescription,' ','_') + '_Address2',0,'Address - Party',@AddressDescription,
                      'Address 2',1,9)

    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'Line3')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(@AddressDescription, ' ', '') + 'Line3','spu_wp_addresses',
                      replace(@AddressDescription,' ','_') + '_Address3',0,'Address - Party',@AddressDescription,
                      'Address 3',1,9)

    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'Line4')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(@AddressDescription, ' ', '') + 'Line4','spu_wp_addresses',
                      replace(@AddressDescription,' ','_') + '_Address4',0,'Address - Party',@AddressDescription,
                      'Address 4',1,9)

	 IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'Line5')

        INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,

                             display_name,is_displayed,product_family)

               VALUES(replace(@AddressDescription, ' ', '') + 'Line5','spu_wp_addresses',

                      replace(@AddressDescription,' ','_') + '_Address5',0,'Address - Party',@AddressDescription,

                      'Address 5',1,9)



    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'Line6')

        INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,

                              display_name,is_displayed,product_family)

               VALUES(replace(@AddressDescription, ' ', '') + 'Line6','spu_wp_addresses',

                      replace(@AddressDescription,' ','_') + '_Address6',0,'Address - Party',@AddressDescription,

                      'Address 6',1,9)



    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'Line7')

        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,

                              display_name,is_displayed,product_family)

               VALUES(replace(@AddressDescription, ' ', '') + 'Line7','spu_wp_addresses',

                      replace(@AddressDescription,' ','_') + '_Address7',0,'Address - Party',@AddressDescription,

                      'Address 7',1,9)



    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'Line8')

        INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,

                              display_name,is_displayed,product_family)

               VALUES(replace(@AddressDescription, ' ', '') + 'Line8','spu_wp_addresses',

                      replace(@AddressDescription,' ','_') + '_Address8',0,'Address - Party',@AddressDescription,

                      'Address 8',1,9)


	 IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'Line9')

        INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,

                              display_name,is_displayed,product_family)

               VALUES(replace(@AddressDescription, ' ', '') + 'Line9','spu_wp_addresses',

                      replace(@AddressDescription,' ','_') + '_Address9',0,'Address - Party',@AddressDescription,

                      'Address 9',1,9)



    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'Line6')

        INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,

                              display_name,is_displayed,product_family)

               VALUES(replace(@AddressDescription, ' ', '') + 'Line6','spu_wp_addresses',

                      replace(@AddressDescription,' ','_') + '_Address6',0,'Address - Party',@AddressDescription,

                      'Address 6',1,9)



    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'Line7')

        INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,

                              display_name,is_displayed,product_family)

               VALUES(replace(@AddressDescription, ' ', '') + 'Line7','spu_wp_addresses',

                      replace(@AddressDescription,' ','_') + '_Address7',0,'Address - Party',@AddressDescription,

                      'Address 7',1,9)



    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'Line8')

        INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,

                              display_name,is_displayed,product_family)

               VALUES(replace(@AddressDescription, ' ', '') + 'Line8','spu_wp_addresses',

                      replace(@AddressDescription,' ','_') + '_Address8',0,'Address - Party',@AddressDescription,

                      'Address 8',1,9)


	 IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'Line9')

        INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,

                              display_name,is_displayed,product_family)

               VALUES(replace(@AddressDescription, ' ', '') + 'Line9','spu_wp_addresses',

                      replace(@AddressDescription,' ','_') + '_Address9',0,'Address - Party',@AddressDescription,

                      'Address 9',1,9)

	 IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'Line10')

        INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,

                              display_name,is_displayed,product_family)

               VALUES(replace(@AddressDescription, ' ', '') + 'Line10','spu_wp_addresses',

                      replace(@AddressDescription,' ','_') + '_Address10',0,'Address - Party',@AddressDescription,

                      'Address 10',1,9)
    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' Address', '') + 'Postcode')
        INSERT INTO wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(replace(@AddressDescription, ' Address', ''),' ','') + 'Postcode','spu_wp_addresses',
                      replace(@AddressDescription,' ','_') + '_Postcode',0,'Address - Party',@AddressDescription,
                      'Postcode',1,9)

    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'Country')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(@AddressDescription, ' ', '') + 'Country','spu_wp_addresses',
                      replace(@AddressDescription,' ','_') + '_Country',0,'Address - Party',@AddressDescription,
                      'Country',1,9)

    --Add Insurer Address
    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'InsLine1')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(@AddressDescription, ' ', '') + 'InsLine1','spu_wp_insurer_addresses',
                      replace(@AddressDescription,' ','_') + '_Address1',0,'Address - Insurer',@AddressDescription,
                      'Address 1',1,9)

    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'InsLine2')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(@AddressDescription, ' ', '') + 'InsLine2','spu_wp_insurer_addresses',
                      replace(@AddressDescription,' ','_') + '_Address2',0,'Address - Insurer',@AddressDescription,
                      'Address 2',1,9)

    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'InsLine3')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(@AddressDescription, ' ', '') + 'InsLine3','spu_wp_insurer_addresses',
                      replace(@AddressDescription,' ','_') + '_Address3',0,'Address - Insurer',@AddressDescription,
                      'Address 3',1,9)

    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'InsLine4')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(@AddressDescription, ' ', '') + 'InsLine4','spu_wp_insurer_addresses',
                      replace(@AddressDescription,' ','_') + '_Address4',0,'Address - Insurer',@AddressDescription,
                      'Address 4',1,9)

    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' Address', '') + 'InsPostcode')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(replace(@AddressDescription, ' Address', ''),' ','') + 'InsPostcode','spu_wp_insurer_addresses',
                      replace(@AddressDescription,' ','_') + '_Postcode',0,'Address - Insurer',@AddressDescription,
                      'Postcode',1,9)

    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'InsCountry')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(@AddressDescription, ' ', '') + 'InsCountry','spu_wp_insurer_addresses',
                      replace(@AddressDescription,' ','_') + '_Country',0,'Address - Insurer',@AddressDescription,
                  'Country',1,9)

    --Add Agent Address
    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'AgentLine1')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(@AddressDescription, ' ', '') + 'AgentLine1','spu_wp_Agent_addresses',
                      replace(@AddressDescription,' ','_') + '_Address1',0,'Address - Agent',@AddressDescription,
                      'Address 1',1,9)

    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'AgentLine2')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(@AddressDescription, ' ', '') + 'AgentLine2','spu_wp_agent_addresses',
                      replace(@AddressDescription,' ','_') + '_Address2',0,'Address - Agent',@AddressDescription,
                      'Address 2',1,9)

    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'AgentLine3')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(@AddressDescription, ' ', '') + 'AgentLine3','spu_wp_agent_addresses',
                      replace(@AddressDescription,' ','_') + '_Address3',0,'Address - Agent',@AddressDescription,
                      'Address 3',1,9)

    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'AgentLine4')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(@AddressDescription, ' ', '') + 'AgentLine4','spu_wp_agent_addresses',
                      replace(@AddressDescription,' ','_') + '_Address4',0,'Address - Agent',@AddressDescription,
                      'Address 4',1,9)

    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' Address', '') + 'AgentPostcode')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(replace(@AddressDescription, ' Address', ''),' ','') + 'AgentPostcode','spu_wp_agent_addresses',
                      replace(@AddressDescription,' ','_') + '_Postcode',0,'Address - Agent',@AddressDescription,
                      'Postcode',1,9)

    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'AgentCountry')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(@AddressDescription, ' ', '') + 'AgentCountry','spu_wp_agent_addresses',
                      replace(@AddressDescription,' ','_') + '_Country',0,'Address - Agent',@AddressDescription,
                  'Country',1,9)

    --Add Underwriter Address
    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'UndLine1')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(@AddressDescription, ' ', '') + 'UndLine1','spu_wp_underwriter_addresses',
                      replace(@AddressDescription,' ','_') + '_Address1',0,'Addresses -  FSA Underwriter',@AddressDescription,
                      'Address 1',1,9)

    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'UndLine2')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(@AddressDescription, ' ', '') + 'UndLine2','spu_wp_underwriter_addresses',
                      replace(@AddressDescription,' ','_') + '_Address2',0,'Addresses -  FSA Underwriter',@AddressDescription,
                      'Address 2',1,9)

    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'UndLine3')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(@AddressDescription, ' ', '') + 'UndLine3','spu_wp_underwriter_addresses',
                      replace(@AddressDescription,' ','_') + '_Address3',0,'Addresses -  FSA Underwriter',@AddressDescription,
                      'Address 3',1,9)

    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'UndLine4')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(@AddressDescription, ' ', '') + 'UndLine4','spu_wp_underwriter_addresses',
                      replace(@AddressDescription,' ','_') + '_Address4',0,'Addresses -  FSA Underwriter',@AddressDescription,
                      'Address 4',1,9)

    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' Address', '') + 'UndPostcode')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(replace(@AddressDescription, ' Address', ''),' ','') + 'UndPostcode','spu_wp_underwriter_addresses',
                      replace(@AddressDescription,' ','_') + '_Postcode',0,'Addresses -  FSA Underwriter',@AddressDescription,
                      'Postcode',1,9)

    IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name=replace(@AddressDescription, ' ', '') + 'UndCountry')
        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
               VALUES(replace(@AddressDescription, ' ', '') + 'UndCountry','spu_wp_underwriter_addresses',
                      replace(@AddressDescription,' ','_') + '_Country',0,'Addresses -  FSA Underwriter',@AddressDescription,
                  'Country',1,9)

    --Create Hidden Common Correspondence Entries
    IF @AddressDescription = 'Correspondence Address'
    Begin
        IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name='c' + replace(@AddressDescription, ' ', '') + 'Line1')
            Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
            VALUES('c' + replace(@AddressDescription, ' ', '') + 'Line1','spu_wp_addresses',
                      replace(@AddressDescription,' ','_') + '_Address1',0,'Common',null,
                      'Address 1',0,9)
        IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name='c' + replace(@AddressDescription, ' ', '') + 'Line2')
            Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
            VALUES('c' + replace(@AddressDescription, ' ', '') + 'Line2','spu_wp_addresses',
                      replace(@AddressDescription,' ','_') + '_Address2',0,'Common',null,
                      'Address 2',0,9)
        IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name='c' + replace(@AddressDescription, ' ', '') + 'Line3')
            Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
            VALUES('c' + replace(@AddressDescription, ' ', '') + 'Line3','spu_wp_addresses',
                      replace(@AddressDescription,' ','_') + '_Address3',0,'Common',null,
                      'Address 3',0,9)
        IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name='c' + replace(@AddressDescription, ' ', '') + 'Line4')
            Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
            VALUES('c' + replace(@AddressDescription, ' ', '') + 'Line4','spu_wp_addresses',
                      replace(@AddressDescription,' ','_') + '_Address4',0,'Common',null,
                      'Address 4',0,9)
        IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name='c' + replace(@AddressDescription, ' ', '') + 'Postcode')
            Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
            VALUES('c' + replace(@AddressDescription, ' ', '') + 'Postcode','spu_wp_addresses',
                      replace(@AddressDescription,' ','_') + '_Postcode',0,'Common',null,
                      'Postcode',0,9)
                IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name='c' + replace(@AddressDescription, ' Address', '') + 'Postcode')
                        Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                                      display_name,is_displayed,product_family)
                        VALUES('c' + replace(@AddressDescription, ' Address', '') + 'Postcode','spu_wp_addresses',
                              replace(@AddressDescription,' ','_') + '_Postcode',0,'Common',null,
                              'Postcode',0,9)
        IF NOT EXISTS(SELECT * FROM wp_fields WHERE field_name='c' + replace(@AddressDescription, ' ', '') + 'Country')
            Insert into wp_fields(field_name,sql,column_name,column_type,main_group,sub_group,
                              display_name,is_displayed,product_family)
            VALUES('c' + replace(@AddressDescription, ' ', '') + 'Country','spu_wp_addresses',
                      replace(@AddressDescription,' ','_') + '_Country',0,'Common',null,
                      'Country',0,9)
    End

    FETCH NEXT FROM cAddresses INTO @AddressDescription, @AddressCode
END

CLOSE cAddresses
DEALLOCATE cAddresses

GO
