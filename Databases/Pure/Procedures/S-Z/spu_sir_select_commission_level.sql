SET ansi_nulls ON
GO
SET quoted_identifier ON
GO

EXECUTE Ddldropprocedure 'spu_sir_select_commission_level'
GO

CREATE PROCEDURE spu_sir_select_commission_level
AS
  BEGIN
      -- Insert statements for procedure here
      SELECT distinct cl.commission_level_id,
             description
	FROM   commission_level cl
             INNER JOIN commission_arrangement ca
               ON ca.commission_level_id = cl.commission_level_id
    WHERE    ca.is_deleted = 0       
  END

GO  