EXECUTE DDLDropProcedure 'spu_SIR_Product_Claim_Event_Add'
GO

SET QUOTED_IDENTIFIER OFF
GO

SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_SIR_Product_Claim_Event_Add

    @claim_event_description_id INT

AS 

BEGIN
IF EXISTS(SELECT NULL FROM Product_Claim_Events WHERE claim_event_description_id=@claim_event_description_id)
	BEGIN
       DELETE FROM Product_Claim_Events WHERE claim_event_description_id=@claim_event_description_id
	END
INSERT INTO Product_Claim_Events (claim_event_description_id, product_id)
	SELECT DISTINCT @claim_event_description_id, product_id FROM product 
	WHERE is_deleted =0

END
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

