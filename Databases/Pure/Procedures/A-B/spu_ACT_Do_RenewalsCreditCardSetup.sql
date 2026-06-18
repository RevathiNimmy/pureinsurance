SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Do_RenewalsCreditCardSetup'
GO

/*
Action Query to prepare a given renewal policy version for possible card processing at the
debit stage in the renewals cycle. 

Basically this procedure checks if the previous policy version has a cashlistitem record
attatched to it that has a mediatype which is linked to a mediatype_issuer entry which is 
linked to a mediatype_connector. If so then we check if the card on that record is still 
valid and if the card collector can still be used etc. If it can't or there was no 
cashlistitem record then we set a flag on the insurance_file record (for the renewal) 
indicating that no valid cashlistitem exists. If we have a valid card though then we copy 
the cashlist and cashlistitem records from the previous policy version and link them to the 
renewal policy version and set the flag on the insurance_file to indicate that we do have a 
valid cashlistitem record to use at a later stage for payment.
*/

CREATE PROCEDURE spu_ACT_Do_RenewalsCreditCardSetup
    @old_insurance_file_cnt INT,
    @renewal_insurance_file_cnt INT
AS

DECLARE 
    @old_cashlistitem_id INT,
    @renewal_cashlistitem_id INT,
    @old_cashlist_id INT,
    @renewal_cashlist_id INT,
    @renewal_date DATETIME

/*Get the cashlistitem_id on the old (previous) insurance_file [THIS IS OPTIONAL]*/
SELECT
    @old_cashlistitem_id = i.cashlistitem_id
FROM insurance_file i
JOIN cashlistitem cli
    ON cli.cashlistitem_id = i.cashlistitem_id
JOIN mediatype mt
    ON mt.mediatype_id = cli.mediatype_id
JOIN mediatype_issuer mti 
    ON mti.mediatype_id = mt.mediatype_id
WHERE  i.insurance_file_cnt = @old_insurance_file_cnt
AND ISNULL(mti.mediatype_connector_id,0) <> 0


/*If no cashlistitem_id then no card details to check and possibly carry over*/
IF @old_cashlistitem_id IS NULL
BEGIN
    /*Just set flag on insurance_file indicating there is not a card (as there wasn't one on the last version)*/
    UPDATE insurance_file
    SET cashlistitem_valid = 0, 
        cashlistitem_id = NULL
    WHERE insurance_file_cnt = @renewal_insurance_file_cnt
END
ELSE 
BEGIN
    /*
    The previous version of the policy has a linked cashlistitem record.
    Do checks now to see if we can debit on the same card that was used.
    If we can then we'll copy over the cashlist and cashlistitem records
    and link them to this renewal policy version.
    */
    SELECT 
        @renewal_date = renewal_date
    FROM insurance_file
    WHERE insurance_file_cnt = @renewal_insurance_file_cnt

    /*Handle cc_expiry_date containing no values (except null), but not null fields.*/
    IF EXISTS
        (
            SELECT 
                NULL
            FROM cashlistitem cli
            JOIN mediatype_issuer mti 
                ON mti.mediatype_issuer_id = cli.mediatype_issuer_id
            JOIN mediatype_connector mtc 
                ON mtc.mediatype_connector_id = mti.mediatype_connector_id
            WHERE cli.cashlistitem_id = @old_cashlistitem_id
            AND CHARINDEX(CHAR(0),cli.cc_expiry_date) <> 1
            AND (cli.is_reversed <> 1 OR cli.is_reversed IS NULL)
            AND mti.is_deleted = 0
            AND mti.is_allowed = 1
            AND mtc.is_deleted = 0   
        )
    BEGIN
        SELECT
            @old_cashlistitem_id = cli.cashlistitem_id
        FROM cashlistitem cli
        JOIN mediatype_issuer mti 
            ON mti.mediatype_issuer_id = cli.mediatype_issuer_id
        JOIN mediatype_connector mtc 
            ON mtc.mediatype_connector_id = mti.mediatype_connector_id
        WHERE cli.cashlistitem_id = @old_cashlistitem_id
        AND DATEADD(MONTH, 1, CAST(LEFT(cli.cc_expiry_date,2) + '/01/' + SUBSTRING(cli.cc_expiry_date,4,2) as datetime)) >= @renewal_date 
        AND (cli.is_reversed <> 1 OR cli.is_reversed IS NULL)
        AND mti.is_deleted = 0
        AND mti.is_allowed = 1
        AND mtc.is_deleted = 0
    END
    ELSE
    BEGIN
        SELECT @old_cashlistitem_id = NULL
    END
     
    IF @old_cashlistitem_id IS NULL
    BEGIN
        /*Just set flag on insurance_file indicating there is not a card (as we found no valid card)*/
        UPDATE insurance_file
        SET cashlistitem_valid = 0, 
            cashlistitem_id = NULL
        WHERE insurance_file_cnt = @renewal_insurance_file_cnt
    END
    ELSE 
    BEGIN
        /*Copy the cashlist record (the parent of the cashlistitem) linked to the old insurance_file */
        SELECT 
            @old_cashlist_id = cashlist_id
        FROM cashlistitem
        WHERE cashlistitem_id = @old_cashlistitem_id

		
        /*Copy the cashlist record linked to the old insurance_file*/
        INSERT INTO cashlist 
        (
            bankaccount_id,
            cashlisttype_id,
            cashliststatus_id, 
            cashlist_ref,
            company_id,
            sub_branch_id,
            currency_id,
            list_date,
            control_total,
            item_count,
            cashlist_drawer_id,
            batch_id,
            pmuser_id,
            confirm_pmuser_id,
            confirm2_pmuser_id,
            date_approved,
            banking_total,
            cash_float_amount,
            date_deposited,
            base_currency_id
        )
        SELECT 
            bankaccount_id,
            cashlisttype_id,
            1, /*Set cashliststatus_id to "Entered"*/
            cashlist_ref,
            company_id,
            sub_branch_id,
            currency_id,
            list_date,
            control_total,
            item_count,
            cashlist_drawer_id,
            batch_id,
            pmuser_id,
            confirm_pmuser_id,
            confirm2_pmuser_id,
            date_approved,
            banking_total,
            cash_float_amount,
            date_deposited,
            base_currency_id
        FROM cashlist
        WHERE cashlist_id = @old_cashlist_id
		
		SELECT @renewal_cashlist_id=@@IDENTITY

        /*Copy the cashlistitem record linked to the old insurance_file and link to the above newly copied cashlist record*/
		
        INSERT INTO cashlistitem 
        (
            allocationstatus_id, 
            mediatype_id,
            cashlist_id,
            account_id,
            media_ref,
            our_ref,                        
            their_ref,                     
            amount,            
            transdetail_id, 
            contact_name,                                                 
            address1,                                 
            address2,                                 
            address3,                                 
            address4,                                 
            postal_code,          
            address_country, 
            payment_name,                                                 
            payment_account_code,                                         
            payment_branch_code,            
            payment_expiry_date,                                    
            payment_reference1,             
            payment_reference2,             
            letter,
            transaction_date,                                       
            cashlistitem_receipt_type_id, 
            cashlistitem_receipt_status_id,
            cashlistitem_payment_type_id, 
            cashlistitem_payment_status_id, 
            cashlistitem_bank_id, 
            cheque_date,                                            
            cc_number,                      
            cc_expiry_date, 
            cc_start_date, 
            cc_issue, 
            cc_pin,               
            cc_auth_code,                                       
            original_amount,       
            amount_tendered,       
            change,                
            batch_id,    
            pmuser_id, 
            receipt_details,                                                                                                                                                                                                                                                  
            cashlistitem_reverse_pmuser_id, 
            cashlistitem_reverse_reason_id, 
            date_presented,                                         
            cheque_in_possession, 
            stop_requested_date,                                    
            stop_printed_date,                                      
            stop_confirmation_date,                                 
            reason,                                                                                                                                                                                                                                                           
            replaces_cashlistitem_id, 
            superceded_by_id, 
            xml_object,                                                                                                                                                                                                                                                       
            cheque_reminder_print_date,                             
            underwriting_year_id, 
            exchange_rate_override_reason_id, 
            currency_base_xrate,                                   
            currency_base_date,                                     
            account_base_xrate,                                    
            account_base_date,                                      
            system_base_xrate,                                     
            system_base_date,                                       
            is_reversed,
            mediatype_issuer_id, 
            cc_name,                                            
            cc_customer,                                        
            cc_manual_auth_code,                                
            cc_transaction_code,
			MediaType_Status_id --Sankar - (WPRvb64 Media Type Status) - Paralleling
        )
        SELECT 
            1, /*Set allocationstatus_id to "Unallocated"*/
            mediatype_id,
            @renewal_cashlist_id, /*Set parent to the cashlist we copied before*/
            account_id,
            media_ref,
            our_ref,                        
            their_ref,                     
            amount,            
            NULL, /*Set transdetail_id to NULL*/
            contact_name,                                                 
            address1,                                 
            address2,                                 
            address3,                                 
            address4,                                 
            postal_code,          
            address_country, 
            payment_name,                                                 
            payment_account_code,                                         
            payment_branch_code,            
            payment_expiry_date,                                    
            payment_reference1,             
            payment_reference2,             
            letter,
            transaction_date,                                       
            cashlistitem_receipt_type_id, 
            cashlistitem_receipt_status_id, 
            cashlistitem_payment_type_id, 
            cashlistitem_payment_status_id, 
            cashlistitem_bank_id, 
            cheque_date,                                            
            cc_number,                      
            cc_expiry_date, 
            cc_start_date, 
            cc_issue, 
            cc_pin,               
            NULL, /*Set cc_auth_code to NULL*/
            original_amount,       
            amount_tendered,       
            change,                
            batch_id,    
            pmuser_id, 
            receipt_details,                                                                                                                                                                                                                                                  
            cashlistitem_reverse_pmuser_id, 
            cashlistitem_reverse_reason_id, 
            date_presented,                                         
            cheque_in_possession, 
            stop_requested_date,                                    
            stop_printed_date,                                      
            stop_confirmation_date,                                 
            reason,                                                                                                                                                                                                                                                           
            replaces_cashlistitem_id, 
            superceded_by_id, 
            xml_object,                                                                                                                                                                                                                                                       
            cheque_reminder_print_date,                             
            underwriting_year_id, 
            exchange_rate_override_reason_id, 
            currency_base_xrate,                                   
            currency_base_date,                                     
            account_base_xrate,                                    
            account_base_date,                                      
            system_base_xrate,                                     
            system_base_date,                                       
            is_reversed,
            mediatype_issuer_id, 
            cc_name,                                            
            cc_customer,                                        
            NULL, /*Set cc_manual_auth_code to NULL*/
            NULL, /*Set cc_transaction_code to NULL*/
			MediaType_Status_id --Sankar - (WPRvb64 Media Type Status) - Paralleling
        FROM cashlistitem
        WHERE cashlistitem_id = @old_cashlistitem_id                                                                                                                                                                                                                                                  

		SELECT @renewal_cashlistitem_id=@@IDENTITY

        /*Link the above newly created cashlistitem record to the new insurance_file*/
        /*Set flag on insurance_file indicating there IS a valid card*/
        UPDATE insurance_file
        SET cashlistitem_id = @renewal_cashlistitem_id,  
            cashlistitem_valid = 1
        WHERE insurance_file_cnt = @renewal_insurance_file_cnt
    END
END
    
GO




