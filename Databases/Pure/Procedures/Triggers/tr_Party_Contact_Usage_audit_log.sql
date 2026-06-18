SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
EXECUTE DDLDropTrigger 'tr_Party_Contact_Usage_audit_log'
GO
CREATE TRIGGER tr_Party_Contact_Usage_audit_log ON dbo.Party_Contact_Usage 
    FOR INSERT,DELETE 
AS
SET NOCOUNT ON;

/*declare all the variables*/
DECLARE @Party_Cnt INT
DECLARE @ScreenHierarchy VARCHAR(500);
DECLARE @Configuration_Audit_Master_Id INT
DECLARE @Contact_Cnt INT
DECLARE @UniqueID VARCHAR(50)
DECLARE @Type VARCHAR(10)
DECLARE @Party_Type_Id INT

IF EXISTS (SELECT * FROM INSERTED)
	SET @Type = 'I'
ELSE 
	SET @Type = 'D'

SELECT * INTO #ins FROM INSERTED;

SELECT * INTO #del FROM DELETED;

IF @Type = 'D'
	SELECT @Party_Cnt=party_cnt, @Contact_Cnt = contact_cnt FROM #del
ELSE
	SELECT @Party_Cnt=party_cnt, @Contact_Cnt = contact_cnt FROM #ins

SELECT @Party_Type_Id = party_type_id from Party  WHERE party_cnt = @Party_Cnt

IF @Party_Type_Id = 3
BEGIN
	Select @UniqueID=pa.UniqueId, @ScreenHierarchy = c.ScreenHierarchy, 
	@configuration_audit_master_id = configuration_audit_master_id from Party_Agent pa 
	INNER JOIN configuration_audit_master cam ON pa.UniqueId = cam.UniqueId
	INNER JOIN Contact c ON c.contact_cnt = @Contact_Cnt
	WHERE pa.party_cnt = @Party_Cnt
END
ElSE IF @Party_Type_Id IN (5,6,9,7,10,11,19)
BEGIN
	Select @UniqueID=p.UniqueId, @ScreenHierarchy = c.ScreenHierarchy, 
	@configuration_audit_master_id = configuration_audit_master_id from Party p 
	INNER JOIN configuration_audit_master cam ON p.UniqueId = cam.UniqueId
	INNER JOIN Contact c ON c.contact_cnt = @Contact_Cnt
	WHERE p.party_cnt = @Party_Cnt
END

IF @Type = 'D' AND ISNULL(@configuration_audit_master_id,0) <> 0
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
'party_contact_usage',
'contact_cnt',
@Contact_Cnt,
@ScreenHierarchy,
'Contact Deleted',
'Contact Deleted')
END

IF EXISTS(SELECT NULL FROM configuration_audit_master cam 
		INNER JOIN configuration_audit_details cad ON cam.configuration_audit_master_id=cad.configuration_audit_master_id 
		WHERE cam.configuration_audit_master_id=@configuration_audit_master_id AND (FieldName IN ('New Agent Added', 'New Fee Added', 'New Re-Insurer Added', 'New Account Executive Added', 'New Account Handler Added', 'New Discount Account Added', 'New Extra Account Added', 'New Agent Group Added')))
RETURN

IF @Type = 'I' AND EXISTS(SELECT 1 FROM configuration_audit_details WHERE configuration_audit_master_id=@configuration_audit_master_id AND TableName='party_contact_usage' 
			AND key_field_value=@Contact_Cnt AND FieldName = 'Contact Deleted')
BEGIN
	DELETE cad 
	FROM configuration_audit_details cad INNER JOIN
	#ins i ON i.contact_cnt = cad.key_field_value
	WHERE key_field_name = 'contact_cnt' AND Type = 'D'
END
ELSE IF @Type = 'I' AND ISNULL(@configuration_audit_master_id,0) <> 0
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
'party_contact_usage',
'contact_cnt',
@Contact_Cnt,
@ScreenHierarchy,
'New Contact Added',
'New Contact Added')
Return
END
