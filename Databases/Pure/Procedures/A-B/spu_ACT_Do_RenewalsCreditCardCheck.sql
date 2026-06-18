SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Do_RenewalsCreditCardCheck'
GO

-- This is called just before the debit stage in the renewals cycle to do a final check to see 
-- if we have a valid card we can try to take payment from. Most checks have already been done
-- during the Renewal Selection stage (in spu_ACT_Do_RenewalsCreditCardSetup) but since we had
-- not quoted at that stage we could not check that the renewal premium was within any card 
-- limits that had been setup for the mediatype issuer.  
--
-- Note that if we are not paying by card then we'll just return a flag indicating that and not
-- proceed any further. If insurance_file.cashlistitem_id has a value and 
-- insurance_file.cashlistitem_valid is 1 then this means we will try to debit on a card.
--
-- In this procedure we ensure that we have a valid card and a linked cashlistitem record, check
-- that the premium is within any ranges setup (and is not zero!) and if it is update the 
-- cashlistitem record to reflect the renewal premium. We then return a positive status back to 
-- the caller that will then try to authorise and take payment from the card.
--
-- If any of the above checks fail we will call a stored procedure to remove the previously
-- copied cashlist and cashlistitem records for the given insurance_file and return a negative
-- status back to the caller so that card processing will not even be attempted.

CREATE PROCEDURE spu_ACT_Do_RenewalsCreditCardCheck
    @renewal_insurance_file_cnt 	int,
    @is_card_payment_type               tinyint OUTPUT,
    @is_valid_card                      tinyint OUTPUT
AS

DECLARE @cashlistitem_valid 		tinyint,
        @cashlistitem_id 		int,
        @renewal_premium                numeric(19,4),
        @min_card_amount                numeric(19,4),
	@max_card_amount                numeric(19,4)

-- Get values to check later from the insurance_file record
SELECT  @cashlistitem_valid = cashlistitem_valid,
	@cashlistitem_id    = cashlistitem_id,
        @renewal_premium    = this_premium
FROM    insurance_file
WHERE   insurance_file_cnt  = @renewal_insurance_file_cnt

-- If not a card payment then do not continue!
IF (@cashlistitem_valid IS NULL) OR 
   (@cashlistitem_valid = 0) OR
   (@cashlistitem_id IS NULL)

   SELECT @is_card_payment_type = 0

ELSE
	
 BEGIN

   SELECT @is_card_payment_type = 1

   -- Get values to check later from the associated cashlistitem record
   SELECT  @min_card_amount    = min_amount,
           @max_card_amount    = max_amount
   FROM    mediatype_issuer mti
   INNER JOIN cashlistitem cli ON cli.mediatype_issuer_id = mti.mediatype_issuer_id
   WHERE   cli.cashlistitem_id = @cashlistitem_id

   -- Check if this renewal policy version has a renewal premium that is not zero!!! and that 
   -- the renewal premium is within the issuer's range (if range's have been specified)
   IF (@renewal_premium <> 0) AND
      ((@min_card_amount IS NULL OR @renewal_premium >= @min_card_amount) AND (@max_card_amount IS NULL OR @renewal_premium <= @max_card_amount))
       BEGIN

          -- Update the amount on the cashlistitem to be the renewal premium 
          UPDATE cashlistitem
          SET    amount = @renewal_premium
          WHERE  cashlistitem_id = @cashlistitem_id
       
          -- Set the return flag as TRUE - i.e. we have valid card details we can try to take payment from
          SET @is_valid_card = 1

       END

   ELSE

       BEGIN

          -- Set the return flag as FALSE - i.e. we have NOT got valid card details so don't try to take payment
          SET @is_valid_card = 0

          -- Call a sp to delete the previously (at selection stage) copied over cashlist and cashlistitem records 
          EXEC spu_ACT_Update_PolicyCashListItem @renewal_insurance_file_cnt, 0

       END
  END
GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO



