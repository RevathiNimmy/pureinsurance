SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_get_controls_view'
GO


SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


---****** Object: Stored Procedure dbo.sp_get_controls_view Script Date: 19/04/00 16:53:30 ******/
CREATE PROCEDURE spu_get_controls_view
 @periltypeid int
AS
SELECT
 Null,	
 caption,
 type,
 display_order,
 mandatory,
 read_only,
 claim_party_type_id,
 claim_lookup_id,
 Null,
 peril_data_defn_id,
 description,
 1,
'General Details',
TabCount =  1
FROM peril_data_definition
WHERE peril_type_id=@periltypeid
ORDER BY peril_data_definition.Display_Order
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

