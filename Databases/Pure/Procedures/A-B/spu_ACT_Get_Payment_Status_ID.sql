if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spu_ACT_Get_Payment_Status_ID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spu_ACT_Get_Payment_Status_ID]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE spu_ACT_Get_Payment_Status_ID
@code varchar(30)
as
select
cashlistitem_payment_status_id
from
cashlistitem_payment_status
where
code = @code

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

