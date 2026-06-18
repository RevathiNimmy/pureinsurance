
EXECUTE DDLDropProcedure 'spu_SIR_Product_MTA_Event_Add'
GO

SET QUOTED_IDENTIFIER OFF
GO

SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_SIR_Product_MTA_Event_Add

    @mta_event_description_id INT

AS 
BEGIN

IF EXISTS(SELECT NULL FROM Product_MTA_Events WHERE mta_event_description_id=@mta_event_description_id)
	BEGIN
       DELETE FROM Product_MTA_Events WHERE mta_event_description_id=@mta_event_description_id		
	END

INSERT INTO Product_MTA_Events (mta_event_description_id,product_id)
	SELECT DISTINCT @mta_event_description_id,product_id FROM product 
	WHERE is_deleted =0


END
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

