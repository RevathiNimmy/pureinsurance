SET QUOTED_IDENTIFIER ON 
SET ANSI_NULLS ON 
GO 

EXECUTE DDLDropTrigger 'tr_CurrencyRate_audit_log' 
GO 

CREATE TRIGGER tr_CurrencyRate_audit_log ON dbo.CurrencyRate 
FOR INSERT, UPDATE
AS
SET NOCOUNT ON;

-- Declare all the variables
DECLARE @UpdateDate VARCHAR(21);
DECLARE @UserId INT;
DECLARE @Type CHAR(1);
DECLARE @UniqueID VARCHAR(50);
DECLARE @ModuleName VARCHAR(100);
DECLARE @ModuleId INT;
DECLARE @ScreenHierarchy VARCHAR(500);
DECLARE @EffectiveDate DATETIME;

-- Set some of these variables
SELECT @ModuleId = 11, @ModuleName = 'Maintain Currency Rates'

SET @UpdateDate = CONVERT(NVARCHAR(30), GETDATE(), 126);
-- Action
IF EXISTS (SELECT * FROM INSERTED)
    IF EXISTS (SELECT * FROM DELETED)
        SET @Type = 'U'
    ELSE 
        SET @Type = 'I';

-- Get list of columns
SELECT * INTO #ins FROM INSERTED;

-- If INSERT or UPDATE, get values
IF @Type IN ('I', 'U')
    SELECT @UserId = UserId, @UniqueID = UniqueID, @ScreenHierarchy = ScreenHierarchy, @EffectiveDate = effective_from FROM #ins;

-- Ensure field name is not in excluded list
IF  @UniqueID IS NOT NULL
BEGIN 
    DECLARE @configuration_audit_master_id INT;

    IF @Type IN ('I', 'U')
        SELECT @configuration_audit_master_id = m.configuration_audit_master_id 
        FROM configuration_audit_master m 
        INNER JOIN #ins i ON m.UniqueId = i.UniqueId;

    IF @configuration_audit_master_id IS NULL
    BEGIN
        INSERT INTO configuration_audit_master (UniqueId, Module_Id, ModuleName, UpdateDate, UserID) 
        VALUES (@UniqueID, @ModuleId, @ModuleName, @UpdateDate, @UserId);
        
        SELECT @configuration_audit_master_id = SCOPE_IDENTITY();
    END;

    IF EXISTS (
        SELECT NULL 
        FROM configuration_audit_master cam 
        INNER JOIN configuration_audit_details cad 
        ON cam.configuration_audit_master_id = cad.configuration_audit_master_id 
        WHERE cam.configuration_audit_master_id = @configuration_audit_master_id 
        AND FieldName = 'Currency Rates Updated' AND cad.key_field_value = @EffectiveDate
    )
    RETURN;

    IF @Type IN ('I', 'U')
    BEGIN

		INSERT INTO configuration_audit_details (
				configuration_audit_master_id,
				Type, 
				TableName, 
				key_field_name,
				key_field_value, 
				key_field_desc,
				FieldName, 
				FieldDisplayName
				)
		SELECT 
				@configuration_audit_master_id,  
				'I',                             
				'CurrencyRate',                 
				'Effective From',                
				@EffectiveDate,                  
				@ScreenHierarchy,                
				'Currency Rates Updated',           
				'Currency Rates Updated';          
    END
END
SET NOCOUNT OFF;
GO
