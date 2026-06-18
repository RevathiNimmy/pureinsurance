SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
EXECUTE DDLDropTrigger 'tr_Contact_Address_Usage_audit_log'
GO
CREATE TRIGGER tr_Contact_Address_Usage_audit_log ON dbo.Contact_Address_Usage
    FOR INSERT,DELETE 
AS
SET NOCOUNT ON;

/*declare all the variables*/
DECLARE @Address_Cnt INT
DECLARE @ScreenHierarchy VARCHAR(500);
DECLARE @Configuration_Audit_Master_Id INT
DECLARE @Contact_Cnt INT
DECLARE @UniqueID VARCHAR(50)
DECLARE @Type VARCHAR(10)
DECLARE @Party_Type_Id INT
DECLARE @Party_Cnt INT

IF EXISTS (SELECT * FROM INSERTED)
	SET @Type = 'I'
ELSE 
	SET @Type = 'D'

SELECT * INTO #ins FROM INSERTED;

SELECT * INTO #del FROM DELETED;

IF @Type = 'D'
	SELECT @Address_Cnt=address_cnt, @Contact_Cnt = contact_cnt FROM #del
ELSE
	SELECT @Address_Cnt=address_cnt, @Contact_Cnt = contact_cnt FROM #ins

SELECT @Party_Cnt = party_cnt from Party_Address_Usage where address_cnt = @Address_Cnt
SELECT @Party_Type_Id = party_type_id from Party  WHERE party_cnt = @Party_Cnt

IF @Party_Type_Id IN (3,9,7,19,5,10,11)
BEGIN
	Select @UniqueID=a.UniqueId, @ScreenHierarchy = c.ScreenHierarchy, 
	@configuration_audit_master_id = configuration_audit_master_id from Address a
	INNER JOIN configuration_audit_master cam ON a.UniqueId = cam.UniqueId
	INNER JOIN Contact c ON c.contact_cnt = @Contact_Cnt
	WHERE a.address_cnt = @Address_Cnt
END

IF @Type = 'D'
BEGIN
INSERT INTO configuration_audit_details(
					configuration_audit_master_id,
                    Type, 
                    TableName, 
                    key_field_name,
                    key_field_value, 
					key_field_desc,
                    FieldName, 
					FieldDisplayName)
VALUES(
@configuration_audit_master_id,
'D',
'address_contact_usage',
'contact_cnt',
@Contact_Cnt,
@ScreenHierarchy,
'Contact Deleted',
'Contact Deleted')
END

IF @Type = 'I' AND EXISTS(SELECT 1 FROM configuration_audit_details WHERE configuration_audit_master_id=@configuration_audit_master_id AND TableName='address_contact_usage' 
			AND key_field_value=@Contact_Cnt AND FieldName = 'Contact Deleted')
BEGIN
	DELETE cad 
	FROM configuration_audit_details cad INNER JOIN
	#ins i ON i.contact_cnt = cad.key_field_value
	WHERE key_field_name = 'contact_cnt' AND Type = 'D'
END
ELSE IF @Type = 'I'
BEGIN
INSERT INTO configuration_audit_details(
					configuration_audit_master_id,
                    Type, 
                    TableName, 
                    key_field_name,
                    key_field_value, 
					key_field_desc,
                    FieldName, 
					FieldDisplayName)
VALUES(
@configuration_audit_master_id,
'I',
'address_contact_usage',
'contact_cnt',
@Contact_Cnt,
@ScreenHierarchy,
'New Contact Added',
'New Contact Added')
Return
END

