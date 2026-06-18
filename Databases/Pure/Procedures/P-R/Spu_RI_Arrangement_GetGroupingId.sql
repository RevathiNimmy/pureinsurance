SET QUOTED_IDENTIfIER OFF SET ANSI_NullS On
GO


Execute DDLDropProcedure 'Spu_RI_Arrangement_GetGroupingId'
GO

Create Procedure Spu_RI_Arrangement_GetGroupingId
@ri_arrangement_id int
AS
Select distinct grouping from ri_arrangement_line
where ri_arrangement_id = @ri_arrangement_id and  grouping is not null

GO