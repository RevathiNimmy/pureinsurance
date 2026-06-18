SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Do_StructureTree_Duplicate'
GO

CREATE PROCEDURE spu_ACT_Do_StructureTree_Duplicate
(
    @company_id INT,
    @new_company_id INT,
    @new_sub_branch_id INT
)

AS

/*
Duplicates the core_nodes ( = 1) from the Structure tree under company_id to a new
tree under new_company_id. Called by the Insert Source Trigger

DD  12/08/2002 - Created
PWF 13/09/2002 - Cleaned up a bit
PWF 18/09/2002 - Fixed a number of issues relating to duplication
*/

/* Declare what we need */
DECLARE 
    @node_id INT, 
    @new_node_id INT,
    @mapping_id INT, 
    @new_mapping_id INT,
    @account_id INT, 
    @new_account_id INT,
    @element_id INT, 
    @new_element_id INT,
    @parent_node_id INT, 
    @new_parent_node_id INT,
    @period_id INT,
	@new_ledger_id INT

BEGIN TRANSACTION

/* We will use this later to build the tree structure */
CREATE TABLE #nodes (
    node_id INT,
    new_node_id INT,
    mapping_id INT,
    new_mapping_id INT)

/* Get the core nodes */
DECLARE Tree_Cursor CURSOR FAST_FORWARD FOR
    SELECT   node_id, mapping_id, account_id, element_id, parent_node_id
    FROM     StructureTree
    WHERE    core_node = 1 
    AND      company_id = @company_id
    ORDER BY node_id

OPEN Tree_Cursor

FETCH NEXT FROM Tree_Cursor
    INTO @node_id, @mapping_id, @account_id, @element_id, @parent_node_id

/* Loop through them */
WHILE @@FETCH_STATUS = 0
BEGIN
    SELECT @new_mapping_id = NULL,
           @new_account_id = NULL,
           @new_element_id = NULL

    IF @mapping_id IS NOT NULL
    BEGIN
        /* Copy the Mapping record */
        INSERT INTO Mapping (
                    company_id, maptype_id, description)
            SELECT  @new_company_id, maptype_id, description
            FROM    Mapping 
            WHERE   mapping_id = @mapping_id
        SELECT @new_mapping_id = @@IDENTITY

        /* Ledgers have already been copied, fix them to the new mapping here */
        UPDATE Ledger 
        SET    mapping_id = @new_mapping_id
        WHERE  mapping_id = @mapping_id 
        AND    sub_branch_id = @new_sub_branch_id
    END

    IF @account_id IS NOT NULL
    BEGIN
		/* Copy the Account record */
        INSERT INTO Account (
                    purgefrequency_id, accountstatus_id, company_id, sub_branch_id,
                    currency_id, accounttype_id, paymenttype_id, ledger_id, account_name, 
                    short_code, restrict_enquiry, restrict_update, delete_at_purge, 
                    contact_name, address1, address2, address3, address4, postal_code, 
                    address_country, phone_area_code, phone_number, phone_extension, 
                    fax_area_code, fax_number, fax_extension, payment_name, payment_account_code, 
                    payment_branch_code, payment_expiry_date, payment_reference1, payment_reference2, 
                    credit_limit, discount_percentage, settlement_period, bank_name, bank_address1, 
                    bank_address2, bank_address3, bank_address4, bank_postal_code, bank_country,
                    bank_phone_area_code, bank_phone_number, bank_phone_extension, bank_fax_area_code,
                    bank_fax_number, bank_fax_extension, comments, account_key, nominal_account_id)
            SELECT  purgefrequency_id, accountstatus_id, @new_company_id, @new_sub_branch_id,
                    currency_id, accounttype_id, paymenttype_id, 
					(SELECT L2.ledger_id 
					FROM Account A
					JOIN Ledger L ON L.ledger_id=A.ledger_id
					JOIN Ledger L2 ON L2.ledger_short_name=L.ledger_short_name AND L2.company_id=@new_company_id
					WHERE A.account_id=@account_id), 
					account_name, 
                    short_code, restrict_enquiry, restrict_update, delete_at_purge, 
                    contact_name, address1, address2, address3, address4, postal_code, 
                    address_country, phone_area_code, phone_number, phone_extension, 
                    fax_area_code, fax_number, fax_extension, payment_name, payment_account_code, 
                    payment_branch_code, payment_expiry_date, payment_reference1, payment_reference2, 
                    credit_limit, discount_percentage, settlement_period, bank_name, bank_address1, 
                    bank_address2, bank_address3, bank_address4, bank_postal_code, bank_country,
                    bank_phone_area_code, bank_phone_number, bank_phone_extension, bank_fax_area_code,
                    bank_fax_number, bank_fax_extension, comments, account_key, nominal_account_id
            FROM    Account 
            WHERE   account_id = @account_id
        SELECT @new_account_id = @@IDENTITY
    END

    IF @element_id IS NOT NULL
    BEGIN
        /* Copy the Element Record */
        INSERT INTO Element (
                    element_name, parent_id)
            SELECT  element_name, parent_id 
            FROM    Element 
            WHERE   element_id = @element_id
        SELECT @new_element_id = @@IDENTITY

        /* Copy the Element Extra Record */
        INSERT INTO ElementExtras (
                    element_id, totalling_id, description, report_map_id,
                    account_map_id, spare_number, spare_text, is_deletable)
            SELECT  @new_element_id, totalling_id, description, report_map_id,
                    account_map_id, spare_number, spare_text, is_deletable
            FROM    ElementExtras
            WHERE   element_id = @element_id
    END

    /* Insert the new Structure Tree Record */
    INSERT INTO StructureTree (
                company_id, mapping_id, account_id, element_id, parent_node_id, core_node)
        VALUES (@new_company_id, @new_mapping_id, @new_account_id, @new_element_id, @parent_node_id, NULL)
    SELECT @new_node_id = @@IDENTITY

    /*
    Hold a copy of the old node -> new node. This will be used to
    build the parent node structure.
     */
    INSERT INTO #nodes (
                node_id, new_node_id)
        VALUES (@node_id, @new_node_id)

    /* Move onto the next record */
    FETCH NEXT FROM Tree_Cursor
        INTO @node_id, @mapping_id, @account_id, @element_id, @parent_node_id
END

CLOSE Tree_Cursor
DEALLOCATE Tree_Cursor

/* Fix the parent nodes */
UPDATE StructureTree
SET    StructureTree.parent_node_id = (SELECT new_node_id 
                                       FROM   #nodes 
                                       WHERE  node_id = StructureTree.parent_node_id)
WHERE  company_id = @new_company_id
AND    parent_node_id > 0


DROP TABLE #nodes

COMMIT TRANSACTION
GO
