if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spu_ACT_Sel_Locale]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spu_ACT_Sel_Locale]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_ACT_Sel_Locale
@countryid int
AS
select
code
from
country
where
country_id = @countryid

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

