SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PartySupplierBusiness_del'
GO

CREATE PROCEDURE spu_PartySupplierBusiness_del
(
    @party_cnt              INT,
    @UserId                 INT = NULL,
    @UniqueId               VARCHAR(50) = NULL,
    @ScreenHierarchy        VARCHAR(500) = NULL
)
AS
BEGIN
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
		SELECT
			@Configuration_Audit_Master_Id,
			'D',
			'Party_Supplier_Business',
			'party_cnt',
			psb.party_cnt,
			@ScreenHierarchy + '/Supply',
			'supplier_business_id',
			'Supplier',
			sb.description AS OldValue,
			NULL           AS NewValue
		FROM Party_Supplier_Business psb
		JOIN Supplier_Business sb
			ON sb.supplier_business_id = psb.supplier_business_id
		WHERE psb.party_cnt = @party_cnt
		  AND psb.supplier_business_id IS NOT NULL

		UNION ALL

		SELECT
			@Configuration_Audit_Master_Id,
			'D',
			'Party_Supplier_Business',
			'party_cnt',
			psb.party_cnt,
			@ScreenHierarchy + '/supply(' + LTRIM(RTRIM(sb.description)) + ')',
			'supplier_speciality_id',
			'Speciality',
			ss.description AS OldValue,
			NULL AS NewValue
		FROM Party_Supplier_Business psb
		JOIN Supplier_Speciality ss
			ON ss.supplier_speciality_id = psb.supplier_speciality_id
		LEFT JOIN Supplier_Business sb
			ON sb.supplier_business_id = psb.supplier_business_id
		WHERE psb.party_cnt = @party_cnt
		  AND psb.supplier_speciality_id IS NOT NULL;


	END
	DELETE FROM Party_Supplier_Business WHERE party_cnt = @party_cnt
END
GO