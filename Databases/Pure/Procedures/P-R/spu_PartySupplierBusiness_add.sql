SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PartySupplierBusiness_add'
GO

CREATE PROCEDURE spu_PartySupplierBusiness_add
(
    @party_cnt              INT,
    @supplier_speciality_id INT = NULL,
    @supplier_business_id   INT = NULL,
    @UserId                 INT = NULL,
    @UniqueId               VARCHAR(50) = NULL,
    @ScreenHierarchy        VARCHAR(500) = NULL
)
AS
BEGIN
    INSERT INTO Party_Supplier_Business
    (
        party_cnt,
        supplier_speciality_id,
        supplier_business_id
    )
    VALUES
    (
        @party_cnt,
        @supplier_speciality_id,
        @supplier_business_id
    );

    /* 2) Audit (only when UniqueId is provided) */
    IF @UniqueId IS NOT NULL
    BEGIN
        DECLARE @Configuration_Audit_Master_Id INT;
        DECLARE @UpdateDate VARCHAR(30);

        SET @UpdateDate = CONVERT(VARCHAR(30), GETDATE(), 126);

        SELECT @Configuration_Audit_Master_Id = cam.configuration_audit_master_id
        FROM configuration_audit_master cam
        WHERE cam.UniqueId = @UniqueId;

        IF @Configuration_Audit_Master_Id IS NULL
        BEGIN
            INSERT INTO configuration_audit_master
            (
                UniqueId,
                Module_Id,
                ModuleName,
                UpdateDate,
                UserID
            )
            VALUES
            (
                @UniqueId,
                21,
                'Other Party Maintenance',
                @UpdateDate,
                @UserId
            );

            SET @Configuration_Audit_Master_Id = SCOPE_IDENTITY();
        END;

		IF @supplier_business_id IS NOT NULL
		BEGIN
			DECLARE @BusinessDesc VARCHAR(255);

			SELECT @BusinessDesc = sb.description
			FROM Supplier_Business sb
			WHERE sb.supplier_business_id = @supplier_business_id;

			IF EXISTS
			(
				SELECT 1
				FROM configuration_audit_details d
				WHERE d.configuration_audit_master_id = @Configuration_Audit_Master_Id
				  AND d.TableName       = 'Party_Supplier_Business'
				  AND d.key_field_name  = 'party_cnt'
				  AND d.key_field_value = @party_cnt
				  AND d.FieldName       = 'supplier_business_id'
				  AND d.OldValue IS NOT NULL
				  AND d.OldValue        = @BusinessDesc
			)
			BEGIN
				DELETE d
				FROM configuration_audit_details d
				WHERE d.configuration_audit_master_id = @Configuration_Audit_Master_Id
				  AND d.TableName       = 'Party_Supplier_Business'
				  AND d.key_field_name  = 'party_cnt'
				  AND d.key_field_value = @party_cnt
				  AND d.FieldName       = 'supplier_business_id'
				  AND d.OldValue IS NOT NULL
				  AND d.OldValue        = @BusinessDesc;

				-- Do NOT insert
			END
			ELSE
			BEGIN
				INSERT INTO configuration_audit_details
				(
					configuration_audit_master_id,
					Type,
					TableName,
					key_field_name,
					key_field_value,
					key_field_desc,
					FieldName,
					FieldDisplayName,
					OldValue,
					NewValue
				)
				VALUES
				(
					@Configuration_Audit_Master_Id,
					'I',
					'Party_Supplier_Business',
					'party_cnt',
					@party_cnt,
					@ScreenHierarchy + '/Supply',
					'supplier_business_id',
					'Supplier',
					NULL,
					@BusinessDesc
				);
			END
		END;


		/* Speciality audit */
		IF @supplier_speciality_id IS NOT NULL
		BEGIN
			DECLARE @SpecialityDesc VARCHAR(255);
			DECLARE @supplyDesc VARCHAR(255);

			SELECT @SpecialityDesc = ss.description
			FROM Supplier_Speciality ss
			WHERE ss.supplier_speciality_id = @supplier_speciality_id;

   SELECT @supplyDesc = LTRIM(RTRIM(sb.description))
			FROM Supplier_Business sb
			WHERE sb.supplier_business_id = @supplier_business_id;

			IF EXISTS
			(
				SELECT 1
				FROM configuration_audit_details d
				WHERE d.configuration_audit_master_id = @Configuration_Audit_Master_Id
				  AND d.TableName       = 'Party_Supplier_Business'
				  AND d.key_field_name  = 'party_cnt'
				  AND d.key_field_value = @party_cnt
				  AND d.FieldName       = 'supplier_speciality_id'
				  AND d.key_field_desc  =  @ScreenHierarchy + '/supply(' + @supplyDesc + ')'
				  AND d.OldValue IS NOT NULL
				  AND d.OldValue        = @SpecialityDesc
			)
			BEGIN
				DELETE d
				FROM configuration_audit_details d
				WHERE d.configuration_audit_master_id = @Configuration_Audit_Master_Id
				  AND d.TableName       = 'Party_Supplier_Business'
				  AND d.key_field_name  = 'party_cnt'
				  AND d.key_field_value = @party_cnt
				  AND d.FieldName       = 'supplier_speciality_id'
				  AND d.key_field_desc  =  @ScreenHierarchy + '/supply(' + @supplyDesc + ')'
				  AND d.OldValue IS NOT NULL
				  AND d.OldValue        = @SpecialityDesc;

				-- Do NOT insert
			END
			ELSE
			BEGIN
				
				INSERT INTO configuration_audit_details
				(
					configuration_audit_master_id,
					Type,
					TableName,
					key_field_name,
					key_field_value,
					key_field_desc,
					FieldName,
					FieldDisplayName,
					OldValue,
					NewValue
				)
				VALUES
				(
					@Configuration_Audit_Master_Id,
					'I',
					'Party_Supplier_Business',
					'party_cnt',
					@party_cnt,
					@ScreenHierarchy + '/supply(' + @supplyDesc + ')',
					'supplier_speciality_id',
					'Speciality',
					NULL,
					@SpecialityDesc
				);
			END
		END;
    END;
END
GO
