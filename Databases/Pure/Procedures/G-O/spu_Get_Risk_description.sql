SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Get_Risk_description'
GO


CREATE PROCEDURE spu_Get_Risk_description
    @Risk_Cnt int
AS

/****** Stored Procedure to extract description for Risk_Cnt and get Risk Tax  ******/
/****** Created by  : Ajit Kumar                   ******/
/****** Date        : 09/01/2001                   ******/
select description from risk where Risk_Cnt = @risk_cnt
GO


