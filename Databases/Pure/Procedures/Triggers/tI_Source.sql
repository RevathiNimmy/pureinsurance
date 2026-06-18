SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropTrigger 'tI_Source'
GO
/* AK 31072002 - add a subbranch for each branch */
/* DD 12/08/2002: Added auto generation of Structure Tree */
/* DD 28/08/2002: Only run duplicate tree if Multi-Tree Product Option enabled */
/* CMG/PB 10/09/2002: Add the system options for the new branch */

CREATE TRIGGER tI_Source ON Source FOR INSERT
AS
BEGIN
    DECLARE @source_id integer
    DECLARE @sub_branch_id integer
    DECLARE @Value VARCHAR(20)

    SELECT @source_id=source_id
    FROM inserted

    SELECT @sub_branch_id = ISNULL(MAX(sub_branch_id), 0) + 1 FROM Sub_Branch
    INSERT INTO Sub_Branch (
        sub_branch_id,
        source_id,
        caption_id,
        code,
        description,
        is_deleted,
        effective_date,
        reg_no_1,
        reg_no_2,
        address1,
        address2,
        address3,
        address4,
        postal_code,
        country_id,
        phone_area_code,
        phone_number,
        phone_extension,
        fax_area_code,
        fax_number,
        fax_extension,
        email,
        vat_no
    )
    SELECT
        @sub_branch_id,
        source_id,
        caption_id,
        code,
        description,
        is_deleted,
        getdate() - 1,
        reg_no_1,
        reg_no_2,
        address1,
        address2,
        address3,
        address4,
        postal_code,
        country_id,
        phone_area_code,
        phone_number,
        phone_extension,
        fax_area_code,
        fax_number,
        fax_extension,
        email,
        vat_no
        FROM inserted

    /*CMG/PB 10/09/2002 Copy system options for new branch */
    INSERT into system_options(
        branch_id,
        option_number,
        value,
        description)
    SELECT @source_id,
        option_number,
        value,
        description
    FROM system_options
    WHERE branch_id = 1

    /* DD 28/08/2002 */
    /* Get the Product Option for multi-tree accounting */
    SELECT
        @Value=Value
    FROM
        Hidden_options
    WHERE
        option_number=16

    IF @Value = '1'
    BEGIN
        EXEC spu_ACT_Do_StructureTree_Duplicate @company_id=1, @new_company_id=@source_id, @new_sub_branch_id=@sub_branch_id
    END
END
GO

