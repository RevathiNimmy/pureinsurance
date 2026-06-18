if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spu_PMB_Policy_Fee_Del]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spu_PMB_Policy_Fee_Del]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE spu_PMB_Policy_Fee_Del
	@insurance_file_cnt int
AS
BEGIN
	DELETE FROM policy_fee
	WHERE [insurance_file_cnt] = @insurance_file_cnt
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

